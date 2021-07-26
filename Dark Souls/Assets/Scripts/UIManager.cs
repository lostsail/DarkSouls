using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 管理UI组件
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;

    [SerializeField] GameObject Player;
    [SerializeField] GameObject BagUI;
    [SerializeField] GameObject PickUpUI;
    [SerializeField] GameObject CollectPrefab;
    [SerializeField] GameObject CollectableList;
    PlayerInput pi;
    List<ItemOnWorld> collectionList;

    bool isBagOpen;
    int collectAmount;
    int collectIndex;
    float collectTime;
    ItemOnWorld selectedItem;

    private void Awake()
    {
        if (!uiManager)
        {
            uiManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        uiManager.pi = uiManager.Player.GetComponent<PlayerInput>();
        uiManager.collectionList = new List<ItemOnWorld>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BagUIControl();
        PickUpUIControl();
    }

    /// <summary>
    /// 管理拾取物品的UI
    /// </summary>
    private static void PickUpUIControl()
    {
        if (uiManager.collectAmount != uiManager.collectionList.Count)
        {
            uiManager.collectAmount = uiManager.collectionList.Count;

            if (uiManager.collectionList.Count != 0)
            {
                ReFreshCollectList();
                uiManager.PickUpUI.SetActive(true);
            }
            else
            {
                for (int i = uiManager.CollectableList.transform.childCount - 1; i >= 0; i--)
                {
                    PoolManager.ReturnPool(uiManager.CollectableList.transform.GetChild(i).gameObject);
                }
                uiManager.PickUpUI.SetActive(false);
            }
        }

        if (uiManager.PickUpUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                PickUpItem(uiManager.selectedItem);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && Time.time - uiManager.collectTime > 0.2f)
            {
                uiManager.collectTime = Time.time;
                if (uiManager.collectIndex > 0)
                {
                    uiManager.collectIndex--;
                    ReFreshCollectList();
                }
                else
                {
                    uiManager.PickUpUI.transform.GetChild(0).Translate(Vector3.down * 20);
                }



            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && Time.time - uiManager.collectTime > 0.2f)
            {
                uiManager.collectTime = Time.time;
                if (uiManager.collectIndex < uiManager.collectionList.Count - 1)
                {
                    uiManager.collectIndex++;
                    ReFreshCollectList();
                }
                else
                    uiManager.PickUpUI.transform.GetChild(0).Translate(Vector3.up * 20);
            }
        }
    }

    /// <summary>
    /// 刷新拾取列表
    /// </summary>
    private static void ReFreshCollectList()
    {
        if (uiManager.collectIndex > uiManager.collectionList.Count - 1)
            uiManager.collectIndex = uiManager.collectionList.Count - 1;

            uiManager.selectedItem = uiManager.collectionList[uiManager.collectIndex];

        for (int j = uiManager.CollectableList.transform.childCount - 1; j >= 0; j--)
        {
            PoolManager.ReturnPool(uiManager.CollectableList.transform.GetChild(j).gameObject);
        }

        int i = 0;
        foreach (ItemOnWorld item in uiManager.collectionList)
        {
            GameObject newItem = PoolManager.GetFromPool(uiManager.CollectPrefab);
            newItem.transform.SetParent(uiManager.PickUpUI.transform.GetChild(0));
            newItem.transform.GetChild(1).GetComponent<Text>().text = item.item.itemName;

            if (i == uiManager.collectIndex)
            {
                newItem.transform.GetChild(0).gameObject.SetActive(true);
            }

            i++;
        }

        if (i > 1)
        {
            uiManager.PickUpUI.transform.GetChild(1).GetComponent<Scrollbar>().value = 1 - (float)(uiManager.collectIndex) / (i - 1);
        }
    }

    /// <summary>
    /// 管理背包UI
    /// </summary>
    private static void BagUIControl()
    {
        if (uiManager.pi.BagSignal)
        {
            uiManager.BagUI.SetActive(!uiManager.BagUI.activeSelf);
            uiManager.pi.BagSignal = false;
        }

        if (uiManager.BagUI.activeSelf == true && uiManager.isBagOpen == false)
        {
            uiManager.pi.perspectiveEnable = false;
            Time.timeScale = 0;
            Cursor.visible = true;
            uiManager.pi.inputEnable = false;
            InventoryManager.RefreshInventory();
            uiManager.isBagOpen = true;
        }
        else if (uiManager.BagUI.activeSelf == false && uiManager.isBagOpen == true)
        {
            uiManager.pi.perspectiveEnable = true;
            Time.timeScale = 1;
            Cursor.visible = false;
            uiManager.pi.inputEnable = true;
            uiManager.isBagOpen = false;
        }
    }

    /// <summary>
    /// 将物品添加至拾取列表
    /// </summary>
    /// <param name="item"></param>
    public static void CollectItem(ItemOnWorld item)
    {
        uiManager.collectionList.Add(item);
    }

    /// <summary>
    /// 将物品移出拾取列表
    /// </summary>
    /// <param name="item"></param>
    public static void RemoveItem(ItemOnWorld item)
    {
        int temp = uiManager.collectionList.FindIndex(x => x == item);
        if (temp < uiManager.collectIndex)
        {
            uiManager.collectIndex--;
        }


        uiManager.collectionList.Remove(item);
    }

    /// <summary>
    /// 将物品加入背包
    /// </summary>
    /// <param name="item"></param>
    public static void PickUpItem(ItemOnWorld item)
    {
        item.inventory.list.Add(item.item);
        InventoryManager.CreateNewItem(item.item);
        Destroy(item.gameObject);
        uiManager.collectionList.Remove(item);
    }
}
