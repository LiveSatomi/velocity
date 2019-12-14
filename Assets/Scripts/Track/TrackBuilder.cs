using UnityEngine;

namespace Track
{
    public class TrackBuilder : MonoBehaviour
    {

        public GameObject trackSection;

        // Start is called before the first frame update
        void Start()
        {
            GameObject section = Instantiate(trackSection, new Vector3(0, 0, 0), Quaternion.identity, transform);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void AddSection(Transform endpoint)
        {
            var endpointPosition = endpoint.position;
            GameObject section = Instantiate(trackSection, new Vector3(0, 0, 0), Quaternion.identity, transform);
            var startpoint = section.transform.Find("Startpoint");
            section.transform.position = endpointPosition + (section.transform.position - startpoint.position);
            

        }
    }
}
