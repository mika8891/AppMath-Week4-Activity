using UnityEngine;

public class LerpMove : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float timeToReachTarget= 3f;

    private float totalTime;
    private Vector3 startingPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) return;
        totalTime += Time.deltaTime;
        var t = Mathf.Clamp01(totalTime / timeToReachTarget);
        this.transform.position = Vector3.Lerp(startingPos, target.position, t * t * t);

    }
}
