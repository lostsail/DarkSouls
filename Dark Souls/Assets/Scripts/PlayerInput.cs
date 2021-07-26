using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 将玩家输入转换成信号
/// </summary>
public class PlayerInput : MonoBehaviour
{
    //key
    string KeyUp = "w";
    string KeyDown = "s";
    string KeyRight = "d";
    string KeyLeft = "a";
    string KeyRun = "left shift";
    string KeyJump = "space";
    string KeyBag = "b";

    //signal
    float targetDup;
    float targetDright;
    Vector3 currentVelocity;
    public Vector3 targetVelocity;
    public float Dmag;
    public Vector3 Dvec; //传入的移动信号
    public float cameraH;
    public float cameraV;

    //parameter
    public float smoothTime = 0.1f;
    [SerializeField] float hCamFactor;
    [SerializeField] float vCamFactor;


    //switch
    public bool inputEnable = true;
    public bool perspectiveEnable = true;
    public bool lockPlanarMovement;
    public bool isRun;
    public bool jump;
    public bool attack;
    public bool BagSignal;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void FixedUpdate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        InputSignal();
        PerspectiveSignal();
        UISignal();
    }

    /// <summary>
    /// 接受按键输入并转换为信号
    /// </summary>
    private void InputSignal()
    {
        if (inputEnable)
        {
            targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
            targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);
            targetVelocity = SquareToCircle(targetDup, targetDright);
            if (Input.GetKey(KeyRun))
            {
                isRun = true;
                targetVelocity *= 2;
            }
            jump = Input.GetKeyDown(KeyJump);
            attack = Input.GetMouseButtonDown(0);
        }
        else
        {
            if (!lockPlanarMovement)
                targetVelocity = Vector3.zero;
        }

        Dvec = Vector3.SmoothDamp(Dvec, targetVelocity, ref currentVelocity, smoothTime);
        Dmag = Mathf.Sqrt(Dvec.x * Dvec.x + Dvec.z * Dvec.z);
    }

    /// <summary>
    /// 接受鼠标输入并转换为视角信号
    /// </summary>
    private void PerspectiveSignal()
    {
            cameraH = Input.GetAxis("Mouse X") * vCamFactor;
            cameraV = Input.GetAxis("Mouse Y") * hCamFactor;
    }

    /// <summary>
    /// 背包UI的信号输入
    /// </summary>
    private void UISignal()
    {
        if (Input.GetKeyDown(KeyBag))
        {
            BagSignal = true;
        }
    }

    /// <summary>
    /// 将玩家在前后和左右方向上的输入转换成速度向量
    /// </summary>
    /// <param name="targetDup">前后方向上的输入</param>
    /// <param name="targetDright">左右方向上的输入</param>
    /// <returns>玩家实际速度向量</returns>
    private Vector3 SquareToCircle(float targetDup, float targetDright)
    {
        float temp = targetDup;
        targetDup *= Mathf.Sqrt(1 - targetDright * targetDright / 2.0f);
        targetDright *= Mathf.Sqrt(1 - temp * temp / 2.0f);
        return new Vector3(targetDright, 0, targetDup);
    }
}
