using Lean.Pool;
using UnityEngine;

namespace Track {
    public class Obstacle : MonoBehaviour, IPoolable {
        public Collider boost;

        public Collider turbo;

        public void OnSpawn() {
        }

        public void OnDespawn() {
        }

        public bool IsBoostCollider(Collider col) {
            return col == boost;
        }

        public bool IsTurboCollider(Collider col) {
            return col == turbo;
        }
    }
}