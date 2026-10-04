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

        if (rangeLine == null) rangeLine = GetComponentInChildren<LineRenderer>();
        if (rangeLine != null)
        {
            rangeLine.startColor = rangeLine.endColor = rangeColor;
            rangeLine.widthMultiplier = 0.05f;
            rangeLine.loop = false;
            rangeLine.useWorldSpace = true;
        }

        SetupLineRenderer();
        DrawRange(); 
    }

    protected virtual void Update()
    {
        if (!isActive) return;
        UpdateBehavior();
    }

    public void StopFiring() => isActive = false;

        /// <summary>True if the creature is within range and inside the cone (flat XZ check).</summary>
    protected bool InCone(Creature c, float halfAngle)
    {
        Vector3 to = c.transform.position - transform.position;
        to.z= 0f;
        if (to.sqrMagnitude > range * range) return false;
        return Vector3.Angle(transform.right, to) <= halfAngle;
    }

/// <summary>Closest creature inside the cone, or null.</summary>
    protected Creature FindCreatureInCone(float halfAngle)
    {
        Creature best = null;
        float bestSqr = float.MaxValue;
        foreach (var c in Creature.All)
        {
            if (!InCone(c, halfAngle)) continue;
            float sqr = (c.transform.position - transform.position).sqrMagnitude;
            if (sqr < bestSqr) { best = c; bestSqr = sqr; }
        }
        return best;
    }

    protected void DrawWedge(float halfAngle)
{
    if (rangeLine == null) return;

    int segments = Mathf.Max(arcSegments, 2);
    rangeLine.positionCount = segments + 2;   // origin + arc points
    rangeLine.loop = true;                    // closes the pie slice

    rangeLine.SetPosition(0, transform.position);
    float start = -halfAngle;
    float step = (halfAngle * 2f) / segments;

    for (int i = 0; i <= segments; i++)
    {
        Vector3 dir = Quaternion.Euler(0f, 0f, start + step * i) * transform.right;
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
        float a = i * Mathf.PI * 2f / segments;
        Vector3 offset = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0f) * range;
        rangeLine.SetPosition(i, transform.position + offset);
    }
}
    // draws a straight outline for line of sight turrets
    protected void DrawSightLine()
    {
        if (rangeLine == null) return;
        rangeLine.SetPosition(0, transform.position);
        rangeLine.SetPosition(1, transform.position + transform.right * range);
    }
    protected abstract void UpdateBehavior();
    protected abstract void SetupLineRenderer();
    protected abstract void DrawRange();
}