using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image slotImage;

    private void Awake()
    {
        slotImage = GetComponent<Image>();
    }

    public void ItemOnClick()
    {
        Debug.Log("click");
        InventoryManager.UpdateItemDescription(slotItem.description);
        InventoryManager.s_instance.useButton.interactable = true;
        InventoryManager.s_instance.selectedPrefab = slotItem.itemPrefab;
    }
}