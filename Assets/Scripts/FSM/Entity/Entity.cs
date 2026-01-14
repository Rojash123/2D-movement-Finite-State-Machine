using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public FiniteStateMachine fsm { get; private set; }

    [SerializeField] protected float moveMentSpeed;
    [SerializeField] protected float jumpForce;


    [Range(0, 1)]
    [SerializeField] protected float airMultiplier;
    [Range(0, 1)]
    public float wallSlidemultiplier;

    [Header("Collission Detection")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;


    public bool isGroundCheck { get; private set; }
    public bool isWallDetected { get; private set; }



    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fsm = new();
    }
    public void CallAnimationTrigger()
    {
        fsm.currentState.CallAnimationTrigger();
    }

    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        HandleCollission();
        fsm.currentState.Update();
    }

    public virtual void MovePlayer()
    {
    }
    public virtual void MovePlayerAir()
    {
    }

    public void JumpPlayer()
    {
        SetVelocity(rb.linearVelocityX, jumpForce);
    }
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }
    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && !facingRight)
        {
            Flip();
        }
        else if (xVelocity < 0 && facingRight)
        {
            Flip();
        }
    }
    private void HandleCollission()
    {
        isGroundCheck = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

        if (secondaryWallCheck != null)
        {
            isWallDetected =
            Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
            && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround)
            ;
        }
        else
        {
            isWallDetected =
           Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }

    }
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0, -groundCheckDistance, 0));
    }
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = -facingDir;
    }
}
