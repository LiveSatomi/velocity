#region

using System;
using Controller;
using Track;
using UnityEngine;
using UnityEngine.SceneManagement;

#endregion

namespace Ship {
    public class ShipDriver : MonoBehaviour {
        public delegate void CourseFinishedEvent();

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
        ///     Cached animator component.
        /// </summary>
        private Animator animator;


        /// <summary>
        ///     Indicates the percentage of lane change animation completed. Controlled by animator.
        /// </summary>
        [HideInInspector] public float changeProgress;

        /// <summary>
        ///     State that represents if the ship has collided with an obstacle.
        /// </summary>
        private bool collided;

        /// <summary>
        ///     Decay rate of speedBoost.
        /// </summary>
        [Range(0, 1)] public float decayRate;

        /// <summary>
        ///     Unity InputAction that controls the ship.
        /// </summary>
        private ShipInputAction inputAction;

        // TODO Initialize this from TrackBuilder or elsewhere
        public float laneWidth = 2;

        /// <summary>
        ///     Cached rigidbody component.
        /// </summary>
        private Rigidbody rb;

        /// <summary>
        ///     Speed boost based on performance. Decays to 0 over time.
        /// </summary>
        private float speedBoost;

        /// <summary>
        ///     Authority for elapsed time.
        /// </summary>
        public TimeController timeController;

        [SerializeField] private float turbo = 3f;

        [SerializeField] private float boost = 1f;


        /// <summary>
        ///     State tied to the desired direction to change lanes.
        /// </summary>
        public float InputDirection { get; private set; }

        /// <summary>
        ///     State that represents the direction of the lane change being executed.
        /// </summary>
        public float ChangeDirection { get; set; }

        /// <summary>
        ///     State that tells where the animation started. Used to adjust for rounding errors at the end of each lane change.
        /// </summary>
        public float ChangeStartPosition { get; set; }

        /// <summary>
        ///     Current forward velocity.
        /// </summary>
        public float Speed { get; private set; }

        public static event CourseFinishedEvent OnCourseFinished;

        private void Awake() {
            inputAction = new ShipInputAction();
            inputAction.ShipControls.ChangeLane.performed += ctx => {
                InputDirection = Math.Sign(ctx.ReadValue<float>());
            };
        }

        // Start is called before the first frame update
        private void Start() {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update() {
            if (!collided) {
                animator.SetFloat(AnimatorChangeDirection, InputDirection);
                Speed = timeController.CurrentMinSpeed() + speedBoost;
                speedBoost = Math.Max(0, speedBoost - decayRate * Time.deltaTime);
            }
        }

        private void FixedUpdate() {
            var trans = transform;
            var positionNow = trans.position;
            if (Math.Abs(ChangeDirection) > .01) {
                positionNow.x = changeProgress * ChangeDirection * laneWidth + ChangeStartPosition;
            }

            positionNow.z = trans.position.z + Speed * Time.deltaTime;
            trans.position = positionNow;
        }


        private void OnEnable() {
            inputAction.Enable();
        }

        private void OnDisable() {
            inputAction.Disable();
        }

        public void OnTriggerEnter(Collider other) {
            var obstacle = other.GetComponent<Obstacle>();
            if (obstacle != null) {
                if (obstacle.IsBoostCollider(other)) {
                    animator.SetBool(AnimatorBoost, true);
                } else if (obstacle.IsTurboCollider(other)) {
                    animator.SetBool(AnimatorTurbo, true);
                } else {
                    CollideWithObstacle(obstacle);
                }
            }
        }

        public void OnTriggerExit(Collider other) {
            var obstacle = other.GetComponent<Obstacle>();
            if (obstacle != null) {
                if (obstacle.IsBoostCollider(other)) {
                    animator.SetBool(AnimatorBoost, false);
                } else if (obstacle.IsTurboCollider(other)) {
                    animator.SetBool(AnimatorTurbo, false);
                }
            }
        }

        private void CollideWithObstacle(Obstacle obstacle) {
            OnCourseFinished?.Invoke();
            collided = true;
            rb.velocity = new Vector3(0, 0, 0);
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }

        public void AddTurbo() {
            speedBoost += turbo;
        }

        public void AddBoost() {
            speedBoost += boost;
        }
    }
}