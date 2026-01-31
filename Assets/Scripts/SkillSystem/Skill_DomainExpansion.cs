using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Skill_DomainExpansion : Skill_Base
{
    [SerializeField] private GameObject domainPrefab;

    [Header("Slowdown Details")]
    public float slowDownPercentage = 0.9f;
    public float slowDownDuration = 5f;

    [Header("Shard Cast Details")]
    [SerializeField] private int shardToCast = 10;
    [SerializeField] private float shardCastDomainSlow = 1f;
    [SerializeField] private float shardCastDomainSlowDuration = 8f;
    private float spellCastTimer;
    private float spellPerSecond;


    [Header("Shard Cast Details")]
    [SerializeField] private int echoToCast = 10;
    [SerializeField] private float echoCastDomainSlow = 1f;
    [SerializeField] private float echoCastDomainSlowDuration = 8f;
    [SerializeField] private float healhToRestoreWithEcho = 0.05f;


    [Header("Domain Details")]
    public float maxSize = 10;
    public float expansionSpeed = 2f;


    private List<Enemy> trappedTarget = new List<Enemy>();
    private Transform currentTarget;

    public void DoSpellCast()
    {
        spellCastTimer -= Time.deltaTime;

        if (currentTarget == null)
            currentTarget = FindTargetInDomain();

        if (currentTarget != null && spellCastTimer < 0)
        {
            SpellCast(currentTarget);
            spellCastTimer = 1 / spellPerSecond;
            currentTarget = null;
        }
    }

    private void SpellCast(Transform target)
    {
        if (skillUpgradeType == SkillUpgradeType.Domain_EchoSpawn)
        {
            Vector3 offset = Random.value < 0.5f ? new Vector2(1, 0) : new Vector2(-1, 0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }

        if (skillUpgradeType == SkillUpgradeType.Domain_ShardSpawn)
        {
            skillManager.shard.CreateRawShard(currentTarget, true);
        }

    }
    private Transform FindTargetInDomain()
    {
        trappedTarget.RemoveAll(t => t == null || t.health.isDead);

        if (trappedTarget.Count == 0) return null;

        int randomIndex = Random.Range(0, trappedTarget.Count);
        return trappedTarget[randomIndex].transform ?? null;
    }

    public float GetDomainDuration()
    {
        if (skillUpgradeType == SkillUpgradeType.DomainSlowDown)
            return slowDownDuration;
        else if (skillUpgradeType == SkillUpgradeType.Domain_ShardSpawn)
            return shardCastDomainSlowDuration;
        else if (skillUpgradeType == SkillUpgradeType.Domain_EchoSpawn)
            return shardCastDomainSlowDuration;

        return 0;
    }
    public float GetSlowPercentage()
    {
        if (skillUpgradeType == SkillUpgradeType.DomainSlowDown)
            return slowDownPercentage;
        else if (skillUpgradeType == SkillUpgradeType.Domain_ShardSpawn)
            return shardCastDomainSlow;
        else if (skillUpgradeType == SkillUpgradeType.Domain_EchoSpawn)
            return echoCastDomainSlow;

        return 0;
    }

    private int GetSpellToCast()
    {
        if (skillUpgradeType == SkillUpgradeType.Domain_ShardSpawn)
            return shardToCast;
        else if (skillUpgradeType == SkillUpgradeType.Domain_EchoSpawn)
            return echoToCast;

        return 0;
    }
    public bool InstantDomain()
    {
        return skillUpgradeType != SkillUpgradeType.Domain_EchoSpawn
            && skillUpgradeType != SkillUpgradeType.Domain_ShardSpawn;
    }

    public void CreateDomain()
    {
        spellPerSecond = GetSpellToCast() / GetDomainDuration();
        GameObject domain = Instantiate(domainPrefab, transform.position, Quaternion.identity);
        domain.GetComponent<SkillObject_DomainExpansion>().SetupDomain(this);
    }

    public void AddTargets(Enemy target)
    {
        trappedTarget.Add(target);
    }
    public void clearTarget()
    {
        foreach (Enemy target in trappedTarget)
        {
            target.StopSlowDown();
        }

        trappedTarget = new List<Enemy>();
    }

}
