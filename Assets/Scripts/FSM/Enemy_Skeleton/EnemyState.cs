using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName)
    {
        this.enemy = enemy;
        anim = enemy.animator;
        rb = enemy.rb;
        entityStats= enemy.entityStats;
    }
    public override void UpdateAnimationParameter()
    {
        base.UpdateAnimationParameter();
        float battleSpeedMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;
        anim.SetFloat("moveanimspeedmultiplier", enemy.moveanimspeedmultiplier);
        anim.SetFloat("xvelocity", rb.linearVelocity.x);
        anim.SetFloat("battleanimspeedmultiplier", battleSpeedMultiplier);
    }
}
