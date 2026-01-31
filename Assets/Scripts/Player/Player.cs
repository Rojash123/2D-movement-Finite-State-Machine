using System;
using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Player : Entity
{
    public static event Action OnPlayerDeath;

    public Player_SkillManager skillManager { get; private set; }
    public Entity_Health playerHealth { get; private set; }
    public Player_VFX vfx { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }


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

    [Header("Special Ability")]
    public float riseSpeed = 25f;
    public float riseMaxDistance = 3;

    public Vector2 moveVector;
    public Vector2 mousePositionVector;

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
    public playerCounterAttackState counterAttackState { get; private set; }

    public Player_SwordThrowState swordThrowState { get; private set; }
    public Player_DomainExpansionState domainExpansionState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        statusHandler = GetComponent<Entity_StatusHandler>();
        skillManager = GetComponent<Player_SkillManager>();
        playerHealth = GetComponent<Entity_Health>();
        vfx = GetComponent<Player_VFX>();

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
        counterAttackState = new(fsm, "counterattack", this);
        swordThrowState = new(fsm, "Swordthrow", this);
        domainExpansionState=new(fsm,"jumpFall",this);
    }

    protected override void Start()
    {
        base.Start();
        fsm.InititalizeMethod(idleState);
        InputAction = inputReader.playerMovement;
        InputAction.Player.Spell.performed += ctx => skillManager.shard.TryUSeSkill();
        InputAction.Player.Spell.performed += ctx => skillManager.timeEcho.TryUSeSkill();
    }

    private void OnEnable()
    {
        inputReader.OnPlayerMove += HandleMove;
        inputReader.OnMouseMovePosition += SaveMousePosition;
    }

    private void SaveMousePosition(Vector2 position)
    {
        mousePositionVector = position;
    }

    private void OnDisable()
    {
        inputReader.OnPlayerMove -= HandleMove;
        inputReader.OnMouseMovePosition -= SaveMousePosition;
    }

    public void TeleportPlayer(Vector3 position) => transform.position = position;


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
    protected override IEnumerator SlowDownEntityCoroutine(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveMentSpeed;
        float originaljumpForce = jumpForce;
        float originalAnimSpeed = animator.speed;

        Vector2 originalWallJump = wallJumpDirection;
        Vector2 originalJumpAttackVelocity = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = new Vector2[attackVelocity.Length];
        Array.Copy(attackVelocity, originalAttackVelocity, attackVelocity.Length);

        float speedMultiplier = 1 - slowMultiplier;

        moveMentSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        animator.speed *= speedMultiplier;
        wallJumpDirection *= speedMultiplier;
        jumpAttackVelocity *= speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            originalAttackVelocity[i] *= speedMultiplier;
        }
        yield return new WaitForSeconds(duration);

        moveMentSpeed = originalMoveSpeed;
        jumpForce = originaljumpForce;
        animator.speed = originalAnimSpeed;
        wallJumpDirection = originalWallJump;
        jumpAttackVelocity = originalJumpAttackVelocity;
        attackVelocity = originalAttackVelocity;
        for (int i = 0; i < originalAttackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
            ;
        }
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
