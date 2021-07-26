using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerInput pi;
    Vector3 currentVelocity;

    float RotateX;
    private void Awake()
    {
        pi = player.GetComponent<PlayerInput>();
        RotateX = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.transform.position+Vector3.up*1.4f;
        transform.rotation = player.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if (pi.perspectiveEnable)
        {
            CameraRotate(); 
        }

        CameraMovement();
    }


    /// <summary>
    /// 控制相机旋转
    /// </summary>
    private void CameraRotate()
    {
        transform.Rotate(0, pi.cameraH, 0, Space.World);
        RotateX -= pi.cameraV;
        if (RotateX > 80.0f)
            RotateX = 80.0f;
        if (RotateX < -80.0f)
            RotateX = -80.0f;
        transform.eulerAngles = new Vector3(RotateX, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    /// <summary>
    /// 控制相机移动
    /// </summary>
    private void CameraMovement()
    {
        transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + Vector3.up * 1.4f,ref currentVelocity,0.1f);
    }
}
