using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerMovement;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public PlayerMovement playerMovement;

    public event Action<bool> OnPlayerJump;
    public event Action<bool> OnPlayerAttack;
    public event Action<bool> OnPlayerDash;
    public event Action<bool> OnPlayerSpellPressed;
    public event Action<Vector2> OnPlayerMove;
    private void OnEnable()
    {
        if (playerMovement == null)
        {
            playerMovement = new PlayerMovement();
            playerMovement.Player.AddCallbacks(this);
        }
        playerMovement.Enable();
    }
    private void OnDisable()
    {
        playerMovement.Disable();
    }

    public void DisableInput()
    {
        playerMovement.Disable();
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPlayerAttack?.Invoke(true);
            return;
        }
        OnPlayerAttack?.Invoke(false);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPlayerJump?.Invoke(true);
        }
        if (context.canceled)
        {
            OnPlayerJump?.Invoke(false);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        OnPlayerMove?.Invoke(context.ReadValue<Vector2>());
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPlayerDash?.Invoke(true);
            return;
        }
        OnPlayerDash?.Invoke(false);
        return;
    }

    public void OnCounter(InputAction.CallbackContext context)
    {
    }
    public void OnSpell(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnPlayerSpellPressed?.Invoke(true);
        }
        if (context.canceled)
        {
            OnPlayerSpellPressed?.Invoke(false);
        }
    }
}
