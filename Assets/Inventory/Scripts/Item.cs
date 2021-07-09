using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName = "Inventory/Item")] 
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemPic;
    [TextArea]public string description;
}
