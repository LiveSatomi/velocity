using Ship;
using TMPro;
using UnityEngine;

namespace Ui {
    public class SpeedTextUpdater : MonoBehaviour {
        private Rigidbody body;

        public ShipDriver ship;

        private TextMeshProUGUI text;

        // Start is called before the first frame update
        private void Start() {
            body = ship.GetComponent<Rigidbody>();
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        private void Update() {
            var speed = body.velocity.z;
            text.text = "Speed: " + speed.ToString("n1");
        }
    }
}