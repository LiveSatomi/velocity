namespace Track {
    /// <summary>
    ///     Layer that determines how far back to push an obstacle from its neutral position. Pushing the end of the obstacle
    ///     past the section that it is placed on will cause it to disappear rather than sitting on the next section.
    /// </summary>
    public class NudgeLayer {
        /// <summary>
        ///     Indicates how far back to push the layer.
        /// </summary>
        public float Nudge { get; }

        public NudgeLayer(float nudge) {
            Nudge = nudge;
        }
    }
}