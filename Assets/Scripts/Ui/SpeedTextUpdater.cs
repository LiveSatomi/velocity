using Ship;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class SpeedTextUpdater : MonoBehaviour
    {
        public ShipDriver ship;
        private TextMeshProUGUI text;
        private Rigidbody body;

        // Start is called before the first frame update
        void Start()
        {
            body = ship.GetComponent<Rigidbody>();
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            var speed = body.velocity.z;
            text.text = "Speed: " + speed.ToString("n1");
        }
    }
}
