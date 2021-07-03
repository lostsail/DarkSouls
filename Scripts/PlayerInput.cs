using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //key
    string KeyUp = "w";
    string KeyDown = "s";
    string KeyRight = "d";
    string KeyLeft = "a";
    string KeyRun = "left shift";
    string KeyJump = "space";

    //signal
    float targetDup;
    float targetDright;
    Vector2 currentVelocity;
    public Vector2 targetXZ;
    public float Dmag;
    public Vector2 Dvec; //传入的移动信号
    public float cameraH;
    public float cameraV;

    //parameter
    public float smoothTime = 0.1f;
    [SerializeField] float hCamFactor;
    [SerializeField] float vCamFactor;


    //switch
    public bool inputEnable = true;
    public bool lockPlanarMovement;
    public bool isRun;
    public bool jump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputSignal();
        PerspectiveSignal();
    }

    private void InputSignal()
    {
        if (inputEnable)
        {
            targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
            targetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);
            targetXZ = SquareToCircle(targetDup, targetDright);
            if (Input.GetKey(KeyRun))
            {
                isRun = true;
                targetXZ *= 2;
            }

            jump = Input.GetKeyDown(KeyJump);
        }
        else
        {
            if (!lockPlanarMovement)
                targetXZ = Vector2.zero;
        }

        Dvec = Vector2.SmoothDamp(Dvec, targetXZ, ref currentVelocity, smoothTime);
    }

    private void PerspectiveSignal()
    {
        cameraH = Input.GetAxis("Mouse X")*vCamFactor;
        cameraV = Input.GetAxis("Mouse Y")*hCamFactor;
    }

    private Vector2 SquareToCircle(float targetDup, float targetDright)
    {
        float temp = targetDup;
        targetDup *= Mathf.Sqrt(1 - targetDright * targetDright / 2.0f);
        targetDright *= Mathf.Sqrt(1 - temp * temp / 2.0f);
        return new Vector2(targetDright, targetDup);
    }
}
