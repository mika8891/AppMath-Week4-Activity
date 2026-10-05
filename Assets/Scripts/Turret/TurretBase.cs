using UnityEngine;

public abstract class TurretBase : MonoBehaviour
{
    [SerializeField] protected float range = 8f;

    [Header("Range Visualization")]
    [SerializeField] protected LineRenderer rangeLine;
    [SerializeField] protected int arcSegments = 24;
    [SerializeField] protected Color rangeColor = Color.red;

    protected bool isActive = true;

    protected virtual void Awake()
    {
        if (rangeLine == null) 
            rangeLine = GetComponentInChildren<LineRenderer>();

        SetupLineRenderer();
        DrawRange(); 
    }

    protected virtual void Update()
    {
        if (!isActive) return;
        UpdateBehavior();
    }

    public void StopFiring() => isActive = false;

    protected bool TryGetCreatureInCone(Creature creature, float halfAngle, out float sqrDistance)
    {
        sqrDistance = float.MaxValue;

        Vector3 offset = creature.transform.position - transform.position;
        offset.z = 0f; 

        sqrDistance = offset.sqrMagnitude;
        if (sqrDistance > range * range) return false;

        return Vector3.Angle(transform.right, offset) <= halfAngle;
    }

    protected Creature FindCreatureInCone(float halfAngle)
    {
        Creature bestCreature = null;
        float closestSqrDist = float.MaxValue;

        var allCreatures = Creature.All;
        for (int i = 0; i < allCreatures.Count; i++)
        {
            Creature c = allCreatures[i];
            if (c == null) continue;

            if (TryGetCreatureInCone(c, halfAngle, out float sqrDist))
            {
                if (sqrDist < closestSqrDist)
                {
                    closestSqrDist = sqrDist;
                    bestCreature = c;
                }
            }
        }

        return bestCreature;
    }

    protected void DrawWedge(float halfAngle)
    {
        if (rangeLine == null) return;

        int segments = Mathf.Max(arcSegments, 2);
        rangeLine.positionCount = segments + 2; 
        rangeLine.loop = true; 

        rangeLine.SetPosition(0, transform.position);
        float startAngle = -halfAngle;
        float angleStep = (halfAngle * 2f) / segments;

        for (int i = 0; i <= segments; i++)
        {
            Vector3 dir = Quaternion.Euler(0f, 0f, startAngle + angleStep * i) * transform.right;
            rangeLine.SetPosition(i + 1, transform.position + dir * range);
        }
    }

    protected void DrawCircle()
    {
        if (rangeLine == null) return;

        int segments = Mathf.Max(arcSegments, 12);
        rangeLine.positionCount = segments;
        rangeLine.loop = true;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * range;
            rangeLine.SetPosition(i, transform.position + offset);
        }
    }

    protected void DrawSightLine()
    {
        if (rangeLine == null) return;
        rangeLine.positionCount = 2;
        rangeLine.SetPosition(0, transform.position);
        rangeLine.SetPosition(1, transform.position + transform.right * range);
    }

    protected abstract void UpdateBehavior();
    protected abstract void SetupLineRenderer();
    protected abstract void DrawRange();
}