using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Track
{
    public class TrackSection : MonoBehaviour
    {

        private Boolean preparedNextSection;
        private Transform threshold;
        private GameObject player;
        public TrackBuilder builder;
        private Transform endpoint;

        // Start is called before the first frame update
        void Start()
        {
            endpoint = transform.Find("Endpoint");
            player = GameObject.FindWithTag("Player");
            threshold = transform.Find("Threshold");
            builder = Object.FindObjectOfType<TrackBuilder>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!preparedNextSection)
            {
                if (player.transform.position.z > threshold.position.z)
                {
                    builder.AddSection(endpoint);
                    preparedNextSection = true;
                }
            }
        }
    }
}
