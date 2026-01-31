using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player_DomainExpansionState : PlayerState
{
    private Vector2 originalPosition;
    private float originalGravity;
    private float finalRiseDistance;

    private bool isLevitating;
    private bool createDomain;

    public Player_DomainExpansionState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        originalPosition = player.transform.position;
        originalGravity = rb.gravityScale;
        finalRiseDistance = GetAvailableRiseDistance();
        player.SetVelocity(0, player.riseSpeed);
        player.playerHealth.SetCanTakeDamage(false);
    }
    public override void Update()
    {
        base.Update();
        if (Vector2.Distance(originalPosition, player.transform.position) >= finalRiseDistance && !isLevitating)
        {
            Levitate();
        }

        if (isLevitating)
        {
            skillManager.domainExpansion.DoSpellCast();
            if (stateTimer <= 0)
            {
                isLevitating = false;
                rb.gravityScale = originalGravity;
                fsm.ChangeState(player.idleState);
            }
        }

    }

    public override void ExitState()
    {
        base.ExitState();
        createDomain = false;
        player.playerHealth.SetCanTakeDamage(true);

    }

    private void Levitate()
    {
        isLevitating = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        stateTimer = skillManager.domainExpansion.GetDomainDuration();

        if (!createDomain)
        {
            createDomain = true;
            skillManager.domainExpansion.CreateDomain();
        }

    }

    private float GetAvailableRiseDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.up, player.riseMaxDistance, player.whatIsGround);

        return hit.collider != null ? hit.distance - 1 : player.riseMaxDistance;
    }
}
