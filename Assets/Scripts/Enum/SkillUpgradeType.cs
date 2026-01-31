using UnityEngine;

public enum SkillUpgradeType
{
    None,

    Dash,
    DashClone_OnStart,
    DashClone_OnStartAndArrival,
    DashShrad_OnStart,
    DashShrad_OnStartAndArrival,

    Shard,
    Shard_MoveToEnemy,
    Shard_MultiCast,
    Shard_Teleport,
    Shard_TeleportHealthRewind,

    SwordThrow,
    SwordThrow_Spin,
    SwordThrow_Pierce,
    SwordThrow_Bounce,

    TimeEcho,
    TimeEcho_SingleAttack,
    TimeEcho_MultiAttack,
    TimeEcho_ChanceToMultiply,

    TimeEcho_HealWisp,
    TimeEcho_CleanWisp,
    TimeEcho_CoolDown_Wisp,

    DomainSlowDown,
    Domain_EchoSpawn,
    Domain_ShardSpawn
}
