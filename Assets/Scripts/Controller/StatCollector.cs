#region

using Ship;
using UnityEngine;

#endregion

namespace Controller {
    /// <summary>
    ///     Collects stats from everything that could affect the player's score.
    /// </summary>
    public class StatCollector : MonoBehaviour {
        private float finalSpeed;

        public ShipDriver ship;

        public TimeController timeController;

        public void OnEnable() {
            ShipDriver.OnCourseFinished += CollectFinalStats;
        }

        public void OnDisable() {
            ShipDriver.OnCourseFinished -= CollectFinalStats;
        }

        private void CollectFinalStats() {
            finalSpeed = ship.Speed;
        }
    }
}