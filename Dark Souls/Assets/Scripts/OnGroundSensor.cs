using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 检测玩家是否在地面上
/// </summary>
public class OnGroundSensor : MonoBehaviour
{
    [SerializeField] CapsuleCollider capsuleCollider;

    Vector3 point1;
    Vector3 point2;
    float radius;

    private void Awake()
    {
        radius = capsuleCollider.radius;
    }

    private void FixedUpdate()
    {
        OnGroundDetect();
    }

    private void OnGroundDetect()
    {
        point1 = transform.position + Vector3.up * (radius-0.2f);
        point2 = transform.position + Vector3.up * (capsuleCollider.height - radius);

        Collider[] colliders = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));

        if (colliders.Length != 0)
            SendMessage("IsOnGround");
        else
            SendMessage("IsNotOnGround");
    }
}
