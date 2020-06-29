﻿using UnityEngine;

namespace Ship {
    /// <summary>
    ///     NormalChange occurs when the ship is not close to any obstacles. Relatively, it starts slowly and takes a long time
    ///     to complete.
    /// </summary>
    public class NormalChange : StateMachineBehaviour {
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