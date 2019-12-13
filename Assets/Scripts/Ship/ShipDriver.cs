using System;
using UnityEngine;

namespace Ship
{
    public class ShipDriver : MonoBehaviour
    {
        private Rigidbody rb;

        private ShipInputAction inputAction;

        private float changeDirection;
        private float xVelocity;

        void Awake()
        {
            inputAction = new ShipInputAction();
            inputAction.ShipControls.ChangeLane.performed += ctx =>
            {
                changeDirection = ctx.ReadValue<float>();
            };
        }
        
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (changeDirection < 0)
            {
                xVelocity = -1;
            } else if (changeDirection > 0)
            {
                xVelocity = 1;
            }
            else
            {
                xVelocity = 0;
            }
        }

        void FixedUpdate()
        {
            rb.velocity = new Vector3(xVelocity, 0, 1);
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
