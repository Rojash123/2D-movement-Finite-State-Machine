using System;
using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    [Header("Player Input")]
    [SerializeField] InputReader inputReader;
    public PlayerMovement InputAction;

    [Header("Attack Details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackDuration = 0.1f;
    public float comboResetDuration = 1f;
    private Coroutine attackQueuedCoRoutine;

    [Header("Movement")]
    public Vector2 wallJumpDirection;
    public float dashDuration = 0.25f;
    public float dashSpeed = 20f;

    public Vector2 moveVector;

    #region Finite State Machine
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }
    public JumpState jumpState { get; private set; }
    public FallState fallState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public WallJumpState walljumpState { get; private set; }
    public Dashstate dashState { get; private set; }
    public AttackState attackState { get; private set; }
    public JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }


    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new(fsm, "idle", this);
        moveState = new(fsm, "move", this);
        jumpState = new(fsm, "jumpfall", this);
        fallState = new(fsm, "jumpfall", this);
        wallSlideState = new(fsm, "wallslide", this);
        walljumpState = new(fsm, "jumpfall", this);
        dashState = new(fsm, "dash", this);
        attackState = new(fsm, "basicattack", this);
        jumpAttackState = new(fsm, "playerjumpattack", this);
        deadState = new(fsm, "dead", this);
    }

    protected override void Start()
    {
        base.Start();
        fsm.InititalizeMethod(idleState);
        InputAction = inputReader.playerMovement;
    }
    private void OnEnable()
    {
        inputReader.OnPlayerMove += HandleMove;
    }
    private void OnDisable()
    {
        inputReader.OnPlayerMove -= HandleMove;
    }

    public void DisableInput()
    {
        inputReader.DisableInput();
        InputAction.Disable();
    }
    public override void EntityDeath()
    {
        base.EntityDeath();
        OnPlayerDeath?.Invoke();
        fsm.ChangeState(deadState);
    }

    public override void MovePlayer()
    {
        SetVelocity(moveVector.x * moveMentSpeed, rb.linearVelocityY);
    }
    public override void MovePlayerAir()
    {
        SetVelocity(moveVector.x * moveMentSpeed * airMultiplier, rb.linearVelocityY);
    }
    private void HandleMove(Vector2 move)
    {
        moveVector = move;
    }
    public void EnterAttackStateWithDelay()
    {
        if (attackQueuedCoRoutine != null)
            StopCoroutine(attackQueuedCoRoutine);

        attackQueuedCoRoutine = StartCoroutine(EnterAttackStateCoroutine());
    }

    private IEnumerator EnterAttackStateCoroutine()
    {
        yield return new WaitForEndOfFrame();
        fsm.ChangeState(attackState);
    }
}
