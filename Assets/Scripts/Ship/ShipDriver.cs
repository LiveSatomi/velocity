using System;
using UnityEngine;

namespace Ship
{
    public class ShipDriver : MonoBehaviour
    {
        private Rigidbody rb;

        private ShipInputAction inputAction;

        public float InputDirection { get; private set; }
        public float changeSpeed;
        public float changeProgress;
        public float ChangeDirection { get; set; }
        public float ChangeStartPosition { get; set; }

        public float minSpeed = 3;
        private Animator animator;
        public AnimationCurve curve;

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
            if (Math.Abs(ChangeDirection) > .01)
            {
                var positionNow = transform.position;
                positionNow.x = changeProgress * ChangeDirection + ChangeStartPosition;
                transform.position = positionNow;
            }
        }

        void FixedUpdate()
        {
            rb.velocity = new Vector3(0, 0, minSpeed);
            
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
