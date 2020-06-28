using System;
using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ship {
    public class ShipDriver : MonoBehaviour {
        public delegate void CourseFinishedEvent();

        /// <summary>
        ///     Cached animator parameter.
        /// </summary>
        private static readonly int AnimatorChangeDirection = Animator.StringToHash("changeDirection");

        /// <summary>
        ///     Speed boost based on performance. Decays to 0 over time.
        /// </summary>
        private readonly float speedBoost = 1;

        /// <summary>
        ///     Cached animator component.
        /// </summary>
        private Animator animator;

        /// <summary>
        ///     Indicates the percentage of lane change animation completed. Controlled by animator.
        /// </summary>
        public float changeProgress;

        /// <summary>
        ///     State that represents if the ship has collided with an obstacle.
        /// </summary>
        private bool collided;

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
        ///     Authority for elapsed time.
        /// </summary>
        public TimeController timeController;

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

        void Awake() {
            inputAction = new ShipInputAction();
            inputAction.ShipControls.ChangeLane.performed += ctx => {
                InputDirection = Math.Sign(ctx.ReadValue<float>());
            };
        }

        // Start is called before the first frame update
        void Start() {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update() {
            if (!collided) {
                animator.SetFloat(AnimatorChangeDirection, InputDirection);
                Speed = timeController.CurrentMinSpeed() + speedBoost;
            }
        }

        void FixedUpdate() {
            var trans = transform;
            var positionNow = trans.position;
            if (Math.Abs(ChangeDirection) > .01)
                positionNow.x = changeProgress * ChangeDirection * laneWidth + ChangeStartPosition;

            positionNow.z = trans.position.z + Speed * Time.deltaTime;
            trans.position = positionNow;
        }


        void OnEnable() {
            inputAction.Enable();
        }

        void OnDisable() {
            inputAction.Disable();
        }

        public void CollideWithObstacle() {
            OnCourseFinished?.Invoke();
            collided = true;
            rb.velocity = new Vector3(0, 0, 0);
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}