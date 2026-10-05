using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Creature creaturePrefab;
    [SerializeField] private Transform[] quadraticPoints; // SpawnA, PointA, Target
    [SerializeField] private Transform[] cubicPoints;     // SpawnB, PointB1, PointB2, Target
    [SerializeField] private float interval = 1.5f, travelTime = 8f;

    private IEnumerator Start()
    {
        bool useQuad = true;
        while (true)
        {
            var path = useQuad ? quadraticPoints : cubicPoints; // choose path
            var c = Instantiate(creaturePrefab, path[0].position, Quaternion.identity); // spawn creature
            c.Init(System.Array.ConvertAll(path, p => p.position), travelTime); 
            useQuad = !useQuad;
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnDrawGizmos()
    {
        DrawPath(quadraticPoints, Color.cyan);
        DrawPath(cubicPoints, Color.magenta);
    }

    private void DrawPath(Transform[] pts, Color col)
    {
        // Draw a bezier curve using Gizmos
        if (pts == null || pts.Length < 3) return;
        var v = System.Array.ConvertAll(pts, p => p.position);
        Gizmos.color = col;
        Vector3 prev = v[0];
        for (int i = 1; i <= 30; i++)
        {
            // Evaluate the bezier curve at t = i / 30
            Vector3 cur = Bezier.Evaluate(v, i / 30f);
            Gizmos.DrawLine(prev, cur);
            prev = cur;
        }
    }
}