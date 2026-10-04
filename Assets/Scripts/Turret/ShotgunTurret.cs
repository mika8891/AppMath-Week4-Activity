using UnityEngine;

public class ShotgunTurret : TurretBase
{
    [Header("Shotgun Settings")]
    [SerializeField] private float coneHalfAngle = 40f;
    [SerializeField] private int pelletCount = 6;
    [SerializeField] private float spreadAngle = 30f;
    [SerializeField] private float cooldown = 4f;
    [SerializeField] private GameObject bulletPrefab;   
    [SerializeField] private Transform muzzle;

    private float cooldownTimer;

    protected override void UpdateBehavior()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        Creature target = FindCreatureInCone(coneHalfAngle);
        if (target == null) return;

        FireBlast(target);
        cooldownTimer = cooldown;
        
    }

    private void FireBlast(Creature target)
    {
        if (bulletPrefab == null) return;

        Vector3 spawnPos = muzzle != null ? muzzle.position : transform.position;
        Vector3 baseDir = target.transform.position - spawnPos;
        baseDir.z = 0f;
        baseDir.Normalize();

        float start = -spreadAngle * 0.5f;
        float step = pelletCount > 1 ? spreadAngle / (pelletCount - 1) : 0f;

        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 dir = Quaternion.Euler(0f, 0f, start + step * i) * baseDir;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
        }
    }
    protected override void SetupLineRenderer()
    {
        if (rangeLine == null) return;
        rangeLine.positionCount = arcSegments + 2;
        rangeLine.loop = true; 
    }

    protected override void DrawRange() => DrawCircle();
}