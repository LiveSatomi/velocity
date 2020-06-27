using Controller;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class MinimumSpeedTextUpdater : MonoBehaviour
    {
        public TimeController timeController;
        private TextMeshProUGUI text;

        // Start is called before the first frame update
        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            var currentMinSpeed = timeController.CurrentMinSpeed();
            text.text = "Minimum: " + currentMinSpeed.ToString("n1");
        }
    }
}
