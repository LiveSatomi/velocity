using System;
using Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ship
{
    public class ShipDriver : MonoBehaviour {
        public delegate void CourseFinishedEvent();

        public static event CourseFinishedEvent OnCourseFinished;
        
        private Rigidbody rb;

        private ShipInputAction inputAction;

        public TimeController timeController;
        
        public float InputDirection { get; private set; }
        public float changeProgress;
        public float ChangeDirection { get; set; }
        public float ChangeStartPosition { get; set; }

        // TODO Initialize this from TrackBuilder or elsewhere
        public float laneWidth = 2;

        private float speedBoost = 1;
        public float Speed { get; private set; }

        private bool collided = false;
        
        private Animator animator;

        private static readonly int AnimatorChangeDirection = Animator.StringToHash("changeDirection");

        void Awake()
        {
            inputAction = new ShipInputAction();
            inputAction.ShipControls.ChangeLane.performed += ctx =>
            {
                InputDirection = Math.Sign(ctx.ReadValue<float>());
            };
        }
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
            if (!collided) {
                animator.SetFloat(AnimatorChangeDirection, InputDirection);
                Speed = timeController.CurrentMinSpeed() + speedBoost;
                var trans = transform;
                var positionNow = trans.position;
                if (Math.Abs(ChangeDirection) > .01)
                {
                    positionNow.x = changeProgress * ChangeDirection * laneWidth + ChangeStartPosition;
                }

                positionNow.z = trans.position.z + Speed * Time.deltaTime;
                trans.position = positionNow;
            }
        }


        private void OnEnable()
        {
            inputAction.Enable();
        }

        private void OnDisable()
        {
            inputAction.Disable();
        }

        public void CollideWithObstacle() {
            OnCourseFinished?.Invoke();
            collided = true;
            rb.velocity = new Vector3(0,0,0);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
