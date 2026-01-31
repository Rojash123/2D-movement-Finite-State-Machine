using System.Collections.Generic;
using UnityEngine;

public class SkillObject_DomainExpansion : SkillObject_Base
{
    private Skill_DomainExpansion domainManager;

    private float expansionSpeed = 2f;
    private float slowDownPercentage = 0.9f;
    private float duration = 5f;

    private Vector3 targetScale;
    private bool isShrinking;


    public void SetupDomain(Skill_DomainExpansion skillManager)
    {
        domainManager = skillManager;

        duration=skillManager.GetDomainDuration();
        float maxSize = skillManager.maxSize;
        slowDownPercentage=skillManager.GetSlowPercentage();
        expansionSpeed = domainManager.expansionSpeed;

        targetScale = Vector3.one * maxSize;
        Invoke(nameof(ShrikDomain), duration);
    }

    private void Update()
    {
        HandleScaling();
    }

    private void HandleScaling()
    {
        float sizeDifference = Mathf.Abs(transform.localScale.x - targetScale.x);
        bool shouldChangeScale = sizeDifference > 0.1f;

        if (shouldChangeScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, expansionSpeed * Time.deltaTime);
        }
        if (isShrinking &&sizeDifference<0.1f)
        {
            TerminateDomain();
        }
    }

    private void TerminateDomain()
    {
        domainManager.clearTarget();
        Destroy(gameObject);
    }

    private void ShrikDomain()
    {
        isShrinking= true;
        targetScale = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null) return;

        domainManager.AddTargets(enemy);
        enemy.SlowDownPlayer(duration,slowDownPercentage,true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null) return;

        enemy.StopSlowDown();
    }
}
