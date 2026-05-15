using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerInputsController : MonoBehaviour
{
    public event Action<Vector2> onMoveInput;
    public event Action<Vector2> onLookInput;

    public void OnMove(InputAction.CallbackContext context)
    {
        onMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        onLookInput?.Invoke(context.ReadValue<Vector2>());
    }
}