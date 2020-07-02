#region

using Lean.Pool;
using UnityEngine;

#endregion

namespace Track {
    public abstract class ObstaclePlacer : MonoBehaviour {
        public abstract void PlaceObstacles(TrackSection target, TrackSection[] previous);

        public void DespawnObstacle(Obstacle obstacle) {
            LeanPool.Despawn(obstacle);
        }
    }
}