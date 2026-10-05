using UnityEngine;
using System.Collections.Generic;

public class Creature : MonoBehaviour
{
    private Vector3[] points;
    private float duration, t;
    private bool dead;
    public static readonly List<Creature> All = new List<Creature>();
    private void OnEnable() => All.Add(this);
    private void OnDisable() => All.Remove(this);

    public void Init(Vector3[] pathPoints, float travelTime)
    {
        points = pathPoints;
        duration = travelTime;
    }

    private void Update()
    {
        // Move the creature along the Bezier curve 
        t += Time.deltaTime / duration;
        transform.position = Bezier.Evaluate(points, t);

        if (t >= 1f)
        {
            // when creature reaches the end of thhe path close to target, it will be destroyed and player will take damage
            PlayerHealth.Instance.TakeDamage(1);
            Destroy(gameObject);
        }
    }

    public void Kill()
    {
        // creature killed = coin spawn
        if (dead) return;              
        dead = true;
        CoinBank.Instance.SpawnCoin(transform.position);
        Destroy(gameObject);
    }
}
