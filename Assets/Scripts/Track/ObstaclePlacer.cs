using Lean.Pool;
using UnityEngine;

namespace Track {
    public abstract class ObstaclePlacer : MonoBehaviour {
        public abstract void PlaceObstacles(TrackSection target);

        public void DespawnObstacle(Obstacle obstacle) {
            LeanPool.Despawn(obstacle);
        }
    }
}