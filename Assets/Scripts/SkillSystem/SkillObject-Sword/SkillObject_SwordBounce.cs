using System.Collections.Generic;
using UnityEngine;

public class SkillObject_SwordBounce : SkillObject_Sword
{
    private float bounceSpeed;
    private int bounceCount;

    private Collider2D[] enemyTargets;
    private Transform nextTarget;
    private List<Transform> selectedBefore = new List<Transform>();

    public override void Update()
    {
        HandleBounce();
        HandleComeback();
    }

    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);
        anim?.SetTrigger("Spin");
        bounceSpeed = swordManager.bounceSpeed;
        bounceCount= swordManager.bounceCount;
    }

    private void HandleBounce()
    {
        if (nextTarget == null)
            return;

        transform.position=Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed*Time.deltaTime);
        if (Vector2.Distance(transform.position, nextTarget.position) < 0.75f)
        {
            DamageEnemyInRadius(transform, 1);
            BounceToNextTarget();
            if (bounceCount == 0 || nextTarget==null)
            {
                nextTarget = null;
                GetSwordBacktoPlayer();
            }
        }

    }
    private void BounceToNextTarget()
    {
        nextTarget = GetNextTarget();
        bounceCount--;
    }
    private Transform GetNextTarget()
    {
        List<Transform> ValidTargets = GetValidTargets();
        Transform nextTarget = ValidTargets[Random.Range(0, ValidTargets.Count)];
        selectedBefore.Add(nextTarget);
        return nextTarget;
    }
    private List<Transform> GetValidTargets()
    {
        List<Transform> validTargets = new List<Transform>();
        List<Transform> aliveTargets = GetAliveTargets();

        foreach (Transform target in aliveTargets)
        {
            if (target != null && !selectedBefore.Contains(target))
                validTargets.Add(target);
        }
        if (validTargets.Count > 0)
        {
            return validTargets;
        }
        else
        {
            selectedBefore.Clear();
            return aliveTargets;
        }
    }
    private List<Transform> GetAliveTargets()
    {
        List<Transform> targets = new List<Transform>();
        foreach (var enemy in enemyTargets)
        {
            if (enemy != null)
                targets.Add(enemy.transform);
        }
        return targets;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyTargets == null)
        {
            enemyTargets = EnemiesAround(transform, 10f);
            rb.simulated = false;
        }
        DamageEnemyInRadius(transform, 1);

        if (enemyTargets.Length <= 1 || bounceCount == 0)
            GetSwordBacktoPlayer();
        else
            nextTarget=GetNextTarget();

    }
}
