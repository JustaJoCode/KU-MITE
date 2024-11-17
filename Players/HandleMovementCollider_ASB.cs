using UnityEngine;
using System.Collections;

public class HandleMovementCollider_ASB : StateMachineBehaviour {

    StateManager states;

    public int index;

    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (states == null)
            states = animator.transform.GetComponentInParent<StateManager>();

        states.CloseMovementCollider(index);

    }

  
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (states == null)
            states = animator.transform.GetComponentInParent<StateManager>();

        states.OpenMovementCollider(index);
    }

}
