using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName, enemy)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        if (player == null)
            player = enemy.PlayerDetection().transform;

        if (shouldRetreat())
        {
            rb.linearVelocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }

    }
    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetection())
            UpdateBattleTimer();

        if (IsBattleTimeOver())
            fsm.ChangeState(enemy.idleState);

        if (WithInAttackRange() && enemy.PlayerDetection())
            fsm.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.battleMoveSpeed * DirectionToPlayer(), rb.linearVelocity.y);

    }
    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    private bool IsBattleTimeOver() => Time.time - lastTimeWasInBattle > enemy.battleTimeDuration;
    private bool WithInAttackRange() => DistancetoPlayer() < enemy.attackDistance;
    private bool shouldRetreat() => DistancetoPlayer() < enemy.retreatMinDistance;

    private float DistancetoPlayer()
    {
        if (player == null) return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }
    private int DirectionToPlayer()
    {
        if (player == null) return 0;

        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
