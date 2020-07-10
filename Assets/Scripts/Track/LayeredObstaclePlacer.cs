using System;
using System.Collections.Generic;
using Lean.Pool;
using Ship;
using UnityEngine;
using Random = System.Random;

namespace Track {
    /// <summary>
    ///     This obstacle placer generates obstacles in layers. Layer 0 info is context free and loaded when PlaceObstacles is
    ///     called. The final layer will be loaded sometime later and will actually Spawn the obstacle.
    /// </summary>
    public class LayeredObstaclePlacer : ObstaclePlacer {
        /// <summary>
        ///     The random number generator's seed.
        /// </summary>
        private const int Seed = 12;

        /// <summary>
        ///     A poolable prefab that shall be placed on the track.
        /// </summary>
        public GameObject obstacle;

        /// <summary>
        ///     Random number generator
        /// </summary>
        private readonly Random random = new Random(Seed);

        /// <summary>
        ///     Layer 0 data. A decision on whether or not an obstacle should appear based on random probability independent of all
        ///     other obstacles.
        ///     Also caches the TrackSection an obstacle is placed on.
        /// </summary>
        private readonly List<ExistenceLayer[]> layer0 = new List<ExistenceLayer[]>();

        /// <summary>
        ///     The probability for 1 lane of 1 section to have an obstacle. This is the context free probability; previous
        ///     sections or neighboring lanes may cause the obstacle to not appear anyway.
        /// </summary>
        [Range(0, 1)] public float existenceProbability;

        /// <summary>
        ///     Layer 1 data. This layer's data remove obstacles in lanes that are too close to obstacles in the same lane.
        /// </summary>
        private readonly List<NudgeLayer[]> layer1 = new List<NudgeLayer[]>();

        /// <summary>
        ///     Length of the lane section.
        /// </summary>
        private float laneLength;

        /// <summary>
        ///     Length of the obstacles.
        /// </summary>
        private float obstacleLength;

        /// <summary>
        ///     The threshold for layer 1 that determines if an obstacle should be completely discarded before it is spawned.
        /// </summary>
        private float maximumNudgeDistance;


        /// <summary>
        ///     Cached player object
        /// </summary>
        private ShipDriver ship;

        private void Awake() {
            ship = GameObject.FindWithTag(ShipDriver.Tag).GetComponent<ShipDriver>();
            laneLength = GameObject.FindWithTag(TrackBuilder.Tag).GetComponent<SectionBuilder>().laneLength;
            obstacleLength = obstacle.GetComponent<MeshRenderer>().bounds.size.z;
            maximumNudgeDistance = laneLength - obstacleLength;
        }


        /// <summary>
        ///     Loads layer 0 of obstacle data for the target section. The passed layer will be cached so that the next layer can
        ///     be loaded on subsequent calls after loading layer 0 for the subsequent parameter.
        /// </summary>
        /// <param name="target"></param>
        public override void PlaceObstacles(TrackSection target) {
            var lanes = target.GetLanes();


            AddLayer0Data(lanes);
            AddLayer1Data(lanes);

            var lastLayerCount = layer1.Count;

            for (var lane = 0; lane < lanes.Count; lane++) {
                var existenceLayer = layer0[lastLayerCount - 1][lane];
                var nudgeLayer = layer1[lastLayerCount - 1][lane];
                if (existenceLayer.Exists && nudgeLayer.Nudge < maximumNudgeDistance) {
                    var position = new Vector3(0, .5f, -(laneLength / 2 - obstacleLength / 2) + nudgeLayer.Nudge);
                    LeanPool.Spawn(obstacle, position, Quaternion.identity, existenceLayer.Track, false);
                }
            }
        }

        private void AddLayer0Data(List<Transform> lanes) {
            var existenceCrossSection = new ExistenceLayer[lanes.Count];
            for (var lane = 0; lane < lanes.Count; lane++) {
                existenceCrossSection[lane] =
                    new ExistenceLayer(random.NextDouble() < existenceProbability, lanes[lane]);
            }

            layer0.Add(existenceCrossSection);
        }

        private void AddLayer1Data(List<Transform> lanes) {
            var layerPreCount = layer1.Count;

            var nudgeCrossSection = new NudgeLayer[lanes.Count];
            for (var lane = 0; lane < lanes.Count; lane++) {
                if (layer0[layerPreCount][lane].Exists) {
                    var neededNudge = 0f;
                    var clearDistance = ship.CalculateClearDistance();
                    for (var sec = layerPreCount - 1;
                        sec >= 0 && sec > layerPreCount - Math.Ceiling(clearDistance / laneLength);
                        sec--) {
                        if (layer0[sec][lane].Exists && layer1[sec][lane].Nudge < maximumNudgeDistance) {
                            neededNudge = Math.Max(0,
                                clearDistance - (layerPreCount - sec) * laneLength + layer1[sec][lane].Nudge);
                            break;
                        }
                    }

                    nudgeCrossSection[lane] = new NudgeLayer(neededNudge);
                } else {
                    nudgeCrossSection[lane] = new NudgeLayer(0);
                }
            }

            layer1.Add(nudgeCrossSection);
        }
    }
}