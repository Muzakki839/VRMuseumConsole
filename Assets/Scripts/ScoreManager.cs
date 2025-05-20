using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text timeText;

    public float gameTime = 60f; // durasi game 60 detik
    private int score = 0;
    private float timer;

    void Start()
    {
        timer = gameTime;
        UpdateUI();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0) timer = 0;
            UpdateUI();
        }
    }

    public void Add10Points()
    {
        score += 10;
        UpdateUI();
    }

    public void Add50Points()
    {
        score += 50;
        UpdateUI();
    }

    public void Subtract15Points()
    {
        score -= 15;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        timeText.text = "Time: " + Mathf.CeilToInt(timer) + "s";
    }
}
