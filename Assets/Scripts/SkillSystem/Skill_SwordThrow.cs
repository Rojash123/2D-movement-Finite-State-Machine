using UnityEngine;

public class Skill_SwordThrow : Skill_Base
{
    private SkillObject_Sword currentSword;

    private float currentThrowPower;

    [Header("Regular Sword Upgrade")]
    [SerializeField] private GameObject swordPrefab;
    [Range(0, 10)]
    [SerializeField] private float throwPower = 5f;
    [SerializeField] private float regularThrowPower=5f;


    [Header("Pierce Sword Upgrade")]
    [SerializeField] private GameObject swordPiercePrefab;
    public int pierceAmount = 2;
    [SerializeField] private float pierceThrowPower = 5f;


    [Header("Spin Sword Upgrade")]
    [SerializeField] private GameObject spinSwordPrefab;
    public int maxDistance = 5;
    public float attackPerSecondTimer = 6;
    public float maxDuration = 6;
    [SerializeField] private float spinThrowPower = 5f;


    [Header("Bounce Sword Upgrade")]
    [SerializeField] private GameObject bounceSwordPrefab;
    public int bounceCount = 5;
    public float bounceSpeed = 12;
    [SerializeField] private float bounceThrowPower = 5f;


    [Header("Trajectory Prediction")]
    [SerializeField] private float swordGravity;
    [SerializeField] private GameObject predictionDots;
    [SerializeField] private int noOfDots = 20;
    [SerializeField] private float distanceBetweenDots = 0.05f;

    private Transform[] dots;
    private Vector2 confirmDirection;

    protected override void Awake()
    {
        base.Awake();
        swordGravity = swordPrefab.GetComponent<Rigidbody2D>().gravityScale;
        dots = GenerateDots();
    }
    public override bool CanUseSkills()
    {
        UpdateThrowPower();
        if (currentSword != null)
        {
            currentSword.GetSwordBacktoPlayer();
            return false;
        }

        return base.CanUseSkills();
    }
    public void PredictTrajectory(Vector2 direction)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].position = GetTrajectoryPoint(direction, i * distanceBetweenDots);
        }
    }
    private Vector2 GetTrajectoryPoint(Vector2 direction, float t)
    {
        float scaledThrowPower = currentThrowPower * 10;
        Vector2 initialVelocity = scaledThrowPower * direction;
        Vector2 gravityEffect = 0.5f * Physics2D.gravity * swordGravity * (t * t);

        Vector2 predictedPoint = initialVelocity * t + gravityEffect;
        Vector2 playerPosition = transform.root.position;

        return playerPosition + predictedPoint;
    }
    public void ThrowSword()
    {
        GameObject swordPrefab = GetSword();
        GameObject newSword = Instantiate(swordPrefab, dots[0].position, transform.rotation);
        currentSword = newSword.GetComponent<SkillObject_Sword>();
        currentSword.SetupSword(this, GetThrowPower());
    }

    private GameObject GetSword()
    {
        if (Unlocked(SkillUpgradeType.SwordThrow))
            return swordPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Pierce))
            return swordPiercePrefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Spin))
            return spinSwordPrefab;

        if (Unlocked(SkillUpgradeType.SwordThrow_Bounce))
            return bounceSwordPrefab;

        return null;
    }
    private void UpdateThrowPower()
    {
        if (Unlocked(SkillUpgradeType.SwordThrow))
           currentThrowPower= regularThrowPower;

        if (Unlocked(SkillUpgradeType.SwordThrow_Pierce))
            currentThrowPower = pierceThrowPower;


        if (Unlocked(SkillUpgradeType.SwordThrow_Spin))
            currentThrowPower = spinThrowPower;

        if (Unlocked(SkillUpgradeType.SwordThrow_Bounce))
            currentThrowPower = bounceThrowPower;
    }

    private Vector2 GetThrowPower() => currentThrowPower * confirmDirection * 10;
    public void ConfirmTrajectory(Vector2 direction) => confirmDirection = direction;
    public void EnableDots(bool enable)
    {
        foreach (Transform t in dots)
            t.gameObject.SetActive(enable);
    }
    private Transform[] GenerateDots()
    {
        Transform[] newDots = new Transform[noOfDots];
        for (int i = 0; i < noOfDots; i++)
        {
            newDots[i] = Instantiate(predictionDots, transform.position, Quaternion.identity).transform;
            newDots[i].gameObject.SetActive(false);
        }
        return newDots;
    }

}
