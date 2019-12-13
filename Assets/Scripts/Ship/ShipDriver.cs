﻿using UnityEngine;

namespace Ship
{
    public class ShipDriver : MonoBehaviour
    {
        public Rigidbody rb;
    
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void FixedUpdate()
        {
            rb.velocity = new Vector3(0, 0, 1);
        }
    }
}
