using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionHandle : MonoBehaviour
{
    ActorController ac;
    Animator anim;

    void Awake()
    {
        ac = transform.parent.GetComponent<ActorController>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    //void OnAnimatorMove()
    //{
    //    if (ac.state == ActorController.State.attack)
    //    {
    //        SendMessageUpwards("OnAttackRootMotion", anim.deltaPosition);
    //    }
    //}
}
