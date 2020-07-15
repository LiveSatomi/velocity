using UnityEngine;
using Random = System.Random;

namespace Ship {
    /// <summary>
    ///     A ShipInputController controlled by a random number generator and integers representing probability of actions. An
    ///     action is decided in advance and taken as soon as it becomes available.
    /// </summary>
    public class AutoShipInputController : ShipInputController {
        /// <summary>
        ///     The random number generator's seed.
        /// </summary>
        private const int Seed = 12;

        /// <summary>
        ///     The direction to take on the next lane change.
        /// </summary>
        private float direction = 1f;

        /// <summary>
        ///     Relative probability of not using a lane change (losing the game).
        /// </summary>
        [Range(0, 100)] [SerializeField] private int waitChance = 0;

        /// <summary>
        ///     Relative probability of using a boost change.
        /// </summary>
        [Range(0, 100)] [SerializeField] private int boostChance = 0;

        /// <summary>
        ///     Relative probability of using a turbo change.
        /// </summary>
        [Range(0, 100)] [SerializeField] private int turboChance = 1;

        /// <summary>
        ///     Random number generator
        /// </summary>
        private readonly Random random = new Random(Seed);

        /// <summary>
        ///     The behavior for lane changes decided in advance.
        /// </summary>
        private int decision;

        public override event DirectionChangedEvent OnDirectionChanged;

        protected override void Awake() {
            base.Awake();
            ship.OnLaneChanged += type => {
                direction *= -1;
                decision = 0;
            };
        }

        private void Update() {
            if (decision == 0) {
                var chance = waitChance + boostChance + turboChance;
                var next = random.Next(chance);
                if (next < waitChance) {
                    decision = 1;
                } else if (next < boostChance) {
                    decision = 2;
                } else {
                    decision = 3;
                }
            }

            switch (decision) {
                case 2 when ship.CanBoost():
                case 3 when ship.CanTurbo():
                    OnDirectionChanged?.Invoke(direction);
                    break;
                default:
                    OnDirectionChanged?.Invoke(0);
                    break;
            }
        }
    }
}