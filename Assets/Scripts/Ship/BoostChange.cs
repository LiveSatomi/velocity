using UnityEngine;

namespace Ship {
    /// <summary>
    ///     BoostChange occurs when the ship is moderately close to an obstacle. Relatively, it is moderate speed and takes a
    ///     moderate time to complete.
    /// </summary>
    public class BoostChange : StateMachineBehaviour {
        private const LaneChangeType Type = LaneChangeType.Boost;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            LaneChangeUtil.StartChange(shipDriver, Type);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var shipDriver = animator.gameObject.GetComponent<ShipDriver>();
            shipDriver.AddBoost();
            LaneChangeUtil.EndChange(shipDriver);
        }
    }
}