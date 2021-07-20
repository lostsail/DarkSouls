using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyed : MonoBehaviour,IDestroy
{
    public void DestroyEvent()
    {
        Destroy(gameObject);
    }
}
