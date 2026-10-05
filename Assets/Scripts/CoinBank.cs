using System.Collections;
using TMPro;
using UnityEngine;

public class CoinBank : MonoBehaviour
{
    public static CoinBank Instance { get; private set; }

    [Header("UI Objects")]
    [SerializeField] private RectTransform coinPrefab;   
    [SerializeField] private RectTransform canvasRoot;   
    [SerializeField] private RectTransform bankPanel;    
    [SerializeField] private TMP_Text label;

    [Header("Settings")]
    [SerializeField] private float flyTime = 0.6f;
    [SerializeField] private float punchScale = 0.2f;

    private int balance = 0;
    private Vector3 originalPanelScale;
    private Coroutine punchRoutine;

    private void Awake()
    {
        Instance = this;
        if (bankPanel != null) originalPanelScale = bankPanel.localScale;
    }

    public void SpawnCoin(Vector3 enemyWorldPosition)
    {
        if (coinPrefab == null || canvasRoot == null) return;

        // Spawn the coin inside the UI canvas
        RectTransform coin = Instantiate(coinPrefab, canvasRoot);
        coin.localScale = Vector3.one; 
        coin.position = Camera.main.WorldToScreenPoint(enemyWorldPosition);
        
        StartCoroutine(FlyToBank(coin));
    }

    private IEnumerator FlyToBank(RectTransform coin)
    {
        Vector3 startPos = coin.position;
        float elapsed = 0f;

        while (elapsed < flyTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flyTime;
            
            // Smoothly move the coin to the bank panel
            coin.position = Vector3.Lerp(startPos, bankPanel.position, t * t);
            yield return null;
        }

        Destroy(coin.gameObject);
        AddCoin();
    }

    private void AddCoin()
    {
        balance += 1;
        label.text = "Bank: " + balance;

        // animation
        if (punchRoutine != null) StopCoroutine(punchRoutine);
        punchRoutine = StartCoroutine(JuiceAnimation());
    }

    private IEnumerator JuiceAnimation()
    {
        float elapsed = 0f;
        float duration = 0.2f; // pop speed

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // smooth math curve for punch effect
            float scaleOffset = 1f + Mathf.Sin(t * Mathf.PI) * punchScale;
            bankPanel.localScale = originalPanelScale * scaleOffset;
            yield return null;
        }

        // Snap back to original scale 
        bankPanel.localScale = originalPanelScale;
    }
}
