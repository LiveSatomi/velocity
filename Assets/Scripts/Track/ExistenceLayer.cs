using UnityEngine;

namespace Track {
    /// <summary>
    ///     This layer exists to determine if an obstacle should be considered for this location. It also holds a reference to
    ///     the track.
    /// </summary>
    public class ExistenceLayer {
        /// <summary>
        ///     Determines whether an object should exist.
        /// </summary>
        public bool Exists { get; }

        /// <summary>
        ///     Holds a reference to the cache the obstacle using this layer data should be placed on.
        /// </summary>
        public Transform Track { get; }

        public ExistenceLayer(bool exists, Transform track) {
            Exists = exists;
            Track = track;
        }
    }
}