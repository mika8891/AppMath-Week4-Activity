using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 18f, hitRadius = 0.6f, lifetime = 2f;

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;

        lifetime -= Time.deltaTime;
        if (lifetime <= 0f) { Destroy(gameObject); return; }

        foreach (var c in Creature.All)
        {
            Vector3 d = c.transform.position - transform.position;
            d.z = 0f;
            if (d.sqrMagnitude <= hitRadius * hitRadius)
            {
                c.Kill();
                Destroy(gameObject);
                return;
            }
        }
    }
}