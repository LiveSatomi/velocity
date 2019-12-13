// GENERATED AUTOMATICALLY FROM 'Assets/Input/ShipInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ShipInputAction : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @ShipInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ShipInputAction"",
    ""maps"": [
        {
            ""name"": ""ShipControls"",
            ""id"": ""8fd8bb6f-7ccb-4327-8470-0e63b2cd77cf"",
            ""actions"": [
                {
                    ""name"": ""ChangeLane"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6e0bf8a1-b130-4a71-889b-a3226c6dd4f7"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrow"",
                    ""id"": ""558bfff3-9e53-44c7-8ae6-f34c43ed5565"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeLane"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""d4c6ef01-8890-4d98-9836-2490f2bd9870"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeLane"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b5b07a9f-3095-4892-882c-38eff37c0c14"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeLane"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ShipControls
        m_ShipControls = asset.FindActionMap("ShipControls", throwIfNotFound: true);
        m_ShipControls_ChangeLane = m_ShipControls.FindAction("ChangeLane", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // ShipControls
    private readonly InputActionMap m_ShipControls;
    private IShipControlsActions m_ShipControlsActionsCallbackInterface;
    private readonly InputAction m_ShipControls_ChangeLane;
    public struct ShipControlsActions
    {
        private @ShipInputAction m_Wrapper;
        public ShipControlsActions(@ShipInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @ChangeLane => m_Wrapper.m_ShipControls_ChangeLane;
        public InputActionMap Get() { return m_Wrapper.m_ShipControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ShipControlsActions set) { return set.Get(); }
        public void SetCallbacks(IShipControlsActions instance)
        {
            if (m_Wrapper.m_ShipControlsActionsCallbackInterface != null)
            {
                @ChangeLane.started -= m_Wrapper.m_ShipControlsActionsCallbackInterface.OnChangeLane;
                @ChangeLane.performed -= m_Wrapper.m_ShipControlsActionsCallbackInterface.OnChangeLane;
                @ChangeLane.canceled -= m_Wrapper.m_ShipControlsActionsCallbackInterface.OnChangeLane;
            }
            m_Wrapper.m_ShipControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ChangeLane.started += instance.OnChangeLane;
                @ChangeLane.performed += instance.OnChangeLane;
                @ChangeLane.canceled += instance.OnChangeLane;
            }
        }
    }
    public ShipControlsActions @ShipControls => new ShipControlsActions(this);
    public interface IShipControlsActions
    {
        void OnChangeLane(InputAction.CallbackContext context);
    }
}
