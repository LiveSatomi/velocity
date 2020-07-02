﻿#region

using Controller;
using Ship;
using TMPro;
using UnityEngine;

#endregion

namespace Ui {
    public class MinimumSpeedTextUpdater : MonoBehaviour {
        private TextMeshProUGUI text;

        public TimeController timeController;

        public ShipDriver shipDriver;

        // Start is called before the first frame update
        private void Start() {
            text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        private void Update() {
            var currentMinSpeed = shipDriver.BaseSpeed;
            text.text = "Minimum: " + currentMinSpeed.ToString("n1");
        }
    }
}