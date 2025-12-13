using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    public float stateTimer;
    public float stateDuration = 3;

    private void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public void SetTimer()
    {
        stateTimer = stateDuration;
    }
}
