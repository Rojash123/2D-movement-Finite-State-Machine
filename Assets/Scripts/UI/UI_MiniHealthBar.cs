using UnityEngine;

public class UI_MiniHealthBar : MonoBehaviour
{
    private Entity entity=>GetComponentInParent<Entity>();

    private void OnEnable()
    {
        entity.OnFlip += HandleFlip;
    }
    private void OnDisable()
    {
        if (entity == null) return;
        entity.OnFlip -= HandleFlip;
    }
    private void HandleFlip()=> transform.rotation = Quaternion.identity;
}
