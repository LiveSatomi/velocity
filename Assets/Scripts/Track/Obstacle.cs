using Lean.Pool;
using UnityEngine;

namespace Track {
    public class Obstacle : MonoBehaviour, IPoolable {
        public delegate void ObstacleWidthSetEvent(float width);

        public const int ObstacleLayer = 8;

        public void OnSpawn() {
        }

        public void OnDespawn() {
        }

        public static event ObstacleWidthSetEvent OnObstacleWidthSet;

        public void Awake() {
            OnObstacleWidthSet?.Invoke(GetComponent<MeshRenderer>().bounds.size.x);
        }
    }
}