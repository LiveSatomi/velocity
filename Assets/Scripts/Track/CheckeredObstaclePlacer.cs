using Lean.Pool;
using UnityEngine;

namespace Track {
    public class CheckeredObstaclePlacer : ObstaclePlacer {
        public static bool adjusted;

        public GameObject obstacle;

        public override void PlaceObstacles(TrackSection target) {
            var lanes = target.GetLanes();
            var position = new Vector3(0, .5f, 0);
            for (var i = 0; i < lanes.Count; i++) {
                var lane = lanes[i];
                if (i % 2 == (adjusted ? 1 : 0)) {
                    LeanPool.Spawn(obstacle, position, Quaternion.identity, lane, false);
                }
            }

            adjusted = !adjusted;
        }
    }
}