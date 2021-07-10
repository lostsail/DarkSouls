using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static InventoryManager instance;
    public static InventoryManager s_instance
    {
        get { return instance; }
    }
    public Inventory myBag;
    public GameObject slotGrid;
    public Button useButton;
    public Slot slotPrefab;
    public Text itemDescription;
    public GameObject selectedPrefab;
    public GameObject weaponHandle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if(instance!=this)
                Destroy(gameObject);
        }
    }

    public static void RefreshInventory()
    {
        Debug.Log("Refresh");
        RefreshItem();
        instance.itemDescription.text = "";
        instance.useButton.interactable = false;
    }

    public static void UpdateItemDescription(string itemDesc)
    {
        instance.itemDescription.text = itemDesc;
    }

    public static void CreateNewItem(Item item)
    {
        Slot newItem=Instantiate(instance.slotPrefab, instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemPic;
    }

    public static void RefreshItem()
    {
        foreach (Transform child in instance.slotGrid.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in instance.myBag.list)
        {
            CreateNewItem(item);
        }
    }

    public static void UseItem()
    {
        if (instance.selectedPrefab != null)
        {
            Instantiate(instance.selectedPrefab,instance.weaponHandle.transform);
        }
    }
}
