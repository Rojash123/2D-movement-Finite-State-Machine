using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect-SkillPoint", menuName = "Scriptable Objects/SkillPointEffect")]
public class ItemEffect_GrantSkillPoint : ItemEffectDataSO
{
    [SerializeField] private int pointToAdd;
    public override void ExecuteEffect()
    {
        UI ui=FindFirstObjectByType<UI>();
        ui.skillTree.AddSkillPoints(pointToAdd);
    }
}
