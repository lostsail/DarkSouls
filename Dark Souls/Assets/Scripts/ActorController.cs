using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色控制器
/// </summary>
public class ActorController : MonoBehaviour
{
    public enum State
    {
        ground,
        jump,
        fall,
        roll,
        jab,
        attack
    }

    [SerializeField] GameObject model;
    [SerializeField] GameObject cam;
    PlayerInput pi;
    Animator anim;
    Rigidbody rb;

    public State state = new State();

    [SerializeField] float walkFactor;
    [SerializeField] float runFactor;
    [SerializeField] float turnFactor;
    [SerializeField] Vector3 jumpThrust;
    [SerializeField] float rollMultipliar;
    [SerializeField] float jabMultipliar;

    float attackLayerTargetWeight;
    int attackLayerIndex;
    bool isOnGround;
    Vector3 deltaPos;
    Vector3 slopeDirection;
    Vector3 targetForward;

    private void Awake()
    {
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        state = State.ground;
        attackLayerIndex = anim.GetLayerIndex("attack");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetParameter();

    }

    private void FixedUpdate()
    {
        SlopeDetect();
        ActorMovement();

    }

    /// <summary>
    /// 斜面检测，优化角色在斜面上的移动
    /// </summary>
    private void SlopeDetect()
    {
        Ray beginSlopeDetectRay = new Ray(transform.position + Vector3.up * 0.5f, Vector3.down);
        Ray endSlopeDetectRay = new Ray(transform.position + Vector3.up * 0.5f + transform.forward * 0.1f, Vector3.down);
        RaycastHit beginHitInfo;
        RaycastHit endHitInfo;

        if (Physics.Raycast(beginSlopeDetectRay, out beginHitInfo, 1.1f, LayerMask.GetMask("Ground")) && Physics.Raycast(endSlopeDetectRay, out endHitInfo, 1.9f, LayerMask.GetMask("Ground")) && isOnGround)
            slopeDirection = (endHitInfo.point - beginHitInfo.point).normalized;
        else
            slopeDirection = Vector3.zero;
    }

    /// <summary>
    /// 向动画机传递参数
    /// </summary>
    private void SetParameter()
    {
        anim.SetFloat("forward", pi.Dmag);

        if (pi.jump)
        {
            anim.SetTrigger("jump");
        }

        if (pi.attack)
        {
            anim.SetTrigger("attack");
        }
    }

    /// <summary>
    /// 控制角色移动
    /// </summary>
    private void ActorMovement()
    {
        float factor = walkFactor * (pi.isRun ? runFactor : 1.0f);

        //rb.position += deltaPos;
        //deltaPos = Vector3.zero;

        if (!pi.lockPlanarMovement)
        {
            Vector3 forward = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized;
            rb.velocity = right * pi.Dvec.x * factor + forward * pi.Dvec.z * factor + transform.up * rb.velocity.y;

            if (pi.Dmag > 0.1f)
                transform.forward = Vector3.Slerp(transform.forward, Vector3.ProjectOnPlane(rb.velocity, Vector3.up), turnFactor);
        }

        if (state == State.roll)
        {
            rb.velocity = transform.forward * anim.GetFloat("rollVelocity") * rollMultipliar;
        }

        if (state == State.jab)
        {
            rb.velocity = -transform.forward * anim.GetFloat("jabVelocity") * jabMultipliar;
        }


        if (slopeDirection != Vector3.zero&& (state ==State.ground||state==State.roll))
        {
            rb.velocity = slopeDirection*rb.velocity.magnitude;
        }
    }

    //******************************************* 接受massage ************************************************************

    void OnJumpEnter()
    {
        state = State.jump;
        pi.inputEnable = false;
        pi.lockPlanarMovement = true;
        rb.AddRelativeForce(jumpThrust, ForceMode.Impulse);
    }

    void OnGroundEnter()
    {
        state = State.ground;
        pi.inputEnable = true;
        pi.lockPlanarMovement = false;
    }

    void OnGroundExit()
    {
        pi.lockPlanarMovement = true;
    }

    void IsOnGround()
    {
        isOnGround = true;
        anim.SetBool("ground", true);
    }

    void IsNotOnGround()
    {
        isOnGround = false;
        anim.SetBool("ground", false);
    }

    void OnFallEnter()
    {
        state = State.fall;
        pi.inputEnable = false;
        pi.lockPlanarMovement = true;
    }

    void OnFallUpdate()
    {
        if (rb.velocity.magnitude > 5.0f)
        {
            anim.SetTrigger("roll");
        }
    }

    void OnRollEnter()
    {
        state = State.roll;
        rb.velocity = Vector3.zero;
        pi.lockPlanarMovement = true;
        pi.inputEnable = false;
    }

    void OnJabEnter()
    {
        state = State.jab;
        rb.velocity = Vector3.zero;
        pi.lockPlanarMovement = true;
        pi.inputEnable = false;
    }

    void OnAttackEnter()
    {
        state = State.attack;
        attackLayerTargetWeight = 1.0f;
        pi.inputEnable = false;
    }

    void OnAttackUpdate()
    {
        float attackLayerWeight = Mathf.Lerp(anim.GetLayerWeight(attackLayerIndex), attackLayerTargetWeight, Time.deltaTime * 5);
        anim.SetLayerWeight(attackLayerIndex, attackLayerWeight);
    }

    void OnAttackIdleEnter()
    {
        state = State.ground;
        attackLayerTargetWeight = 0;
        pi.lockPlanarMovement = false;
        pi.inputEnable = true;
    }

    void OnAttackIdleUpdate()
    {
        float attackLayerWeight = Mathf.Lerp(anim.GetLayerWeight(attackLayerIndex), attackLayerTargetWeight, Time.deltaTime * 5);
        anim.SetLayerWeight(attackLayerIndex, attackLayerWeight);
    }

    void OnAttackRootMotion(Vector3 _deltaPosition)
    {
        deltaPos += _deltaPosition;
    }
}
