using System;
using Unity.VisualScripting;
using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispSpeed = 5f;
    [SerializeField] private GameObject OnDeathVFX;
    [SerializeField] private LayerMask whatIsGround;

    private Transform playerTransform;
    private TrailRenderer wispTrail;
    private bool shoudldMoveToPlayer = false;

    private Entity_Health entityHealth;
    private Skill_TimeEcho skillManager;
    private Entity_StatusHandler statusHandler;
    private SkillObject_Health skillObjectHealth;

    public int maxAttack { get; private set; }

    private void Update()
    {
        if (shoudldMoveToPlayer)
        {
            HandleWispMovement();
        }
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocityY);
            StopHorizontalMovement();
        }
    }
    public void SetupEcho(Skill_TimeEcho skillManager)
    {
        this.skillManager = skillManager;

        stats = skillManager.player.entityStats;
        statusHandler=skillManager.player.statusHandler;
        damageScaleData = skillManager.damageScaleData;
        entityHealth = skillManager.player.playerHealth;

        maxAttack = skillManager.GetMaxAttack();
        anim.SetBool("canAttack", maxAttack > 0);
        playerTransform = skillManager.transform.root;

        FLipRotation();

        skillObjectHealth = GetComponent<SkillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        Invoke(nameof(HandleDeath), skillManager.GetEchoTimeDuration());
    }
    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            rb.linearVelocity.Set(0, rb.linearVelocityY);
    }
    public void HandleDeath()
    {
        Instantiate(OnDeathVFX, transform.position, Quaternion.identity);

        if (skillManager.ShouldCreateWisp())
        {
            TurnIntoWhisp();
        }
        else
            Destroy(gameObject);
    }

    private void TurnIntoWhisp()
    {
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        shoudldMoveToPlayer = true;
        rb.simulated = false;
    }

    private void HandleWispMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, wispSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, playerTransform.position) < 0.75f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }
    private void HandlePlayerTouch()
    {
        float healAmount=skillObjectHealth.lastDamageTaken*skillManager.GetPercentageOfDamageHealted();
        entityHealth.IncreaseHealth(healAmount);

        float coolDownReduce=skillManager.GetCoolDownReduction();
        skillManager.ReduceCoolDownBY(coolDownReduce);

        if (skillManager.CanReduceNegativeEffect())
        {
            statusHandler.RemoveAllNegativeEffects();
        }

    }
    private void FLipRotation()
    {
        Transform target = ClosestTarget();
        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void PerformAttacK()
    {
        DamageEnemyInRadius(targetCheck, 3);

        if (targetGotHit == false)
            return;

        bool canDuplicate = UnityEngine.Random.value < skillManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -11;

        if (canDuplicate)
            skillManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }
}
