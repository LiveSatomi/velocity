using System;
using Lean.Pool;
using Ship;
using UnityEngine;

namespace Track {
    public class Obstacle: MonoBehaviour, IPoolable {
        public void OnSpawn() {
        }

        public void OnDespawn() {
        }

        public void OnTriggerEnter(Collider other) {
            var shipDriver = other.GetComponent<ShipDriver>();
            if (shipDriver != null) {
                shipDriver.CollideWithObstacle();
            }
        }
        
    }
}