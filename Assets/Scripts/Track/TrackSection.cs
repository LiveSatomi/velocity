using System;
using Lean.Pool;
using Ship;
using UnityEngine;

namespace Track
{
    public class TrackSection : MonoBehaviour, IPoolable
    {

        public Material material;
        
        public Transform Threshold { get; private set; }
        public Transform StartPoint { get; private set; }
        public Transform EndPoint { get; private set; }

        private ShipDriver ship;
        private TrackBuilder track;

        private bool thresholdPassed;
        
        void Awake()
        {
            StartPoint = transform.Find("StartPoint");
            EndPoint = transform.Find("EndPoint");
            Threshold = transform.Find("Threshold");
            ship = GameObject.FindWithTag("Player").GetComponent<ShipDriver>();
            track = GameObject.FindWithTag("Track").GetComponent<TrackBuilder>();
            
        }

        private void Update()
        {
            if (!thresholdPassed && ship.transform.position.z > Threshold.transform.position.z)
            {
                track.AddSection();
                thresholdPassed = true;
            }
        }

        public void OnSpawn()
        {
            
        }

        public void OnDespawn()
        {
            thresholdPassed = false;
        }
    }
}
