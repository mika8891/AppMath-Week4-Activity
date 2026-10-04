using UnityEngine;

public class SniperTurret : TurretBase
{
    [Header("Sniper Settings")]
    [SerializeField] private float sightTolerance = 3f;   // narrow cone = "line"
    [SerializeField] private float cooldown = 5.5f;
    [SerializeField] private GameObject bulletPrefab;          // changed from Projectile
    [SerializeField] private Transform muzzle;

    private float cooldownTimer;

    protected override void UpdateBehavior()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        Creature target = FindCreatureInCone(sightTolerance);
        if (target == null) return;

        Fire(target);
        cooldownTimer = cooldown;
    }

    private void Fire(Creature target)
    {
        if (bulletPrefab == null) return;
        Vector3 spawnPos = muzzle != null ? muzzle.position : transform.position;
        Vector3 dir = target.transform.position - spawnPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
    }

    protected override void SetupLineRenderer()
    {
        if (rangeLine == null) return;
        rangeLine.positionCount = 2;
    }

    protected override void DrawRange() => DrawSightLine();
}