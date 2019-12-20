using UnityEngine;
using Utility;

namespace Track
{
    public class SectionBuilder: MonoBehaviour
    {

        public int width = 1;

        public int length = 10;

        public int thresholdDistance = 5;
        
        public TrackSection BuildSection()
        {
            var section = CreatePlane.Create("Plane", 1, 1, width, length, Orientation.Horizontal, false);
            var startPoint = new GameObject("StartPoint") {tag = "StartPoint"};
            startPoint.transform.parent = section.transform;
            startPoint.transform.localPosition = new Vector3(0, 0, -length / 2f);

            var endPoint = new GameObject("EndPoint") {tag = "EndPoint"};
            endPoint.transform.parent = section.transform;
            endPoint.transform.localPosition = new Vector3(0, 0, length / 2f);
            
            var threshold = new GameObject("Threshold") {tag = "Threshold"};
            threshold.transform.parent = section.transform;
            threshold.transform.localPosition = new Vector3(0, 0, -length / 2f - thresholdDistance);

            return section.AddComponent<TrackSection>();
        }
    }
}