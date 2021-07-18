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

        uiManager.pi = Player.GetComponent<PlayerInput>();
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

    private void PickUpUIControl()
    {
        if (collectAmount!=collectionList.Count)
        {
            collectAmount = collectionList.Count;

            if (collectionList.Count != 0)
            {
                foreach (Transform child in uiManager.CollectableList.transform)
                {
                    Destroy(child.gameObject);
                }

                foreach (ItemOnWorld item in collectionList)
                {
                    GameObject newItem=Instantiate(CollectPrefab, uiManager.CollectableList.transform);
                    newItem.transform.GetChild(1).GetComponent<Text>().text = item.name;
                }
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
                PickUpItem(collectionList[0]);
            } 
        }
    }

    private void BagUIControl()
    {
        if (pi.BagSignal)
        {
            uiManager.BagUI.SetActive(!BagUI.activeSelf);
            uiManager.pi.BagSignal = false;
        }

        if (BagUI.activeSelf == true && isBagOpen == false)
        {
            Time.timeScale = 0;
            uiManager.pi.inputEnable = false;
            InventoryManager.RefreshInventory();
            uiManager.isBagOpen = true;
        }
        else if (BagUI.activeSelf == false && isBagOpen == true)
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
