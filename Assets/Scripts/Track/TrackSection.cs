using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Ship;
using UnityEngine;

namespace Track {
    public class TrackSection : MonoBehaviour, IPoolable {
        public Material gutterMaterial;

        public Material laneMaterial;

        /// <summary>
        ///     The number of lanes this section has; this should match the mesh attached to the GameObject
        /// </summary>
        public int Lanes { get; set; }
        
       
        public Transform Threshold { get; private set; }

        public Transform StartPoint { get; private set; }

        public Transform EndPoint { get; private set; }
        
        private ShipDriver ship;

        private bool thresholdPassed;

        private TrackBuilder track;


        public void OnSpawn() {
            track.obstaclePlacer.PlaceObstacles(this, new TrackSection[] {});
        }

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

        private void Update() {
            if (!thresholdPassed && ship.transform.position.z > Threshold.transform.position.z) {
                track.AddSection();
                thresholdPassed = true;
            }
        }

        public List<Transform> GetLanes() {
            var lanes = transform.Find("Lanes").Cast<Transform>().ToList();
            return lanes;
        }
    }
}