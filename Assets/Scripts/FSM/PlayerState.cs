using UnityEngine;

public abstract class PlayerState:EntityState
{
    protected Player player;
    public PlayerState(FiniteStateMachine finiteStateMachine,string stateName, Player player) :base (finiteStateMachine, stateName)
    {
        this.player = player;
        anim = player.animator;
        rb = player.rb;
    }
    public override void Update()
    {
        base.Update();
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        if (player.InputAction.Player.Dash.WasPerformedThisFrame() && canDash())
        {
            fsm.ChangeState(player.dashState);
        }
    }
    private bool canDash()
    {
        if (player.isWallDetected) return false;
        if (fsm.currentState == player.dashState) return false;
        return true;
    }
}
