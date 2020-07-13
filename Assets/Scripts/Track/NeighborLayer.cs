namespace Track {
    /// <summary>
    ///     This layer removes some obstacles which have obstacles in different lanes of the same section.
    /// </summary>
    public class NeighborLayer {
        /// <summary>
        ///     Determines whether an object should exist.
        /// </summary>
        public bool Exists { get; }

        public NeighborLayer(bool exists) {
            Exists = exists;
        }
    }
}