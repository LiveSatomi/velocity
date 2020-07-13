using System;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Ship;
using UnityEngine;
using Utility;
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
        ///     This layer's data is a decision on whether or not an obstacle should appear based on random probability independent
        ///     of all
        ///     other obstacles.
        ///     Also caches the TrackSection an obstacle is placed on.
        /// </summary>
        private readonly List<ExistenceLayer[]> layer0 = new List<ExistenceLayer[]>();

        /// <summary>
        ///     This layer's data decides to remove obstacles based on if there are already obstacles in neighboring lanes of the
        ///     same section.
        /// </summary>
        private readonly List<NeighborLayer[]> layer1 = new List<NeighborLayer[]>();

        /// <summary>
        ///     The probability for 1 lane of 1 section to have an obstacle. This is the context free probability; previous
        ///     sections or neighboring lanes may cause the obstacle to not appear anyway.
        /// </summary>
        [Range(0, 1)] public float existenceProbability;

        /// <summary>
        ///     This layer's data remove obstacles in lanes that are too close to obstacles in the same lane.
        /// </summary>
        private readonly List<NudgeLayer[]> layer2 = new List<NudgeLayer[]>();

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
            AddLayer2Data(lanes);

            var lastLayerCount = layer2.Count;

            for (var lane = 0; lane < lanes.Count; lane++) {
                var existenceLayer = layer0[lastLayerCount - 1][lane];
                var neighborLayer = layer1[lastLayerCount - 1][lane];
                var nudgeLayer = layer2[lastLayerCount - 1][lane];
                if (existenceLayer.Exists && neighborLayer.Exists && nudgeLayer.Nudge < maximumNudgeDistance) {
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

        /// <summary>
        ///     Removes obstacles on either side of an obstacle. Once an obstacle is removed in this way, it cannot remove its own
        ///     neighbors in the same manner.
        /// </summary>
        /// <param name="lanes"></param>
        private void AddLayer1Data(List<Transform> lanes) {
            var layerPreCount = layer1.Count;

            var neighborCrossSection = Enumerable.Repeat(new NeighborLayer(true), lanes.Count).ToArray();

            var indexes = Enumerable.Range(0, lanes.Count);
            var list = indexes.ToList();
            list.Shuffle(random);
            foreach (var i in list) {
                if (layer0[layerPreCount][i].Exists && neighborCrossSection[i].Exists) {
                    if (i > 0 && layer0[layerPreCount][i - 1].Exists && neighborCrossSection[i - 1].Exists) {
                        neighborCrossSection[i - 1] = new NeighborLayer(false);
                    }

                    if (i + 1 != lanes.Count && layer0[layerPreCount][i + 1].Exists &&
                        neighborCrossSection[i + 1].Exists) {
                        neighborCrossSection[i + 1] = new NeighborLayer(false);
                    }
                }
            }

            layer1.Add(neighborCrossSection);
        }

        private void AddLayer2Data(List<Transform> lanes) {
            var layerPreCount = layer2.Count;

            var nudgeCrossSection = new NudgeLayer[lanes.Count];
            for (var lane = 0; lane < lanes.Count; lane++) {
                if (layer0[layerPreCount][lane].Exists && layer1[layerPreCount][lane].Exists) {
                    var neededNudge = 0f;
                    var clearDistance = ship.CalculateClearDistance();
                    for (var sec = layerPreCount - 1;
                        sec >= 0 && sec > layerPreCount - Math.Ceiling(clearDistance / laneLength);
                        sec--) {
                        if (layer0[sec][lane].Exists && layer1[sec][lane].Exists &&
                            layer2[sec][lane].Nudge < maximumNudgeDistance) {
                            neededNudge = Math.Max(0,
                                clearDistance - (layerPreCount - sec) * laneLength + layer2[sec][lane].Nudge);
                            break;
                        }
                    }

                    nudgeCrossSection[lane] = new NudgeLayer(neededNudge);
                } else {
                    nudgeCrossSection[lane] = new NudgeLayer(0);
                }
            }

            layer2.Add(nudgeCrossSection);
        }
    }
}