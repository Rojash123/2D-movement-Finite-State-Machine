using System;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] InputReader inputHandler;

    [SerializeField] private float jumpForce=12;
    [SerializeField] private float movementSpeed=8;
    [SerializeField] AnimatorController playerAnimationController;


    private void Awake()
    {
        inputHandler.OnPlayerAttack += PlayerAttack;
        inputHandler.OnPlayerJump += PlayerJump;
        inputHandler.OnPlayerMove += PlayerMove;
    }
    private void OnDestroy()
    {
        inputHandler.OnPlayerAttack -= PlayerAttack;
        inputHandler.OnPlayerJump -= PlayerJump;
        inputHandler.OnPlayerMove -= PlayerMove;
    }

    private void PlayerMove(Vector2 direction)
    {

    }

    private void PlayerJump(bool isJump)
    {

    }

    private void PlayerAttack(bool isAttack)
    {

    }
}
