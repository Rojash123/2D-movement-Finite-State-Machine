using UnityEngine;

public class Enemy_VFX : Entity_VFX
{
    [Header("Counter Attack Alert")]
    [SerializeField] private GameObject alert;

    public void HandleAttackAlert(bool isActive)=>alert.SetActive(isActive);
}
