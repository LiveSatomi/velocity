using UnityEngine;
using Utility;

namespace Track
{
    /**
     * Builds the mesh for a TrackSection. Pivot is in the midpoint of the plane.
     */
    public class SectionBuilder: MonoBehaviour
    {
        /**
         * A prefab with components other than the mesh.
         */
        public TrackSection section;

        /**
         * Number of lanes.
         */
        public int lanes = 3;

        /**
         * Size that a single lane can accomodate.
         */
        public int laneWidth = 3;

        /**
         * Size of the space between the lanes.
         */
        public float gutterWidth = .25f;

        /**
         * Difference between where the plane is entered and exited.
         */
        public int length = 10;

        /**
         * Distance before the entrance that the threshold is placed.
         */
        public int thresholdDistance = 5;

        /**
         * Generates a mesh adds it to the TrackSection.
         */
        public TrackSection BuildSection()
        {
            var width = laneWidth * lanes + gutterWidth * (lanes - 2);
            var sectionMesh = MeshCreator.CreatePlane( "GeneratedPlane", 1, 2, width, length, Orientation.Horizontal, false);
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