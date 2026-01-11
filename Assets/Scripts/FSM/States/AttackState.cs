using UnityEditor.SceneTemplate;
using UnityEngine;

public class AttackState : PlayerState
{
    private bool attackComboQueued;

    private float attackVelocityTimer;
    private float lastTimeattack;

    private int comboIndex = 1;
    private const int comboStartIndex = 1;
    private int comboLimit = 3;
    private int attackDirection = 0;


    public AttackState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }
    public override void EnterState()
    {
        base.EnterState();
        attackComboQueued = false;
        ResetComboIfNeeded();

        attackDirection=player.moveVector.x!=0? (int)player.moveVector.x : player.facingDir;
        anim.SetInteger("basicattackindex", comboIndex);
        AttackVelocity();
    }
    public override void Update()
    {
        base.Update();
        handleVelocity();

        if (player.InputAction.Player.Attack.WasPerformedThisFrame())
            QueueComboAttack();

        if (triggerCalled)
        {
            if (attackComboQueued)
            {
                anim.SetBool(animBoolName, false);
                player.EnterAttackStateWithDelay();
            }
            else
            {
                fsm.ChangeState(player.idleState);
            }
        }
    }
    public override void ExitState()
    {
        lastTimeattack = Time.time;
        comboIndex++;
        base.ExitState();
    }

    private void QueueComboAttack()
    {
        if(comboIndex<comboLimit)
            attackComboQueued = true;
    }

    private void ResetComboIfNeeded()
    {
        if (comboIndex > comboLimit || Time.time > lastTimeattack + player.comboResetDuration)
            comboIndex = comboStartIndex;
    }
    private void handleVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocityY);
    }
    private void AttackVelocity()
    {
        attackVelocityTimer = player.attackDuration;
        player.SetVelocity(player.attackVelocity[comboIndex - 1].x * attackDirection, player.attackVelocity[comboIndex - 1].y);
    }
}
