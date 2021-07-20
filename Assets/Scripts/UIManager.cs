using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                uiManager.PickUpUI.SetActive(false);
            }
        }

        //Todo 增加选取指定物品的功能 
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
                    Debug.Log("huitan");
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

    private static void ReFreshCollectList()
    {
        uiManager.selectedItem = uiManager.collectionList[uiManager.collectIndex];

        foreach (Transform child in uiManager.CollectableList.transform)
        {
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (ItemOnWorld item in uiManager.collectionList)
        {
            GameObject newItem = Instantiate(uiManager.CollectPrefab, uiManager.CollectableList.transform);
            newItem.name = item.name;
            newItem.transform.GetChild(1).GetComponent<Text>().text = item.name;

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

    private static void BagUIControl()
    {
        if (uiManager.pi.BagSignal)
        {
            uiManager.BagUI.SetActive(!uiManager.BagUI.activeSelf);
            uiManager.pi.BagSignal = false;
        }

        if (uiManager.BagUI.activeSelf == true && uiManager.isBagOpen == false)
        {
            Time.timeScale = 0;
            uiManager.pi.inputEnable = false;
            InventoryManager.RefreshInventory();
            uiManager.isBagOpen = true;
        }
        else if (uiManager.BagUI.activeSelf == false && uiManager.isBagOpen == true)
        {
            Time.timeScale = 1;
            uiManager.pi.inputEnable = true;
            uiManager.isBagOpen = false;
        }
    }

    public static void CollectItem(ItemOnWorld item)
    {
        uiManager.collectionList.Add(item);
    }

    public static void RemoveItem(ItemOnWorld item)
    {
        int temp = uiManager.collectionList.FindIndex(x => x == item);
        if (temp < uiManager.collectIndex)
            uiManager.collectIndex--;

        uiManager.collectionList.Remove(item);
    }

    public static void PickUpItem(ItemOnWorld item)
    {
        item.inventory.list.Add(item.item);
        InventoryManager.CreateNewItem(item.item);
        Destroy(item.gameObject);
        uiManager.collectionList.Remove(item);
    }
}
