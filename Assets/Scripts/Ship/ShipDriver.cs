using System;
using Track;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ship {
    public class ShipDriver : MonoBehaviour {
        public delegate void CourseFinishedEvent();

        /// <summary>
        ///     Number of frames the lane change animation takes.
        /// </summary>
        private const float ChangeFrames = 25f;

        /// <summary>
        ///     Number of frames the boost change animation takes.
        /// </summary>
        private const float BoostFrames = 17f;

        /// <summary>
        ///     Number of frames the turbo change animation takes.
        /// </summary>
        private const float TurboFrames = 9f;

        /// <summary>
        ///     A constant distance to subtract from lateral movement capabilities to combat rounding errors.
        /// </summary>
        private const float Fudge = .3f;

        /// <summary>
        ///     Percentage of progress a change is required to use to clear the obstacle.
        /// </summary>
        private const float PartialRequirement = .2f;

        /// <summary>
        ///     Tag for the player's object.
        /// </summary>
        public const string Tag = "Player";

        /// <summary>
        ///     Cached animator parameter. Indicates the direction of the current lane change.
        /// </summary>
        private static readonly int AnimatorChangeDirection = Animator.StringToHash("changeDirection");

        /// <summary>
        ///     Cached animator parameter. Indicates if the ship is in a boost zone.
        /// </summary>
        private static readonly int AnimatorBoost = Animator.StringToHash("boost");

        /// <summary>
        ///     Cached animator parameter. Indicates if the ship is in a turbo zone.
        /// </summary>
        private static readonly int AnimatorTurbo = Animator.StringToHash("turbo");

        /// <summary>
        ///     Cached MeshRenderer component
        /// </summary>
        private MeshRenderer meshRenderer;

        /// <summary>
        ///     Current forward velocity.
        /// </summary>
        public float Speed { get; private set; }

        [SerializeField] private float initialSpeed = 3;

        /// <summary>
        ///     Minimum speed at any given time. It increases monotonically from initialSpeed over time.
        /// </summary>
        public float BaseSpeed { get; private set; }

        /// <summary>
        ///     The forward speed increase awarded at the end of a boost change
        /// </summary>
        [SerializeField] private float boost = 1f;

        /// <summary>
        ///     The forward speed increase awarded at the end of a turbo change.
        /// </summary>
        [SerializeField] private float turbo = 2f;

        /// <summary>
        ///     Speed boost based on performance. Decays to 0 over time.
        /// </summary>
        private float speedBoost;

        /// <summary>
        ///     Percentage of Speed that becomes the new baseSpeed.
        /// </summary>
        [Range(0, 1)] [SerializeField] private float chargePercentage = 0.3f;

        /// <summary>
        ///     Decay rate of speedBoost.
        /// </summary>
        [Range(0, 1)] [SerializeField] private float decayRate = 0.3f;

        /// <summary>
        ///     State that represents if the ship has collided with an obstacle.
        /// </summary>
        private bool collided;

        /// <summary>
        ///     Transform of the bow of the ship, used to calculate time to collision with obstacles.
        /// </summary>
        private Transform front;

        /// <summary>
        ///     Cached animator component.
        /// </summary>
        private Animator animator;

        /// <summary>
        ///     Indicates the percentage of lane change animation completed. Controlled by animator.
        /// </summary>
        [HideInInspector] public float changeProgress;

        /// <summary>
        ///     State that represents the direction of the lane change being executed.
        /// </summary>
        public float ChangeDirection { get; set; }

        /// <summary>
        ///     State that tells where the animation started. Used to adjust for rounding errors at the end of each lane change.
        /// </summary>
        public float ChangeStartPosition { get; set; }

        /// <summary>
        ///     Unity InputAction that controls the ship.
        /// </summary>
        private ShipInputAction inputAction;

        /// <summary>
        ///     State tied to the an input representing the desired direction to change lanes.
        /// </summary>
        public float InputDirection { get; private set; }

        /// <summary>
        ///     Width of ship model's hitbox.
        /// </summary>
        private float shipWidth;

        /// <summary>
        ///     Distance that must be traversed to lock into the next lane over (includes gutter width).
        /// </summary>
        public float LaneWidth { get; private set; }

        /// <summary>
        ///     The width of the obstacles that must be avoided.
        /// </summary>
        private float obstacleWidth;

        public static event CourseFinishedEvent OnCourseFinished;

        private void Awake() {
            shipWidth = GetComponent<MeshRenderer>().bounds.size.x;
            var sectionBuilder = GameObject.FindWithTag(TrackBuilder.Tag).GetComponent<SectionBuilder>();
            LaneWidth = sectionBuilder.laneWidth + sectionBuilder.gutterWidth;
            BaseSpeed = initialSpeed;
            inputAction = new ShipInputAction();
            inputAction.ShipControls.ChangeLane.performed += ctx => {
                InputDirection = Math.Sign(ctx.ReadValue<float>());
            };
            animator = GetComponent<Animator>();
            foreach (Transform trans in transform) {
                if (trans.name == "Front") {
                    front = trans;
                }
            }
        }

        // Update is called once per frame
        private void Update() {
            if (!collided) {
                animator.SetFloat(AnimatorChangeDirection, InputDirection);
                Speed = BaseSpeed + speedBoost;
                speedBoost = Math.Max(0, speedBoost - decayRate * Time.deltaTime);
                BaseSpeed = Math.Max(BaseSpeed, Speed * chargePercentage);
            }
        }


        private void FixedUpdate() {
            if (Physics.Raycast(front.position, Vector3.forward, out var hitInfo, float.PositiveInfinity,
                1 << Obstacle.ObstacleLayer)) {
                var collideDistance = hitInfo.distance;
                var collideTime = collideDistance / Speed;
                var distanceToClear = (shipWidth + obstacleWidth) / 2;

                var changeCapability = 1 / ChangeFrames * (collideTime / Time.deltaTime) * LaneWidth;
                var boostCapability = 1 / BoostFrames * (collideTime / Time.deltaTime) * LaneWidth;
                var partialBoostDistance = 1 / BoostFrames * (PartialRequirement * BoostFrames) * LaneWidth;
                var partialTurboDistance = 1 / TurboFrames * (PartialRequirement * TurboFrames) * LaneWidth;


                if (boostCapability - Fudge < distanceToClear || partialTurboDistance > distanceToClear) {
                    animator.SetBool(AnimatorBoost, false);
                    animator.SetBool(AnimatorTurbo, true);
                } else if (changeCapability - Fudge < distanceToClear || partialBoostDistance > distanceToClear) {
                    animator.SetBool(AnimatorBoost, true);
                    animator.SetBool(AnimatorTurbo, false);
                } else {
                    animator.SetBool(AnimatorBoost, false);
                    animator.SetBool(AnimatorTurbo, false);
                }
            } else {
                animator.SetBool(AnimatorBoost, false);
                animator.SetBool(AnimatorTurbo, false);
            }

            var trans = transform;
            var positionNow = trans.position;
            if (Math.Abs(ChangeDirection) > .01) {
                positionNow.x = changeProgress * ChangeDirection * LaneWidth + ChangeStartPosition;
            }

            positionNow.z = trans.position.z + Speed * Time.deltaTime;
            trans.position = positionNow;
        }


        private void OnEnable() {
            inputAction?.Enable();
            Obstacle.OnObstacleWidthSet += RegisterObstacleWidth;
        }

        private void OnDisable() {
            inputAction?.Disable();
            Obstacle.OnObstacleWidthSet -= RegisterObstacleWidth;
        }

        private void RegisterObstacleWidth(Vector3 size) {
            obstacleWidth = size.x;
        }

        public void OnTriggerEnter(Collider other) {
            var obstacle = other.GetComponent<Obstacle>();
            if (obstacle != null) {
                CollideWithObstacle();
            }
        }

        private void CollideWithObstacle() {
            OnCourseFinished?.Invoke();
            collided = true;
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void AddTurbo() {
            speedBoost += turbo;
        }

        public void AddBoost() {
            speedBoost += boost;
        }

        public float CalculateClearDistance() {
            return Math.Max(initialSpeed + turbo, Speed + turbo);
        }
    }
}