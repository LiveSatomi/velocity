using Ship;
using TMPro;
using UnityEngine;

namespace Ui {
    public class BoostTextUpdater : MonoBehaviour {

        public ShipDriver ship;

        private TextMeshProUGUI text;

        // Start is called before the first frame update
        private void Start() {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        private void Update() {
            var speed = ship.Speed;
            text.text = "Speed: " + speed.ToString("n1");
        }
    }
}