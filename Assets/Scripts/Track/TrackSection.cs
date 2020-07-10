using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Ship;
using UnityEngine;

namespace Track {
    public class TrackSection : MonoBehaviour, IPoolable {
        /// <summary>
        ///     The player controlled ship
        /// </summary>
        private ShipDriver ship;

        /// <summary>
        ///     State representing whether the ship has reached the Threshold position or not.
        /// </summary>
        private bool thresholdPassed;

        /// <summary>
        ///     A reference to the parent track.
        /// </summary>
        private TrackBuilder track;

        /// <summary>
        ///     The number of lanes this section has; this should match the mesh attached to the GameObject
        /// </summary>
        public int Lanes { get; set; }

        /// <summary>
        ///     A transform with a position before the StartPoint at which a new TrackSection should be spawned for the player to
        ///     see.
        /// </summary>
        public Transform Threshold { get; private set; }

        /// <summary>
        ///     A transform with a position representing the bounds of mesh where the player will enter it.
        /// </summary>
        public Transform StartPoint { get; private set; }

        /// <summary>
        ///     A transform with a position representing the bounds of mesh where the player will leave it.
        /// </summary>
        public Transform EndPoint { get; private set; }


        /// <summary>
        ///     Spawns obstacles on the section.
        /// </summary>
        public void OnSpawn() {
            track.PlaceObstacles(this);
        }

        /// <summary>
        ///     Resets object and despawns all owned obstacles.
        /// </summary>
        public void OnDespawn() {
            thresholdPassed = false;
            foreach (var lane in GetLanes()) {
                var obstacles = lane.GetComponentsInChildren<Obstacle>();
                foreach (var obstacle in obstacles) {
                    track.obstaclePlacer.DespawnObstacle(obstacle);
                }
            }
        }

        private void Awake() {
            StartPoint = transform.Find("StartPoint");
            EndPoint = transform.Find("EndPoint");
            Threshold = transform.Find("Threshold");
            ship = GameObject.FindWithTag("Player").GetComponent<ShipDriver>();
            track = GameObject.FindWithTag("Track").GetComponent<TrackBuilder>();
        }

        /// <summary>
        ///     Keeps track of whether this object has been passed by the ship.
        /// </summary>
        private void Update() {
            if (!thresholdPassed && ship.transform.position.z > Threshold.transform.position.z) {
                track.AddSection();
                thresholdPassed = true;
            }
        }

        /// <summary>
        ///     Gets a list of the objects that make of the sections of lane owned by this object.
        /// </summary>
        /// <returns>A list of transforms for each mesh that makes up a lane.</returns>
        public List<Transform> GetLanes() {
            var lanes = transform.Find("Lanes").Cast<Transform>().ToList();
            return lanes;
        }
    }
}