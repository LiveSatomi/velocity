using Lean.Pool;
using UnityEngine;

namespace Track {
    public class TrackBuilder : MonoBehaviour {
        /// <summary>
        ///     A tag of for the Track GameObject.
        /// </summary>
        public const string Tag = "Track";

        /// <summary>
        ///     Number of sections generated ahead of the player.
        /// </summary>
        public int lookAhead;

        /// <summary>
        ///     Number of sections before obstacles start appearing
        /// </summary>
        public int emptyInitialSections;

        /// <summary>
        ///     Service that places obstacles on the track
        /// </summary>
        public ObstaclePlacer obstaclePlacer;

        /// <summary>
        ///     Service that creates TrackSections
        /// </summary>
        public SectionBuilder sectionBuilder;

        /// <summary>
        ///     A prefab that represents section of track for the player. It contains a small length of each lane on the track.
        /// </summary>
        private TrackSection trackSection;

        /// <summary>
        ///     A reference to the most recently added section of track.
        /// </summary>
        private TrackSection lastSection;


        private void Start() {
            trackSection = sectionBuilder.BuildSection();
            GetComponent<LeanGameObjectPool>().Prefab = trackSection.gameObject;
            lastSection = AddSection(transform);
            for (var i = 1; i < lookAhead; i++) {
                lastSection = AddSection(lastSection.EndPoint);
            }
        }

        /// <summary>
        ///     Connects a new section of track to the end of the last added section.
        /// </summary>
        public void AddSection() {
            lastSection = AddSection(lastSection.EndPoint);
        }

        private TrackSection AddSection(Transform endpoint) {
            var position = new Vector3(0, 0, endpoint.position.z + sectionBuilder.laneLength / 2f);
            var newSection = LeanPool.Spawn(trackSection, position, Quaternion.identity, transform);
            return newSection;
        }

        /// <summary>
        ///     Places obstacles on the specified track section based on the registered obstaclePlacer service.
        /// </summary>
        /// <param name="section">The TrackSection to place obstacles on</param>
        public void PlaceObstacles(TrackSection section) {
            if (emptyInitialSections > 0) {
                emptyInitialSections--;
            } else {
                obstaclePlacer.PlaceObstacles(section);
            }
        }
    }
}