using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] InputReader inputReader;
    public PlayerMovement InputAction;

    [Header("Attack Details")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackDuration=0.1f;
    public float comboResetDuration = 1f;
    private Coroutine attackQueuedCoRoutine;

    [Header("Movement")]
    [SerializeField] private float moveMentSpeed;
    public float jumpForce;
    public Vector2 wallJumpDirection;
    public float dashDuration = 0.25f;
    public float dashSpeed = 20f;

    [Range(0,1)]
    [SerializeField]private float airMultiplier;
    [Range(0, 1)]
    public float wallSlidemultiplier;

    [Header("Collission Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;


    public bool isGrounded {  get; private set; }
    public bool isWallDetected { get; private set; }


    private bool facingRight=true;
    public int facingDir { get; private set; }=1;

    public Animator animator { get; private set; }
    public Rigidbody2D rb {  get; private set; }


    #region Finite State Machine
    public FiniteStateMachine fsm { get; private set;}
    public IdleState idleState { get; private set; }
    public MoveState moveState { get; private set; }
    public JumpState jumpState { get; private set; }
    public FallState fallState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public WallJumpState walljumpState { get; private set; }
    public Dashstate dashState { get; private set; }
    public AttackState attackState { get; private set; }
    public JumpAttackState jumpAttackState { get; private set; }

    #endregion

    public Vector2 moveVector;

    private void Awake()
    {
        animator=GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        fsm = new();
        idleState = new(fsm, "idle",this);
        moveState = new(fsm, "move",this);
        jumpState = new(fsm, "jumpfall", this);
        fallState = new(fsm, "jumpfall", this);
        wallSlideState = new(fsm,"wallslide",this);
        walljumpState = new(fsm, "jumpfall", this);
        dashState = new(fsm,"dash", this);
        attackState = new(fsm,"basicattack", this);
        jumpAttackState = new(fsm, "playerjumpattack", this);


        inputReader.OnPlayerMove += HandleMove;
    }

    public void CallAnimationTrigger()
    {
        fsm.currentState.CallAnimationTrigger();
    }

    public void EnterAttackStateWithDelay()
    {
        if (attackQueuedCoRoutine != null)
            StopCoroutine(attackQueuedCoRoutine);

        attackQueuedCoRoutine=StartCoroutine(EnterAttackStateCoroutine());
    }

    private IEnumerator EnterAttackStateCoroutine()
    {
        yield return new WaitForEndOfFrame();
        fsm.ChangeState(attackState);
    }
    private void Start()
    {
        fsm.InititalizeMethod(idleState);
        InputAction = inputReader.playerMovement;
    }
    private void Update()
    {
        HandleCollission();
        fsm.currentState.Update();
    }
    private void OnDestroy()
    {
        inputReader.OnPlayerMove -= HandleMove;
    }
    private void HandleMove(Vector2 move)
    {
        moveVector=move;
    }

    public void MovePlayer()
    {
        SetVelocity(moveVector.x * moveMentSpeed, rb.linearVelocityY);
    }
    public void MovePlayerAir()
    {
        SetVelocity(moveVector.x * moveMentSpeed*airMultiplier, rb.linearVelocityY);
    }

    public void JumpPlayer()
    {
        SetVelocity(rb.linearVelocityX,jumpForce);
    }
    public void SetVelocity(float xVelocity,float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }
    private void HandleFlip(float xVelocity)
    {
        if(xVelocity>0 && !facingRight)
        {
            Flip();
        }
        else if(xVelocity<0 && facingRight)
        {
            Flip();
        }
    }
    private void HandleCollission()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = 
            Physics2D.Raycast(primaryWallCheck.position, Vector2.right*facingDir, wallCheckDistance, whatIsGround)
            && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
            ;
    }
    public void Flip()
    {

        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = -facingDir;
    }
}
