using UnityEngine;

public class FallState : PlayerAirState
{
    public FallState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }
    public override void Update()
    {
        base.Update();
        if (player.isGroundCheck)
        {
            fsm.ChangeState(player.idleState);
        }
        if (player.isWallDetected)
        {
            fsm.ChangeState(player.wallSlideState);
        }
    }
}
