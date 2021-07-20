using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField]GameObject receiver;

    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            Debug.Log("trigger");
            IDestroy idestory = receiver.GetComponent<IDestroy>();
            if (idestory != null)
            {
                idestory.DestroyEvent();
                isTriggered = true;
            }
            else
                Debug.LogError("destroy error");
        }
    }
}

public interface IDestroy
{
    void DestroyEvent();
}