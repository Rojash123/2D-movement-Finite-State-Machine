using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }
    public override void Update()
    {
        if (player.InputAction.Player.Attack.WasPerformedThisFrame())
        {
            fsm.ChangeState(player.jumpAttackState);
        }
        base.Update();
        if (player.moveVector.x != 0)
        {
            player.MovePlayerAir();
        }

    }
}
