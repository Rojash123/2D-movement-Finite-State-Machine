using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D col;
    public Enemy_DeadState(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName, enemy)
    {
        col=enemy.GetComponent<Collider2D>();
    }
    public override void EnterState()
    {
        anim.enabled = false;
        rb.gravityScale = 12;
        rb.linearVelocity=new(rb.linearVelocity.x,15f);
        col.enabled = false;

        fsm.SwitchOffStateMachine();
    }
}
