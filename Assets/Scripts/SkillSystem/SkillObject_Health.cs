using UnityEngine;

public class SkillObject_Health : Entity_Health
{
    private SkillObject_TimeEcho _timeEcho;

    protected override void Die()
    {
        _timeEcho = GetComponent<SkillObject_TimeEcho>();
        _timeEcho.HandleDeath();
    }
}
