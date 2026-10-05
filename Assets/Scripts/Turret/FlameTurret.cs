using UnityEngine;

public class FlameTurret : TurretBase
{
    [Header("Flame Settings")]
    [SerializeField] private float coneHalfAngle = 15f;
    [SerializeField] private float fireInterval = 0.5f;
    [SerializeField] private float jitterAngle = 18f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Color activeColor = Color.yellow;

    private float fireTimer;

    protected override void UpdateBehavior()
    {
        fireTimer -= Time.deltaTime;

        Creature target = FindCreatureInCone(coneHalfAngle);
        bool hasTarget = target != null;

        if (hasTarget && fireTimer <= 0f)
        {
            Fire(target);
            fireTimer = fireInterval;
        }

        if (rangeLine != null)
        {
            Color currentColor = hasTarget ? activeColor : rangeColor;
            rangeLine.startColor = currentColor;
            rangeLine.endColor = currentColor;
        }
    }

    private void Fire(Creature target)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"{name}: No bullet prefab assigned!", this);
            return;
        }

        Vector3 spawnPos = muzzle != null ? muzzle.position : transform.position;
        Vector3 direction = target.transform.position - spawnPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + Random.Range(-jitterAngle, jitterAngle);
        
        Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
    }

    protected override void SetupLineRenderer()
    {
        if (rangeLine == null) return;
        rangeLine.startColor = rangeColor;
        rangeLine.endColor = rangeColor;
    }

    protected override void DrawRange() => DrawWedge(coneHalfAngle);
}