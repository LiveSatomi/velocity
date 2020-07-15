using UnityEngine;

namespace Ship {
    /// <summary>
    ///     An adapter between fields in the ShipDriver related to user input and the modules that want to control them.
    ///     Subclasses of this component should have ShipDriver components.
    /// </summary>
    public abstract class ShipInputController : MonoBehaviour {
        /// <summary>
        ///     A delegate method to handle Direction related input events.
        /// </summary>
        /// <param name="direction">A float representing the direction. Negative for left, positive for right.</param>
        public delegate void DirectionChangedEvent(float direction);

        /// <summary>
        ///     Cached Player object
        /// </summary>
        protected ShipDriver ship;

        /// <summary>
        ///     A event that is invoked when the input direction changes.
        /// </summary>
        public abstract event DirectionChangedEvent OnDirectionChanged;

        protected virtual void Awake() {
            ship = GetComponent<ShipDriver>();
        }
    }
}