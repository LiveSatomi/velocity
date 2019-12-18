using UnityEngine;

namespace Controller
{
    public class TimeController : MonoBehaviour
    {
        private float elapsedTime;
    
        public float minSpeed = 3;

        // Update is called once per frame
        void Update()
        {
            elapsedTime += Time.deltaTime;
        }


        public float CurrentMinSpeed()
        {
            return minSpeed + elapsedTime / 10;
        }
    }
}
