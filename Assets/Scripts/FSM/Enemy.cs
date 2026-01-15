using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_Stunned stunnedState;

    [Header("BattleState")]
    public float battleMoveSpeed = 3f;
    public float attackDistance = 2f;
    public float battleTimeDuration = 5f;
    public float retreatMinDistance = 1f;
    public Vector2 retreatVelocity;


    [Header("MovementDetails")]
    public float moveSpeed = 1f;
    public float idleTime = 2f;

    [Header("Stunned Details")]
    public Vector2 stunnedVelocity = new(7, 7);
    public float stunnedDuration = 1f;


    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float checkDistance;

    public Transform player { get; private set; }


    [Range(0, 2)]
    public float moveanimspeedmultiplier = 1f;

    public void TryEnterBattleState(Transform player)
    {
        this.player = player;

        if (fsm.currentState == battleState)
            return;

        if (fsm.currentState == attackState)
            return;

        fsm.ChangeState(battleState);
    }
    public override void EntityDeath()
    {
        base.EntityDeath();
        fsm.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        fsm.ChangeState(idleState);
    }

    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetection().transform;

        return player;
    }
    protected override void Update()
    {
        base.Update();
    }

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, checkDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + checkDistance, playerCheck.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + attackDistance, playerCheck.position.y));
    }
    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }
    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
