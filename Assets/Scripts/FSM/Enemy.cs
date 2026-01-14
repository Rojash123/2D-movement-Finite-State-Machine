using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;

    [Header("BattleState")]
    public float battleMoveSpeed=3f;
    public float attackDistance =2f;
    public float battleTimeDuration=5f;
    public float retreatMinDistance = 1f;
    public Vector2 retreatVelocity;


    [Header("MovementDetails")]
    public float moveSpeed = 1f;
    public float idleTime = 2f;

    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float checkDistance;


    [Range(0, 2)]
    public float moveanimspeedmultiplier = 1f;

    protected override void Update()
    {
        base.Update();
    }

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit=
            Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, checkDistance, whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x+checkDistance,playerCheck.position.y));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + attackDistance, playerCheck.position.y));
    }
}
