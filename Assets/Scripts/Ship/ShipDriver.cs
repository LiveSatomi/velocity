using System;
using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ship {
    public class ShipDriver : MonoBehaviour {
        public delegate void CourseFinishedEvent();

        private static readonly int AnimatorChangeDirection = Animator.StringToHash("changeDirection");

        private readonly float speedBoost = 1;

        private Animator animator;

        public float changeProgress;

        private bool collided;

        private ShipInputAction inputAction;

        // TODO Initialize this from TrackBuilder or elsewhere
        public float laneWidth = 2;

        private Rigidbody rb;

        public TimeController timeController;

        public float InputDirection { get; private set; }

        public float ChangeDirection { get; set; }

        public float ChangeStartPosition { get; set; }

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
            }
        }

        private void FixedUpdate() {
            var trans = transform;
            var positionNow = trans.position;
            if (Math.Abs(ChangeDirection) > .01)
                positionNow.x = changeProgress * ChangeDirection * laneWidth + ChangeStartPosition;

            positionNow.z = trans.position.z + Speed * Time.deltaTime;
            trans.position = positionNow;
        }


        private void OnEnable() {
            inputAction.Enable();
        }

        private void OnDisable() {
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