using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager uiManager;
 
    [SerializeField] GameObject Player;
    [SerializeField] GameObject BagUI;
    PlayerInput pi;

    bool isBagOpen;

    private void Awake()
    {
        if (!uiManager)
        {
            uiManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        pi = Player.GetComponent<PlayerInput>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BagUIControl();
    }

    private void BagUIControl()
    {
        if (pi.BagSignal)
        {
            BagUI.SetActive(!BagUI.activeSelf);
            pi.BagSignal = false;
        }

        if (BagUI.activeSelf == true && isBagOpen == false)
        {
            Time.timeScale = 0;
            pi.inputEnable = false;
            InventoryManager.RefreshInventory();
            isBagOpen = true;
        }
        else if (BagUI.activeSelf == false && isBagOpen == true)
        {
            Time.timeScale = 1;
            pi.inputEnable = true;
            isBagOpen = false;
        }
    }
}
