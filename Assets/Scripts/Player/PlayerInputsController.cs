using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputsController : MonoBehaviour
{
    public event Action<Vector2> onMoveInput;
    public event Action<Vector2> onLookInput;
    public event Action onJumpInput;
    public event Action onShootInput;
    public event Action<bool> onAimInput;
    public event Action onPauseInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        onMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        onLookInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            onJumpInput?.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            onShootInput?.Invoke();
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed) onAimInput?.Invoke(true);
        if (context.canceled) onAimInput?.Invoke(false);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onPauseInput?.Invoke();
        }
    }
}