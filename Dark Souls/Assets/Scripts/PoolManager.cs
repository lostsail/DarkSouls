using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ∂‘œÛ≥ÿ
/// </summary>
public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    private Dictionary<string, Stack<GameObject>> pool = new Dictionary<string, Stack<GameObject>>();
    [SerializeField] GameObject prefab;

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
    //    instance.pool["CollectableItem"] = new Stack<GameObject>();
    //    for (int i = 0; i < 100; i++)
    //    {
    //        GameObject newItem = Instantiate(instance.prefab);
    //        newItem.name = instance.prefab.name;
    //        newItem.transform.SetParent(instance.transform);
    //        instance.pool["CollectableItem"].Push(newItem);
    //    }
    //}

    public static void ReturnPool(GameObject item)
    {
        if (!instance.pool.ContainsKey(item.name))
            instance.pool[item.name] = new Stack<GameObject>();

        item.SetActive(false);
        item.transform.SetParent(instance.transform);
        instance.pool[item.name].Push(item);
        //Debug.Log("return pool");
    }

    public static GameObject GetFromPool(GameObject item)
    {
        GameObject newItem;
        if (!instance.pool.ContainsKey(item.name))
            instance.pool[item.name] = new Stack<GameObject>();

        if (instance.pool[item.name].Count == 0)
        {
            newItem= Instantiate(item);
            newItem.name = item.name;
            return newItem;
        }

        newItem= instance.pool[item.name].Pop();
        newItem.SetActive(true);
        return newItem;
    }
}
