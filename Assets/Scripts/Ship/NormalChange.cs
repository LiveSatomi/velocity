using UnityEngine;

namespace Ship {
    /// <summary>
    ///     NormalChange occurs when the ship is not close to any obstacles. Relatively, it starts slowly and takes a long time
    ///     to complete.
    /// </summary>
    public class NormalChange : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            LaneChangeUtil.StartChange(shipDriver);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            LaneChangeUtil.EndChange(shipDriver);
        }
    }
}