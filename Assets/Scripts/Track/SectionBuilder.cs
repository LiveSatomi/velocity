using UnityEngine;
using Utility;

namespace Track {
    ///
    /// Builds the mesh for a TrackSection. Pivot is in the midpoint of the plane.
    ///
    public class SectionBuilder : MonoBehaviour {
        ///
        /// Size of the space between the lanes.
        ///
        public float gutterWidth = .25f;

        ///
        /// Number of lanes.
        ///
        public int lanes = 3;

        ///
        /// Size that a single lane can accomodate.
        ///
        public int laneWidth = 3;

        ///
        /// Difference between where the plane is entered and exited.
        ///
        public int laneLength = 10;

        ///
        /// A prefab with components other than the mesh.
        ///
        public TrackSection section;

        /// Distance before the entrance that the threshold is placed.
        public int thresholdDistance = 5;

        ///
        /// Generates a TrackSection made out of LaneSections and GutterSections.
        ///
        public TrackSection BuildSection() {
            var trackSection = Instantiate(section);
            trackSection.name = "Section";
            trackSection.gameObject.SetActive(false);

            var trans = trackSection.transform;

            var startPoint = new GameObject("StartPoint") {tag = "StartPoint"};
            startPoint.transform.parent = trans;
            startPoint.transform.localPosition = new Vector3(0, 0, -laneLength / 2f);

            var endPoint = new GameObject("EndPoint") {tag = "EndPoint"};
            endPoint.transform.parent = trans;
            endPoint.transform.localPosition = new Vector3(0, 0, laneLength / 2f);

            var threshold = new GameObject("Threshold") {tag = "Threshold"};
            threshold.transform.parent = trans;
            threshold.transform.localPosition = new Vector3(0, 0, -laneLength / 2f - thresholdDistance);


            for (var i = -lanes; i < lanes; i++) {
                var offset = i * (laneWidth + gutterWidth);
                var laneSection = CreateSectionMesh("LaneSection", offset, laneWidth, section.laneMaterial);
                offset = i * (laneWidth + gutterWidth) + (laneWidth + gutterWidth) / 2.0f;
                var gutterSection = CreateSectionMesh("GutterSection", offset, gutterWidth, section.gutterMaterial);
                laneSection.transform.parent = trans.Find("Lanes").transform;
                gutterSection.transform.parent = trans.Find("Gutters").transform;
            }

            trackSection.Lanes = lanes;

            return trackSection;
        }

        ///
        /// Instantiates a Section.
        ///
        private GameObject CreateSectionMesh(string id, float offset, float width, Material material) {
            var laneSection = new GameObject(id);
            laneSection.transform.position = new Vector3(offset, 0, 0);

            var sectionMesh =
                MeshCreator.CreatePlane("GeneratedPlane", 1, 2, width, laneLength, Orientation.Horizontal, false);

            var sectionFilter = laneSection.gameObject.AddComponent<MeshFilter>();
            sectionFilter.sharedMesh = sectionMesh;
            var sectionRenderer = laneSection.gameObject.AddComponent<MeshRenderer>();
            sectionRenderer.material = material;
            return laneSection;
        }
    }
}