using UnityEngine;
using Utility;

namespace Track
{
    public class SectionBuilder: MonoBehaviour
    {

        public TrackSection section;
        
        public int width = 1;

        public int length = 10;

        public int thresholdDistance = 5;
        
        public TrackSection BuildSection()
        {
            var sectionMesh = MeshCreator.CreatePlane( "GeneratedPlane", 1, 1, width, length, Orientation.Horizontal, false);
            var trackSection = Instantiate(section);
            trackSection.name = "Section";
            trackSection.gameObject.SetActive(false);
            
            
            
            MeshFilter meshFilter = trackSection.gameObject.AddComponent<MeshFilter>();
            meshFilter.sharedMesh = sectionMesh;
            var meshRenderer = trackSection.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = section.material;

            var trans = trackSection.transform;

            var startPoint = new GameObject("StartPoint") {tag = "StartPoint"};
            startPoint.transform.parent = trans;
            startPoint.transform.localPosition = new Vector3(0, 0, -length / 2f);

            var endPoint = new GameObject("EndPoint") {tag = "EndPoint"};
            endPoint.transform.parent = trans;
            endPoint.transform.localPosition = new Vector3(0, 0, length / 2f);
            
            var threshold = new GameObject("Threshold") {tag = "Threshold"};
            threshold.transform.parent = trans;
            threshold.transform.localPosition = new Vector3(0, 0, -length / 2f - thresholdDistance);

            return trackSection;
        }
    }
}