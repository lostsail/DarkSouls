using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private void OnDisable()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
