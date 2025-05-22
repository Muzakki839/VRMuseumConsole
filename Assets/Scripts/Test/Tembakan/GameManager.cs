using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public GameObject UI_Intro;                 // UI samping: intro, play, result
    public TMP_Text textIntro;                  // Penjelasan awal
    public Button buttonPlay;                   // Tombol play
    public TMP_Text textResult;                 // Congrats / Failed

    public GameObject UI_Countdown321;          // UI depan: countdown 3..2..1
    public TMP_Text textCountdown321;

    public GameObject UI_GameTimer;             // UI atas: gameplay timer
    public TMP_Text textGameTimer;

    [Header("Game Objects")]
    public GameObject gunObject;
    public GameObject spawnerObject;

    [Header("Reward Settings")]
    public Transform rewardSpawnPoint;
    public GameObject rewardPrefab;

    [Header("Game Settings")]
    public float gameDuration = 120f;
    public int targetScore = 300;

    private float timer;
    private int score = 0;
    private bool gameActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        UI_Intro.SetActive(true);
        UI_Countdown321.SetActive(false);
        UI_GameTimer.SetActive(false);
        gunObject.SetActive(false);
        spawnerObject.SetActive(false);
        textResult.text = "";

        buttonPlay.onClick.AddListener(StartGameSequence);
        textIntro.text = $"Selamat datang!\nWaktu: {gameDuration / 60} menit\nTarget Skor: {targetScore}";
    }

    public void StartGameSequence()
    {
        StartCoroutine(GameStartRoutine());
    }

    private IEnumerator GameStartRoutine()
    {
        UI_Intro.SetActive(false);
        UI_Countdown321.SetActive(true);
        gunObject.SetActive(true);

        // Countdown 3..2..1
        for (int i = 3; i >= 1; i--)
        {
            textCountdown321.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        textCountdown321.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        UI_Countdown321.SetActive(false);

        StartGame();
    }

    private void StartGame()
    {
        score = 0;
        timer = gameDuration;
        gameActive = true;

        spawnerObject.SetActive(true);
        UI_GameTimer.SetActive(true);

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            textGameTimer.text = $"{Mathf.CeilToInt(timer)}s";
            yield return null;
        }

        EndGame();
    }

    private void EndGame()
    {
        gameActive = false;
        spawnerObject.SetActive(false);
        UI_GameTimer.SetActive(false);

        bool success = score >= targetScore;

        textResult.text = success
            ? $"🎉 Congratulations!\nScore: {score}"
            : $"😢 Try Again\nScore: {score}";

        if (success)
        {
            Instantiate(rewardPrefab, rewardSpawnPoint.position, rewardSpawnPoint.rotation);
        }

        UI_Intro.SetActive(true);
        StartCoroutine(ResetAfterDelay());
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        textResult.text = "";
        textIntro.text = $"Selamat datang!\nWaktu: {gameDuration / 60} menit\nTarget Skor: {targetScore}";
    }

    public void AddScore(int amount)
    {
        if (!gameActive) return;
        score += amount;
    }

    public int GetScore() => score;
}
