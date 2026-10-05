using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }
    [SerializeField] private HpBar bar;
    [SerializeField] private GameObject gameOverPanel;
    private const int MaxHp = 20;
    private int hp = MaxHp;

    private void Awake() => Instance = this;

    public void TakeDamage(int amount)
    {
        //reduce player health by amount
        hp = Mathf.Max(0, hp - amount);
        bar.SetHp((float)hp / MaxHp);
        if (hp == 0)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}