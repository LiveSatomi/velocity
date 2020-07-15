using Ship;
using UnityEngine;

namespace Controller {
    /// <summary>
    ///     Collects stats from everything that could affect the player's score.
    /// </summary>
    public class StatCollector : MonoBehaviour {
        private float finalSpeed;

        public ShipDriver ship;

        public TimeController timeController;

        public void OnEnable() {
            ship.OnCourseFinished += CollectFinalStats;
        }

        public void OnDisable() {
            ship.OnCourseFinished -= CollectFinalStats;
        }

        private void CollectFinalStats() {
            finalSpeed = ship.Speed;
        }
    }
}