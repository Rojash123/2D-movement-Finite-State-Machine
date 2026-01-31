using System;
using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private SkillObject_Shard currentShard;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonationTime = 2f;

    [Header("Moving Shard Details")]
    [SerializeField] private float shardSpeed = 7f;

    [Header("Multi Cast Details")]
    [SerializeField] private int maxCharge = 10;
    [SerializeField] private int currentCharge;
    private bool isRecharging;

    [Header("Multi Cast Details")]
    [SerializeField] private float shardExistDuration = 10f;

    [Header("TelePort and Rewind Details")]
    [SerializeField] private float savedHealthPercentage;

    protected override void Awake()
    {
        base.Awake();
        currentCharge = maxCharge;
    }
    public override void TryUSeSkill()
    {
        if (skillUpgradeType == SkillUpgradeType.None) return;

        if (!CanUseSkills()) return;

        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();

        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMove();

        if (Unlocked(SkillUpgradeType.Shard_MultiCast))
            HandleMultiCast();

        if (Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleTeleport();

        if (Unlocked(SkillUpgradeType.Shard_TeleportHealthRewind))
            HandleShardHealthRewind();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCoolDown();
    }
    private void HandleShardMove()
    {
        CreateShard();
        SetSkillOnCoolDown();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
    }
    private void HandleMultiCast()
    {
        if (currentCharge <= 0) return;

        CreateShard();
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharge--;
        if (!isRecharging) StartCoroutine(RechargeShard());
    }
    private void HandleTeleport()
    {
        if (currentShard == null)
        {
            CreateShard();
        }
        else
        {
            SwipePlayerAndShardPosition();
            SetSkillOnCoolDown();
        }
    }
    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard();
            savedHealthPercentage = player.playerHealth.GetHealthPercentage();
        }
        else
        {
            SwipePlayerAndShardPosition();
            player.playerHealth.SetHealthPercentage(savedHealthPercentage);
            SetSkillOnCoolDown();
        }
    }

    private void SwipePlayerAndShardPosition()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPos = player.transform.position;

        currentShard.transform.position = playerPos;
        player.TeleportPlayer(shardPosition);

        currentShard?.Explode();
    }
    private IEnumerator RechargeShard()
    {
        isRecharging = true;
        while (currentCharge < maxCharge)
        {
            currentCharge++;
            yield return new WaitForSeconds(coolDown);
        }
        isRecharging = false;
    }
    public void CreateShard()
    {
        float time = GetDetonationTime();
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);

        if (skillUpgradeType == SkillUpgradeType.Shard_Teleport || skillUpgradeType == SkillUpgradeType.Shard_TeleportHealthRewind)
            currentShard.OnShardExplode += ForceCoolDown;
    }
    public void CreateRawShard(Transform target = null, bool shardCanMove = false)
    {
        bool canMove = shardCanMove != false ? shardCanMove: Unlocked(SkillUpgradeType.Shard_MoveToEnemy)
            || Unlocked(SkillUpgradeType.Shard_MultiCast);

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonationTime, canMove, shardSpeed, target);
    }

    private void ForceCoolDown()
    {
        if (!OnCooldown())
        {
            SetSkillOnCoolDown();
            currentShard.OnShardExplode -= ForceCoolDown;
        }

    }
    public float GetDetonationTime()
    {
        if (skillUpgradeType == SkillUpgradeType.Shard_Teleport)
            return shardExistDuration;

        return detonationTime;
    }
}
