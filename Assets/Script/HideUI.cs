using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        ChangeUIState();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            GameManager.isHide = !GameManager.isHide;
            ChangeUIState();
        }
    }

    private void ChangeUIState()
    {
        if (GameManager.isHide)
        {
            rect.localScale = Vector3.zero;
        }
        else
        {
            rect.localScale = Vector3.one;
        }
    }
}
