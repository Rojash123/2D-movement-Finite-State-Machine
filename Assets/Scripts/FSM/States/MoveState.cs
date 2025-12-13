using UnityEngine;

public class MoveState : PlayerGroundedState
{
    public MoveState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }
    public override void Update()
    {
        base.Update();
        HandleIdle();
    }
    void HandleIdle()
    {
        if (player.moveVector.x == 0)
        {
            fsm.ChangeState(player.idleState);
        }
        else
        {
            player.MovePlayer();
        }
    }
}
