using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    PlayerInput pi;

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
        CameraRotate();
        CameraMovement();
    }

    private void LateUpdate()
    {

    }

    private void CameraRotate()
    {
        transform.Rotate(0, pi.cameraH * Time.deltaTime, 0, Space.World);
        RotateX -= pi.cameraV * Time.deltaTime;
        if (RotateX > 80.0f)
            RotateX = 80.0f;
        if (RotateX < -80.0f)
            RotateX = -80.0f;
        transform.eulerAngles = new Vector3(RotateX, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void CameraMovement()
    {
        transform.position = player.transform.position + Vector3.up * 1.4f;
    }
}
