using UnityEngine;

public class SniperTurret : TurretBase
{
    [Header("Sniper Settings")]
    [SerializeField] private float sightTolerance = 3f;
    [SerializeField] private float cooldown = 5.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzle;

    private float cooldownTimer;

    protected override void UpdateBehavior()
    {
        cooldownTimer -= Time.deltaTime;

        Creature target = FindCreatureInCone(sightTolerance);
        if (target == null) return;

        // Aim toward the target
        AimAtTarget(target);

        if (cooldownTimer <= 0f)
        {
            Fire(target);
            cooldownTimer = cooldown;
        }
    }

    private void AimAtTarget(Creature target)
    {
        Vector3 targetDir = target.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Fire(Creature target)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning($"{name}: No bullet prefab assigned!", this);
            return;
        }

        Vector3 spawnPos = muzzle != null ? muzzle.position : transform.position;
        Vector3 dir = target.transform.position - spawnPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
    }

    protected override void SetupLineRenderer()
    {
        if (rangeLine == null) return;
        rangeLine.startColor = rangeColor;
        rangeLine.endColor = rangeColor;
    }

    protected override void DrawRange() => DrawSightLine();
}