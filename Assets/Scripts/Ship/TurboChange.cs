﻿using UnityEngine;

namespace Ship {
    /// <summary>
    ///     TurboChange occurs when the ship is extremely close to an obstacle. Relatively, it is fast and takes a short time
    ///     to complete.
    /// </summary>
    public class TurboChange : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            shipDriver.ChangeDirection = shipDriver.InputDirection;
            shipDriver.ChangeStartPosition = shipDriver.transform.position.x;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            var transform = shipDriver.transform;

            var pos = transform.position;
            pos.x = shipDriver.ChangeStartPosition + shipDriver.laneWidth * shipDriver.ChangeDirection;
            transform.position = pos;

            shipDriver.ChangeDirection = 0;
        }
    }
}