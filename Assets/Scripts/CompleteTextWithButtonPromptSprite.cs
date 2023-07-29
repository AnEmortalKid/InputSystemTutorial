using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Christina.Typography
{
    public static class CompleteTextWithButtonPromptSprite
    {
        public static string ReadAndReplaceBnding(string textToDisplay, InputBinding actionNeeded,
            TMP_SpriteAsset spriteAsset)
        {
            //Different from christina's in that we are going to use the effective path
            // ToString previously would just be "Keyboard/f" yet
            // newer versions of input system have it "Keyboard/f[Keyboard]"
            string stringButtonName = actionNeeded.effectivePath;
            Debug.LogFormat("ActionNeeded: {0}", stringButtonName);
            stringButtonName = RenameInput(stringButtonName);

            textToDisplay = textToDisplay.Replace(
                "BUTTONPROMPT",
                $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">");

            return textToDisplay;
        }

        private static string RenameInput(string stringButtonName)
        {
            stringButtonName = stringButtonName.Replace("Interact:", String.Empty);

            stringButtonName = stringButtonName.Replace("<Keyboard>/", "Keyboard_");
            stringButtonName = stringButtonName.Replace("<Gamepad>/", "Gamepad_");

            return stringButtonName;
        }
    }
}