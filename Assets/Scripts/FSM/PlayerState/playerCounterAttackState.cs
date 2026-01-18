using UnityEngine;

public class playerCounterAttackState : PlayerState
{
    Player_Combat player_Combat;
    bool counteredSomebody;
    public playerCounterAttackState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        player_Combat = player.GetComponent<Player_Combat>();
    }
    public override void EnterState()
    {
        base.EnterState();
        counteredSomebody = player_Combat.CounterAttack();
        anim.SetBool("counterAttackPerformed", counteredSomebody);
        stateTimer = player_Combat.GetCounterRecoveryDuration();
    }
    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            fsm.ChangeState(player.idleState);

        if (stateTimer < 0 && counteredSomebody == false)
            fsm.ChangeState(player.idleState);
    }
}
