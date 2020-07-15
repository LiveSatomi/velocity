using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Ship {
    /// <summary>
    ///     A ShipInputController controlled by InputActions in the UnityEngine.InputSystem namespace.
    /// </summary>
    public class InputActionShipInputController : ShipInputController {
        /// <summary>
        ///     Unity InputAction that controls the ship.
        /// </summary>
        private ShipInputAction inputAction;

        public override event DirectionChangedEvent OnDirectionChanged;

        protected override void Awake() {
            base.Awake();
            inputAction = new ShipInputAction();
            inputAction.ShipControls.ChangeLane.performed += ctx => {
                if (ctx.control.device is Touchscreen) {
                    var controlParent = ctx.control.parent;
                    var x = (controlParent as TouchControl)?.position.x;
                    if (x?.ReadValue() > Screen.width / 2f) {
                        OnDirectionChanged?.Invoke(1);
                    } else {
                        OnDirectionChanged?.Invoke(-1);
                    }
                } else {
                    OnDirectionChanged?.Invoke(Math.Sign(ctx.ReadValue<float>()));
                }
            };
            inputAction.ShipControls.ChangeLane.canceled += ctx => { OnDirectionChanged?.Invoke(0); };
        }

        private void OnEnable() {
            inputAction?.Enable();
        }


        private void OnDisable() {
            inputAction?.Disable();
        }
    }
}