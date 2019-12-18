using System;
using Controller;
using UnityEngine;

namespace Ship
{
    public class ShipDriver : MonoBehaviour
    {
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
        private float speed;
        private Animator animator;

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
            animator.SetFloat("changeDirection", InputDirection);
            speed = timeController.CurrentMinSpeed() + speedBoost;
            if (Math.Abs(ChangeDirection) > .01)
            {
                var trans = transform;
                var positionNow = trans.position;
                positionNow.x = changeProgress * ChangeDirection * laneWidth + ChangeStartPosition;
                trans.position = positionNow;
            }
        }

        void FixedUpdate()
        {
            rb.velocity = new Vector3(0, 0, speed);
            
        }

        private void OnEnable()
        {
            inputAction.Enable();
        }

        private void OnDisable()
        {
            inputAction.Disable();
        }
    }
}
