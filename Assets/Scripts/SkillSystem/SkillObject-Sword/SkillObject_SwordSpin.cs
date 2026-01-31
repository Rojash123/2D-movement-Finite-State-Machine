using UnityEngine;

public class SkillObject_SwordSpin : SkillObject_Sword
{
    private int maxDistance;
    private float attackPerSecond;
    private float attackTimer;
    private float maxDuration;

    public override void Update()
    {
        handleAttack();
        HandleStopping();
        HandleComeback();
    }
    public override void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        base.SetupSword(swordManager, direction);
        maxDistance= swordManager.maxDistance;
        anim?.SetTrigger("Spin");
        attackPerSecond = swordManager.attackPerSecondTimer;
        Invoke(nameof(GetSwordBacktoPlayer),swordManager.maxDuration);
    }

    private void HandleStopping()
    {
        float distanceToPlayer=Vector2.Distance(transform.position,playerTransform.position);
        if(distanceToPlayer > maxDistance && rb.simulated==true)
        {
            rb.simulated= false;
        }
    }

    private void handleAttack()
    {
        attackTimer -= Time.deltaTime;
        if(attackTimer < 0)
        {
            DamageEnemyInRadius(transform,1);
            attackTimer = 1 / attackPerSecond;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        rb.simulated = false;
    }
}
