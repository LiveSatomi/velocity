using Lean.Pool;
using UnityEngine;

namespace Track {
    public class TrackBuilder : MonoBehaviour {
        public const string Tag = "Track";

        private TrackSection lastSection;

        /// <summary>
        ///     Number of sections generated ahead of the player.
        /// </summary>
        public int lookAhead = 10;

        public ObstaclePlacer obstaclePlacer;

        public SectionBuilder sectionBuilder;

        private TrackSection trackSection;

        // Start is called before the first frame update
        private void Start() {
            trackSection = sectionBuilder.BuildSection();
            GetComponent<LeanGameObjectPool>().Prefab = trackSection.gameObject;
            lastSection = AddSection(transform);
            for (var i = 1; i < lookAhead; i++) {
                lastSection = AddSection(lastSection.EndPoint);
            }
        }

        public void AddSection() {
            lastSection = AddSection(lastSection.EndPoint);
        }

        private TrackSection AddSection(Transform endpoint) {
            var position = new Vector3(0, 0, endpoint.position.z + sectionBuilder.laneLength / 2f);
            var newSection = LeanPool.Spawn(trackSection, position, Quaternion.identity, transform);
            return newSection;
        }
    }
}