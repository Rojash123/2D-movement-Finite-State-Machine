using UnityEngine;

public class WallJumpState : PlayerState
{
    public WallJumpState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }

    public override void EnterState()
    {
        base.EnterState();
        player.SetVelocity(player.wallJumpDirection.x * -player.facingDir,player.wallJumpDirection.y);
    }
    public override void Update()
    {
        base.Update();
        if (rb.linearVelocityY < 0)
        {
            fsm.ChangeState(player.fallState);
        }
        if (player.isWallDetected)
        {
            fsm.ChangeState(player.wallSlideState);
        }
    }
}
