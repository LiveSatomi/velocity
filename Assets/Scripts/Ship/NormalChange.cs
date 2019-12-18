using System;
using UnityEngine;

namespace Ship
{
    public class NormalChange : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            shipDriver.ChangeDirection = shipDriver.InputDirection;
            shipDriver.ChangeStartPosition = shipDriver.transform.position.x;
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            shipDriver.ChangeDirection = 0;
            var pos = shipDriver.transform.position;
            pos.x = (float) Math.Round(pos.x);
            shipDriver.transform.position = pos;
        }
    }
    


}
