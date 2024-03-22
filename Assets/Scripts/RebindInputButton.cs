using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Christina.Typography
{
    /// <summary>
    ///  Component responsible for rebinding an input
    /// </summary>
    public class RebindInputButton : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;

        [SerializeField] private GameObject rebindInProgressPanel;

        [SerializeField] private string actionName;
        [SerializeField] private SetTextToTextBox setTextComponent;

        protected InputActionRebindingExtensions.RebindingOperation rebindingOperation;

        public void StartInteractiveRebind()
        {
            rebindInProgressPanel.SetActive(true);
            inputManager.DisablePlayer();

            InputAction action = inputManager.GetAction(actionName);

            var bindingGroup = "Keyboard";
            if (inputManager.GetActiveDevice() == DeviceType.Gamepad)
            {
                bindingGroup = "Gamepad";
            }
            
            rebindingOperation = action.PerformInteractiveRebinding()
                .WithControlsExcluding("<Gamepad>/Start")
                .WithCancelingThrough("<Gamepad>/Start")
                .OnPotentialMatch(operation =>
                {
                    // work around for bug where rebinding to E causes cancellation
                    if (operation.selectedControl.path is "/Keyboard/escape")
                    {
                        operation.Cancel();
                    }
                })
                .WithBindingGroup(bindingGroup)
                .OnMatchWaitForAnother(0.1f)
                .OnCancel(_ => RebindCancelled())
                .OnComplete(_ => RebindComplete());

            rebindingOperation.Start();
        }

        private void RebindCancelled()
        {
            rebindingOperation.Dispose();
            inputManager.EnablePlayer();
            rebindInProgressPanel.SetActive(false);
        }

        private void RebindComplete()
        {
            rebindingOperation.Dispose();
            inputManager.EnablePlayer();
            rebindInProgressPanel.SetActive(false);
            // ReloadText
            // TODO this is where you would want to Save your bindings
        }
    }
}