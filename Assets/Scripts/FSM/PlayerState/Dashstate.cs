using UnityEngine;

public class Dashstate : PlayerState
{
    private float originalGravityScale;
    public Dashstate(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }

    public override void EnterState()
    {
        base.EnterState();
        stateTimer = player.dashDuration;
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }
    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.facingDir, 0);
        CancelDashIfNeeded();
        if (stateTimer < 0)
        {
            if (player.isGroundCheck)
            {
                fsm.ChangeState(player.idleState);
            }
            else
            {
                fsm.ChangeState(player.fallState);
            }
        }
    }
    public override void ExitState()
    {
        player.SetVelocity(0, 0);
        rb.gravityScale = originalGravityScale;
        base.ExitState();
    }
    void CancelDashIfNeeded()
    {
        if (player.isWallDetected)
        {
            if (player.isGroundCheck)
            {
                fsm.ChangeState(player.idleState);
            }
            else
            {
                fsm.ChangeState(player.wallSlideState);
            }
        }
    }
}
