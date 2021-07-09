using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Vector3 deltaPos;

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
        ActorMovement();
    }

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

    private void ActorMovement()
    {
        float factor = walkFactor * (pi.isRun ? runFactor : 1.0f);

        rb.position += deltaPos;
        deltaPos = Vector3.zero;

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
    }

    //Receive Message

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
        anim.SetBool("ground", true);
    }

    void IsNotOnGround()
    {
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
        float attackLayerWeight = Mathf.Lerp(anim.GetLayerWeight(attackLayerIndex), attackLayerTargetWeight, Time.deltaTime* 5);
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
