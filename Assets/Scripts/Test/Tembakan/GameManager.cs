using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score Data")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        // Singleton pattern aman
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple GameManager instances found!");
            Destroy(gameObject);
        }
    }

    public void AddScore(int value)
    {
        score += value;

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("Score Text is not assigned in GameManager!");
        }

        // Debug log buat cek perubahan skor
        Debug.Log("Score updated: +" + value + " → Total: " + score);
    }
}
