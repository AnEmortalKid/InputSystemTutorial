
using Christina.Typography;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// I didn't mention this one in the video, but this is a debug script I used based on a comment
/// to confirm that we are actually rebinding the input and not just changing the text.
///
/// There may be some confusion since i'm not actually saving the bindings , so they don't seem to persist
/// </summary>
public class ActionResponder : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    private void Awake()
    {
        PlayerInput.PlayerActions player = inputManager.GetPlayerInput().Player;
        player.Interact.performed += ActionLogger;
        player.Jump.performed += ActionLogger;
        player.Kick.performed += ActionLogger;
    }

    private void OnDisable()
    {
        PlayerInput.PlayerActions player = inputManager.GetPlayerInput().Player;
        player.Interact.performed -= ActionLogger;
        player.Jump.performed -= ActionLogger;
        player.Kick.performed -= ActionLogger;
    }

    private void ActionLogger(InputAction.CallbackContext cb)
    {
        Debug.LogFormat("Invoked {0}", cb.action.name);
    }
}