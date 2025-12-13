using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] InputReader inputReader;

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
    #endregion


    public bool isJump;
    public bool iSAttack;
    public bool isDash;
    public Vector2 moveVector;

    private void OnDrawGizmos()
    {
        
    }

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


        inputReader.OnPlayerMove += HandleMove;
        inputReader.OnPlayerJump += HandleJump;
        inputReader.OnPlayerAttack += HandleAttack;
        inputReader.OnPlayerDash += HandleDash;
    }

    private void HandleDash(bool isDash)
    {
        this.isDash = isDash;
    }

    private void Start()
    {
        fsm.InititalizeMethod(idleState);
    }
    private void Update()
    {
        HandleCollission();
        fsm.currentState.Update();
    }
    private void OnDestroy()
    {
        inputReader.OnPlayerMove -= HandleMove;
        inputReader.OnPlayerJump -= HandleJump;
        inputReader.OnPlayerAttack -= HandleAttack;
    }
    private void HandleAttack(bool attack)
    {
        iSAttack = attack;
    }
    private void HandleJump(bool jump)
    {
        isJump = jump;
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
        Debug.Log($"{xVelocity}:{yVelocity}");
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
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right*facingDir, wallCheckDistance, whatIsGround);
    }
    public void Flip()
    {

        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = -facingDir;
    }
}
