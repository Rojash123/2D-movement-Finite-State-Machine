using UnityEngine;

public class SkillObject_Sword : SkillObject_Base
{
    protected Skill_SwordThrow swordManager;

    protected Transform playerTransform;
    protected float comeBackSpeed = 20f;
    protected bool shouldComeback;
    protected float maxDistance = 25f;

    public virtual void Update()
    {
        transform.right = rb.linearVelocity;
        HandleComeback();
    }

    public void GetSwordBacktoPlayer() => shouldComeback = true;

    public virtual void SetupSword(Skill_SwordThrow swordManager, Vector2 direction)
    {
        rb.linearVelocity = direction;
        playerTransform = swordManager.transform.root;

        this.swordManager = swordManager;
        stats = swordManager.player.entityStats;
        damageScaleData = swordManager.damageScaleData;
    }

    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxDistance)
            GetSwordBacktoPlayer();

        if (!shouldComeback) return;

        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, comeBackSpeed * Time.deltaTime);

        if (Vector2.Distance(playerTransform.position, transform.position) < 0.5f)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSword(collision);
        DamageEnemyInRadius(transform, 1);
    }

    protected void StopSword(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;
    }
}
