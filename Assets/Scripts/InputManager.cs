using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.XInput;

namespace Christina.Typography
{
    /// <summary>
    /// Component responsible for keeping a reference to our PlayerInputs and performing operations on it.
    /// </summary>
    [CreateAssetMenu(fileName = "InputManager", menuName = "InputManager", order = 0)]
    public class InputManager : ScriptableObject
    {
        private PlayerInput _playerInput;

        // tracks which device type is active based on recent changes
        private DeviceType activeDevice = DeviceType.Keyboard;

        /// <summary>
        /// Event fired when the active device changes (from xbox to keyboard)
        /// </summary>
        public event Action ActiveDeviceChangeEvent;

        /// <summary>
        /// Event fired when an input binding changes
        /// </summary>
        public event Action BindingsChangedEvent;
        
        private void OnEnable()
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInput();
                // enable the input so we start getting events
                _playerInput.Player.Enable();

                InputSystem.onActionChange += TrackActions;
            }
        }

        public DeviceType GetActiveDevice()
        {
            return activeDevice;
        }

        private void OnDisable()
        {
            InputSystem.onActionChange -= TrackActions;
        }

        public void DisablePlayer()
        {
            _playerInput.Player.Disable();
        }

        public void EnablePlayer()
        {
            _playerInput.Player.Enable();
        }

        /// <summary>
        /// listener that we'll tie to the InputSystem
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="change"></param>
        private void TrackActions(object obj, InputActionChange change)
        {
            if (change == InputActionChange.ActionPerformed)
            {
                InputAction inputAction = (InputAction)obj;
                InputControl activeControl = inputAction.activeControl;
                Debug.LogFormat("Current Control {0}", activeControl);

                var newDevice = DeviceType.Keyboard;

                if (activeControl.device is Keyboard)
                {
                    newDevice = DeviceType.Keyboard;
                }

                if (activeControl.device is Gamepad)
                {
                    newDevice = DeviceType.Gamepad;

                    // we can further categorize these if we had spritesheets per brand
                    if (activeControl.device is XInputController)
                    {
                        Debug.LogFormat("Device is Xbox Controller");
                    }

                    if (activeControl.device is DualShockGamepad)
                    {
                        Debug.LogFormat("Device is Playstation");
                    }
                }

                // we detected a change
                if (activeDevice != newDevice)
                {
                    activeDevice = newDevice;
                    // fire an event to anyone listening
                    ActiveDeviceChangeEvent?.Invoke();
                }
            }

            if (change == InputActionChange.BoundControlsChanged)
            {
                BindingsChangedEvent?.Invoke();
            }
        }


        public PlayerInput GetPlayerInput()
        {
            return _playerInput;
        }

        public InputAction GetAction(string actionName)
        {
            return _playerInput.FindAction(actionName);
        }

        public InputBinding GetBinding(string actionName, DeviceType deviceType)
        {
            InputAction action = GetAction(actionName);

            InputBinding deviceBinding = action.bindings[(int)deviceType];
            return deviceBinding;
        }
    }
}