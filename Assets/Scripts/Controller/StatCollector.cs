using System;
using Ship;
using UnityEngine;

namespace Controller {
    /// <summary>
    /// Collects stats from everything that could affect the player's score.
    /// </summary>
    public class StatCollector: MonoBehaviour {
        public ShipDriver ship;

        public TimeController timeController;


        private float finalSpeed;

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