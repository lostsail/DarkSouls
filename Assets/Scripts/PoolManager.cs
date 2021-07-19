using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    private Dictionary<string, Stack<GameObject>> pool = new Dictionary<string, Stack<GameObject>>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //FillPool();
    }

    //private static void FillPool()
    //{

    //}

    public static void ReturnPool(GameObject item)
    {
        if (!instance.pool.ContainsKey(item.name))
            instance.pool[item.name] = new Stack<GameObject>();

        item.SetActive(false);
        item.transform.SetParent(instance.transform);
        instance.pool[item.name].Push(item);
    }

    public static GameObject GetFromPool(GameObject item)
    {
        if (!instance.pool.ContainsKey(item.name))
            instance.pool[item.name] = new Stack<GameObject>();

        if (instance.pool[item.name].Count == 0)
        {
            return Instantiate(item);
        }

        GameObject newItem= instance.pool[item.name].Pop();
        newItem.SetActive(true);
        return newItem;
    }
}
