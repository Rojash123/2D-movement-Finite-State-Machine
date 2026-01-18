using UnityEngine;

public abstract class PlayerState:EntityState
{
    protected Player player;
    public PlayerState(FiniteStateMachine finiteStateMachine,string stateName, Player player) :base (finiteStateMachine, stateName)
    {
        this.player = player;
        anim = player.animator;
        rb = player.rb;
        entityStats = player.entityStats;
    }
    public override void Update()
    {
        base.Update();
        if (player.InputAction.Player.Dash.WasPerformedThisFrame() && canDash())
        {
            fsm.ChangeState(player.dashState);
        }
    }
    public override void UpdateAnimationParameter()
    {
        base.UpdateAnimationParameter();
        anim.SetFloat("yVelocity", rb.linearVelocityY);
    }
    private bool canDash()
    {
        if (player.isWallDetected) return false;
        if (fsm.currentState == player.dashState) return false;
        return true;
    }
}
