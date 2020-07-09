using Lean.Pool;
using UnityEngine;

namespace Track {
    public class Obstacle : MonoBehaviour, IPoolable {
        public delegate void ObstacleSizeSetEvent(Vector3 size);

        public const int ObstacleLayer = 8;

        public void OnSpawn() {
        }

        public void OnDespawn() {
        }

        public static event ObstacleSizeSetEvent OnObstacleWidthSet;

        public void Awake() {
            OnObstacleWidthSet?.Invoke(GetComponent<MeshRenderer>().bounds.size);
        }
    }
}