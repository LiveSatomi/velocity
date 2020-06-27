using Lean.Pool;
using Ship;
using UnityEngine;
using Utility;

namespace Track
{
    public class TrackBuilder : MonoBehaviour
    {
        public SectionBuilder sectionBuilder;

        public ObstaclePlacer obstaclePlacer;

        /// <summary>
        /// Number of sections generated ahead of the player.
        /// </summary>
        public int lookAhead = 10; 
        
        private TrackSection trackSection;

        private TrackSection lastSection;
        
        // Start is called before the first frame update
        void Start()
        {
            trackSection = sectionBuilder.BuildSection();
            GetComponent<LeanGameObjectPool>().Prefab = trackSection.gameObject;
            lastSection = AddSection(transform);
            for (int i = 1; i < lookAhead; i++)
            {
                lastSection = AddSection(lastSection.EndPoint);
            }
        }

        public void AddSection()
        {
            lastSection = AddSection(lastSection.EndPoint);
        }
        
        private TrackSection AddSection(Transform endpoint)
        {
            var position = new Vector3(0, 0,endpoint.position.z + sectionBuilder.length / 2f);
            var newSection = LeanPool.Spawn(trackSection, position, Quaternion.identity, transform);
            return newSection;
        }
    }
}
