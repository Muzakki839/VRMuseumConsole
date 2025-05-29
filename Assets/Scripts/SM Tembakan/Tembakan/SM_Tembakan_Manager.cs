using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SM_Tembakan_Manager : MonoBehaviour
{
    public static SM_Tembakan_Manager Instance;

    [Header("UI References")]
    public GameObject UI_StandFront;           // UI1 - depan stand
    public TMP_Text textIntro;                 // Informasi awal
    public Button buttonPlay;                  // Tombol play
    public TMP_Text textResult;                // Hasil akhir
    public TMP_Text textScoreGameplay;         // Skor saat main

    public GameObject UI_StageFront;           // UI2 - depan stage (countdown 321)
    public TMP_Text textCountdown321;

    public GameObject UI_StageTop;             // UI3 - atas stage (timer)
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

    [Header("Audio Clips")]
    public AudioClip audioCountdownTick;    // Suara countdown tiap detik 3..2..1
    public AudioClip audioWin;               // Suara menang
    public AudioClip audioLose;              // Suara kalah
    // Background music saat main
    [SerializeField] private AudioSource audioSource;

    private float timer;
    private int score = 0;
    private bool gameActive = false;
    private Vector3 initialGunPosition;
    private Quaternion initialGunRotation;
    private Coroutine gameLoopCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        audioSource = gameObject.AddComponent<AudioSource>();     // untuk efek suara singkat
        initialGunPosition = gunObject.transform.position;
        initialGunRotation = gunObject.transform.rotation;

        SetIdleState();
        buttonPlay.onClick.AddListener(StartGameSequence);
    }


    private void SetIdleState()
    {
        UI_StandFront.SetActive(true);
        textResult.gameObject.SetActive(false);
        UI_StageFront.SetActive(false);
        UI_StageTop.SetActive(false);
        gunObject.SetActive(false);
        spawnerObject.SetActive(false);

        textIntro.gameObject.SetActive(true);
        buttonPlay.gameObject.SetActive(true);
        textResult.text = "";
        textScoreGameplay.text = "";

        textIntro.text = $"Selamat datang!\nWaktu: {gameDuration / 60} menit\nTarget Skor: {targetScore}";
    }

    public void StartGameSequence()
    {
        StartCoroutine(GameStartRoutine());
    }

    private IEnumerator GameStartRoutine()
    {
        textIntro.gameObject.SetActive(false);
        buttonPlay.gameObject.SetActive(false);
        textResult.text = "";
        textScoreGameplay.gameObject.SetActive(true);
        UI_StageFront.SetActive(true);
        gunObject.transform.position = initialGunPosition;
        gunObject.transform.rotation = initialGunRotation;
        gunObject.SetActive(true);
        if (audioCountdownTick != null) audioSource.PlayOneShot(audioCountdownTick);
        for (int i = 3; i >= 1; i--)
        {
            textCountdown321.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        textCountdown321.text = "Go!";
        yield return new WaitForSeconds(0.5f);
        UI_StageFront.SetActive(false);

        StartGame();
    }


    private void StartGame()
    {
        score = 0;
        timer = gameDuration;
        gameActive = true;

        spawnerObject.SetActive(true);
        spawnerObject.GetComponent<TargetSpawner>().StartSpawning();
        UI_StageTop.SetActive(true);
        textScoreGameplay.text = $"Score: {score}";

        if (gameLoopCoroutine != null) StopCoroutine(gameLoopCoroutine);
        gameLoopCoroutine = StartCoroutine(GameLoop());
    }



    private IEnumerator GameLoop()
    {
        while (timer > 0 && gameActive)  // tambahkan cek gameActive juga
        {
            timer -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            textGameTimer.text = $"{minutes:00}:{seconds:00}";
            textScoreGameplay.text = $"Score: {score}";
            yield return null;
        }

        if (gameActive)  // pastikan hanya dipanggil sekali
            EndGame();
    }

    private void EndGame()
    {
        if (!gameActive) return;  // Tambahkan guard supaya EndGame tidak jalan berkali-kali
        gameActive = false;
        textScoreGameplay.gameObject.SetActive(false);
        textResult.gameObject.SetActive(true);
        spawnerObject.GetComponent<TargetSpawner>().StopSpawning();
        spawnerObject.SetActive(false);
        UI_StageTop.SetActive(false);
        gunObject.SetActive(false);

        bool success = score >= targetScore;
        textResult.text = success
            ? $" Congratulations!\nScore: {score}"
            : $" Try Again\nScore: {score}";

        if (success)
        {
            if (audioWin != null)
            {
                audioSource.PlayOneShot(audioWin);
            }

            if (rewardPrefab != null && rewardSpawnPoint != null)
            {
                Instantiate(rewardPrefab, rewardSpawnPoint.position, rewardSpawnPoint.rotation);
            }
        }
        else
        {
            if (audioLose != null)
            {
                audioSource.PlayOneShot(audioLose);
            }
        }

        StartCoroutine(ResetAfterDelay());
    }



    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SetIdleState();
    }

    public void AddScore(int amount)
    {
        if (!gameActive) return;
        score += amount;
    }

    public int GetScore() => score;
}
