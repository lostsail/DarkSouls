using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelinesManager : MonoBehaviour
{
    public GameObject ScanTimeline;

    private void Awake()
    {
        if (GameManager.ThisSceneIsFirstLoad)
        {
            GameManager.TurnToScanMode();
            ScanTimeline.SetActive(true);
        }
    }
}
