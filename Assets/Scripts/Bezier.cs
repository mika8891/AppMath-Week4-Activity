using UnityEngine;

// bezier curve
public class Bezier
{
    public static Vector3 Evaluate(Vector3[] p, float t)
    {
        if (p.Length == 1) return p[0]; 
        var next = new Vector3[p.Length - 1]; // create a new array for the next level of points
        for (int i = 0; i < next.Length; i++) // iterate through the current level of points
            next[i] = Vector3.Lerp(p[i], p[i + 1], t); // interpolate between the current points to get the next level of points
        return Evaluate(next, t);
    }
}