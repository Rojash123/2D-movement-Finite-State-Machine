using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect-Refund", menuName = "Scriptable Objects/RefundEffect")]
public class ItemEffect_Refund : ItemEffectDataSO
{
    public override void ExecuteEffect()
    {
        UI_SkillTree skilltree=FindAnyObjectByType<UI_SkillTree>(FindObjectsInactive.Include);
        skilltree.RefundAllSkills();
    }
}
