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
            UIManager.CollectItem(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UIManager.RemoveItem(this);
    }
}
