// GENERATED AUTOMATICALLY FROM 'Assets/InputActions/PlayerMouseInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerMouseInputAction : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerMouseInputAction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerMouseInputAction"",
    ""maps"": [
        {
            ""name"": ""Mouse"",
            ""id"": ""d9cf7167-d65d-4f37-83ad-8105796c2512"",
            ""actions"": [
                {
                    ""name"": ""MousePosition"",
                    ""type"": ""Value"",
                    ""id"": ""ece7623b-8764-4cd0-85ea-57abdafd27bc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftMouseFire"",
                    ""type"": ""Button"",
                    ""id"": ""72d6813c-f8b2-48e4-8425-e69dc7179b4c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightMouseFire"",
                    ""type"": ""Button"",
                    ""id"": ""75ace1be-c76e-4352-9b51-93eddd8e8e57"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""EndlessShooting"",
                    ""type"": ""Button"",
                    ""id"": ""1aaefec3-22ef-40cb-8479-7459c2cd6ca2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e4cc67e8-1604-47af-af17-ca704067a649"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MousePosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aeb1ad9f-e523-437d-bdd0-03c3a618035c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftMouseFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c849129f-2f67-40d3-89e0-0a0454598823"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightMouseFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e56c41e0-868d-4e4e-b620-0400488afe1b"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EndlessShooting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse
        m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
        m_Mouse_MousePosition = m_Mouse.FindAction("MousePosition", throwIfNotFound: true);
        m_Mouse_LeftMouseFire = m_Mouse.FindAction("LeftMouseFire", throwIfNotFound: true);
        m_Mouse_RightMouseFire = m_Mouse.FindAction("RightMouseFire", throwIfNotFound: true);
        m_Mouse_EndlessShooting = m_Mouse.FindAction("EndlessShooting", throwIfNotFound: true);
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

    // Mouse
    private readonly InputActionMap m_Mouse;
    private IMouseActions m_MouseActionsCallbackInterface;
    private readonly InputAction m_Mouse_MousePosition;
    private readonly InputAction m_Mouse_LeftMouseFire;
    private readonly InputAction m_Mouse_RightMouseFire;
    private readonly InputAction m_Mouse_EndlessShooting;
    public struct MouseActions
    {
        private @PlayerMouseInputAction m_Wrapper;
        public MouseActions(@PlayerMouseInputAction wrapper) { m_Wrapper = wrapper; }
        public InputAction @MousePosition => m_Wrapper.m_Mouse_MousePosition;
        public InputAction @LeftMouseFire => m_Wrapper.m_Mouse_LeftMouseFire;
        public InputAction @RightMouseFire => m_Wrapper.m_Mouse_RightMouseFire;
        public InputAction @EndlessShooting => m_Wrapper.m_Mouse_EndlessShooting;
        public InputActionMap Get() { return m_Wrapper.m_Mouse; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
        public void SetCallbacks(IMouseActions instance)
        {
            if (m_Wrapper.m_MouseActionsCallbackInterface != null)
            {
                @MousePosition.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePosition;
                @LeftMouseFire.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnLeftMouseFire;
                @LeftMouseFire.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnLeftMouseFire;
                @LeftMouseFire.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnLeftMouseFire;
                @RightMouseFire.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnRightMouseFire;
                @RightMouseFire.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnRightMouseFire;
                @RightMouseFire.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnRightMouseFire;
                @EndlessShooting.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnEndlessShooting;
                @EndlessShooting.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnEndlessShooting;
                @EndlessShooting.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnEndlessShooting;
            }
            m_Wrapper.m_MouseActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @LeftMouseFire.started += instance.OnLeftMouseFire;
                @LeftMouseFire.performed += instance.OnLeftMouseFire;
                @LeftMouseFire.canceled += instance.OnLeftMouseFire;
                @RightMouseFire.started += instance.OnRightMouseFire;
                @RightMouseFire.performed += instance.OnRightMouseFire;
                @RightMouseFire.canceled += instance.OnRightMouseFire;
                @EndlessShooting.started += instance.OnEndlessShooting;
                @EndlessShooting.performed += instance.OnEndlessShooting;
                @EndlessShooting.canceled += instance.OnEndlessShooting;
            }
        }
    }
    public MouseActions @Mouse => new MouseActions(this);
    public interface IMouseActions
    {
        void OnMousePosition(InputAction.CallbackContext context);
        void OnLeftMouseFire(InputAction.CallbackContext context);
        void OnRightMouseFire(InputAction.CallbackContext context);
        void OnEndlessShooting(InputAction.CallbackContext context);
    }
}
