using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {

    }
    public override void EnterState()
    {
        base.EnterState();
        player.DisableInput();
        rb.simulated = false;
    }
}
