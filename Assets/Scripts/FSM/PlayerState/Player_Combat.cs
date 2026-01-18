using UnityEngine;

public class Player_Combat : Entity_Combat
{

    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecoveryDuration=0.1f;
    public bool CounterAttack()
    {
        bool hasCounteredSomeOne = false;
        foreach (var collider in GetDetectedColliders())
        {
            ICounterable counterable = collider.GetComponent<ICounterable>();

            if (counterable == null) continue;

            if(counterable.canBeCountered)
            {
                counterable?.HandleCounter();
                hasCounteredSomeOne = true;
            }
        }
        return hasCounteredSomeOne;
    }
    public float GetCounterRecoveryDuration() => counterRecoveryDuration;
}
