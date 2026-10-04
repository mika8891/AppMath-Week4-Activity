using UnityEngine;

// bezier curve
public static class Bezier
{
    public static Vector3 Evaluate(Vector3[] p, float t)
    {
        if (p.Length == 1) return p[0];
        var next = new Vector3[p.Length - 1];
        for (int i = 0; i < next.Length; i++)
            next[i] = Vector3.Lerp(p[i], p[i + 1], t);
        return Evaluate(next, t);
    }
}