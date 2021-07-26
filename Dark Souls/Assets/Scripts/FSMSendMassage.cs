using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于动画机发送massage
/// </summary>
public class FSMSendMassage : StateMachineBehaviour
{
    public string[] massagesOnEnter;
    public string[] massagesOnUpdate;
    public string[] massagesOnExit;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var massage in massagesOnEnter)
        {
            animator.gameObject.SendMessageUpwards(massage); 
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var massage in massagesOnUpdate)
        {
            animator.gameObject.SendMessageUpwards(massage);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var massage in massagesOnExit)
        {
            animator.gameObject.SendMessageUpwards(massage);
        }
    }
}
