using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Christina.Typography
{
    public static class CompleteTextWithButtonPromptSprite
    {
        // capture multiple counts of {Player/Bar} and {Player/Jump}
        // matches lazily to support multiple phrases in one line
        private static string ACTION_PATTERN = @"\{(.*?)\}";
        private static Regex REGEX = new Regex(ACTION_PATTERN, RegexOptions.IgnoreCase);

        public static string ReadAndReplaceBinding(string textToDisplay, InputBinding actionNeeded,
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

        public static string ReplaceActiveBindings(string textWithActions, InputManager inputManager,
            ListOfTmpSpriteAssets spriteAssets)
        {
            return ReplaceBindings(textWithActions, inputManager.GetActiveDevice(), inputManager, spriteAssets);
        }

        public static string ReplaceBindings(string textWithActions, DeviceType deviceType, InputManager inputManager,
            ListOfTmpSpriteAssets spriteAssets)
        {
            MatchCollection matches = REGEX.Matches(textWithActions);

            // original template
            var replacedText = textWithActions;

            foreach (Match match in matches)
            {
                var withBraces = match.Groups[0].Captures[0].Value;
                var innerPart = match.Groups[1].Captures[0].Value;
                Debug.LogFormat("{0} has {1}", withBraces, innerPart);

                var tagText = GetSpriteTag(innerPart, deviceType, inputManager, spriteAssets);

                replacedText = replacedText.Replace(withBraces, tagText);
            }

            return replacedText;
        }

        /// <summary>
        /// Looks up the InputBinding based on device type and returns the TextMeshPro sprite tag
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="deviceType"></param>
        /// <param name="inputManager"></param>
        /// <param name="spriteAssets"></param>
        /// <returns></returns>
        public static string GetSpriteTag(string actionName, DeviceType deviceType, InputManager inputManager,
            ListOfTmpSpriteAssets spriteAssets)
        {
            InputBinding dynamicBinding = inputManager.GetBinding(actionName, deviceType);
            TMP_SpriteAsset spriteAsset = spriteAssets.SpriteAssets[(int)deviceType];

            Debug.LogFormat("Retrieving sprite tag for: {0} with path {1}", dynamicBinding.action,
                dynamicBinding.effectivePath);
            string stringButtonName = dynamicBinding.effectivePath;
            stringButtonName = RenameInput(stringButtonName);

            return $"<sprite=\"{spriteAsset.name}\" name=\"{stringButtonName}\">";
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