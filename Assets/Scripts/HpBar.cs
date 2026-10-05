using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class HpBar : MonoBehaviour
{
    [SerializeField] private Image realFill, ghostFill; 
    [SerializeField] private float holdTime = 0.5f, easeTime = 0.6f;
    private Coroutine routine;

    public void SetHp(float normalized)
    {
        realFill.fillAmount = normalized;         // set the real fill immediately        
        if (routine != null) StopCoroutine(routine); 
        routine = StartCoroutine(GhostDrain(normalized)); // start the ghost fill drain
    }

    private IEnumerator GhostDrain(float target)
    {
        yield return new WaitForSeconds(holdTime);        // ghost fill
        float start = ghostFill.fillAmount;
        for (float t = 0f; t < 1f; t += Time.deltaTime / easeTime)
        {
            float eased = 1f - Mathf.Pow(1f - t, 3f);     // ease-out cubic
            ghostFill.fillAmount = Mathf.Lerp(start, target, eased);
            yield return null;
        }
        ghostFill.fillAmount = target;
    }
}