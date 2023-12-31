using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Christina.Typography
{
    [RequireComponent(typeof(TMP_Text))]
    public class SetTextToTextBox : MonoBehaviour
    {
        [TextArea(2, 3)] [SerializeField] private string message = "Press {Player/Interact} to interact which is also {Player/Interact}.";

        [Header("Setup for sprites")] [SerializeField]
        private ListOfTmpSpriteAssets listOfTmpSpriteAssets;

        [SerializeField] private DeviceType deviceType;

        [SerializeField] private InputManager inputManager;

        /// <summary>
        ///  class auto generated by the InputSystem
        /// </summary>
        private PlayerInput _playerInput;

        private TMP_Text _textBox;

        private void Awake()
        {
            _playerInput = inputManager.GetPlayerInput();
            _textBox = GetComponent<TMP_Text>();

            inputManager.ActiveDeviceChangeEvent += SetText;
            inputManager.BindingsChangedEvent += SetText;
        }

        private void OnDestroy()
        {
            inputManager.ActiveDeviceChangeEvent -= SetText;
            inputManager.BindingsChangedEvent -= SetText;
        }

        private void Start()
        {
            SetText();
        }

        [ContextMenu("Set Text")]
        private void SetText()
        {
            // safe guard against devices not in our sprite sheet
            if ((int)deviceType > listOfTmpSpriteAssets.SpriteAssets.Count - 1)
            {
                Debug.LogFormat("Missing Sprite Asset for {0}", deviceType);
                return;
            }

            InputBinding oldBinding = _playerInput.Player.Interact.bindings[(int)deviceType];

            InputBinding dynamicBinding = inputManager.GetBinding("Player/Interact", deviceType);

            _textBox.text = CompleteTextWithButtonPromptSprite.ReplaceActiveBindings(message,
                inputManager, listOfTmpSpriteAssets);
            // _textBox.text = CompleteTextWithButtonPromptSprite.ReadAndReplaceBinding(
            //     message,
            //     dynamicBinding,
            //     listOfTmpSpriteAssets.SpriteAssets[(int)deviceType]);
            //
            // _textBox.text += CompleteTextWithButtonPromptSprite.GetSpriteTag("Player/Interact", deviceType,
            //     inputManager, listOfTmpSpriteAssets);
        }
    }
}