using UnityEngine;

// for drawing  the path so it can be seen in game display
[RequireComponent(typeof(LineRenderer))]
public class PathDrawer : MonoBehaviour
{
    [SerializeField] private Transform[] points;       // same points the Spawner uses
    [SerializeField] private Color color = Color.cyan;
    [SerializeField] private int segments = 40;
    [SerializeField] private float width = 0.06f;

    private void Start()
    {
        var lr = GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = lr.endColor = color;
        lr.widthMultiplier = width;
        lr.useWorldSpace = true;
        lr.sortingOrder = -1;                          // behind creatures and turrets
        lr.positionCount = segments + 1;

        Vector3[] p = System.Array.ConvertAll(points, t => t.position);
        for (int i = 0; i <= segments; i++)
            lr.SetPosition(i, Bezier.Evaluate(p, i / (float)segments));
    }
}