using UnityEngine;

public class PlayerAirState : Entity
{
    public PlayerAirState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }
    public override void Update()
    {
        base.Update();
        if (player.moveVector.x != 0)
        {
            player.MovePlayerAir();
        }
    }
}
