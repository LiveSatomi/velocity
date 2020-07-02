#region

using UnityEngine;

#endregion

namespace Ship {
    /// <summary>
    ///     TurboChange occurs when the ship is extremely close to an obstacle. Relatively, it is fast and takes a short time
    ///     to complete.
    /// </summary>
    public class TurboChange : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            LaneChangeUtil.StartChange(shipDriver);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            shipDriver.AddTurbo();
            LaneChangeUtil.EndChange(shipDriver);
        }
    }
}