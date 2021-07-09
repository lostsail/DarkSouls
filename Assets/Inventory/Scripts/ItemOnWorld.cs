using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item item;
    public Inventory inventory;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inventory.list.Add(item);
            InventoryManager.CreateNewItem(item);
            Destroy(gameObject);
        }
    }
}
