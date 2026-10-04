using UnityEngine;

public class FlameTurret : TurretBase
{
    [Header("Flame Settings")]
    [SerializeField] private float coneHalfAngle = 15f;
    [SerializeField] private float fireInterval = 0.5f;   // seconds between bullets
    [SerializeField] private float jitterAngle = 18f;     // random wobble in degrees
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Color activeColor = Color.yellow;

    private float timer;

    protected override void UpdateBehavior()
    {
        timer -= Time.deltaTime;

        Creature target = FindCreatureInCone(coneHalfAngle);
        bool burning = target != null;

        if (burning && timer <= 0f)
        {
            Fire(target);
            timer = fireInterval;
        }

        if (rangeLine != null)
        {
            Color col = burning ? activeColor : rangeColor;
            rangeLine.startColor = rangeLine.endColor = col;
        }
    }

    private void Fire(Creature target)
    {
        if (bulletPrefab == null) { Debug.LogWarning(name + ": no bullet prefab assigned"); return; }

        Vector3 spawnPos = muzzle != null ? muzzle.position : transform.position;
        Vector3 dir = target.transform.position - spawnPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + Random.Range(-jitterAngle, jitterAngle);
        Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0f, 0f, angle));
    }

    protected override void SetupLineRenderer()
    {
        if (rangeLine == null) return;
        rangeLine.positionCount = arcSegments + 2;
        rangeLine.loop = true; 
    }

    protected override void DrawRange() => DrawWedge(coneHalfAngle);
}