using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SM_LemparBola_Manager : MonoBehaviour
{
    public static SM_LemparBola_Manager Instance;

    [Header("UI References")]
    public GameObject UI_StandFront; // UI1 - depan stand
    public TMP_Text textIntro;
    public Button buttonPlay;
    public TMP_Text textResult;
    public TMP_Text textScoreGameplay;

    public GameObject UI_StageFront; // UI2 - countdown 3..2..1
    public TMP_Text textCountdown321;

    public GameObject UI_StageTop; // UI3 - timer gameplay
    public TMP_Text textGameTimer;

    [Header("Game Objects")]
    public GameObject[] cans; // Semua kaleng
    public GameObject ballSpawner;
    public GameObject ball; // Referensi ke bola

    private Vector3 initialBallPosition;
    private Quaternion initialBallRotation;

    [Header("Reward Settings")]
    public Transform rewardSpawnPoint;
    public GameObject rewardPrefab;

    [Header("Game Settings")]
    public float gameDuration = 10f;

    [Header("Audio Clips")]
    public AudioClip audioCountdownTick;
    public AudioClip audioWin;
    public AudioClip audioLose;

    [SerializeField] private AudioSource audioSource;

    private float timer;
    private int score = 0;
    private int targetScore;
    private bool gameActive = false;
    private Coroutine gameLoopCoroutine;

    private Vector3[] initialCanPositions;
    private Quaternion[] initialCanRotations;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (ball != null)
        {
            initialBallPosition = ball.transform.position;
            initialBallRotation = ball.transform.rotation;
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        initialCanPositions = new Vector3[cans.Length];
        initialCanRotations = new Quaternion[cans.Length];
        for (int i = 0; i < cans.Length; i++)
        {
            initialCanPositions[i] = cans[i].transform.position;
            initialCanRotations[i] = cans[i].transform.rotation;
        }

        targetScore = Mathf.CeilToInt(cans.Length * 0.8f);

        SetIdleState();
        buttonPlay.onClick.AddListener(StartGameSequence);
    }

    private void SetIdleState()
    {
        UI_StandFront.SetActive(true);
        UI_StageFront.SetActive(false);
        UI_StageTop.SetActive(false);
        textResult.gameObject.SetActive(false);
        textIntro.gameObject.SetActive(true);
        textScoreGameplay.gameObject.SetActive(false);

        ballSpawner.SetActive(false);

        textIntro.text = $"Lempar Bola ke Kaleng!\nTarget: {targetScore} poin\nWaktu: {gameDuration} detik";
        buttonPlay.gameObject.SetActive(true);
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
        ResetCans();

        score = 0;
        timer = gameDuration;
        gameActive = true;

        textScoreGameplay.text = $"Score: {score}";
        UI_StageTop.SetActive(true);
        ballSpawner.SetActive(true);

        if (gameLoopCoroutine != null) StopCoroutine(gameLoopCoroutine);
        gameLoopCoroutine = StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (timer > 0 && gameActive)
        {
            timer -= Time.deltaTime;
            int seconds = Mathf.CeilToInt(timer);
            textGameTimer.text = $"Time: {seconds}s";
            textScoreGameplay.text = $"Score: {score}";
            yield return null;
        }

        if (gameActive)
            EndGame();
    }

    private void EndGame()
    {
        if (!gameActive) return;
        gameActive = false;

        UI_StageTop.SetActive(false);
        ballSpawner.SetActive(false);
        textScoreGameplay.gameObject.SetActive(false);
        textResult.gameObject.SetActive(true);

        bool success = score >= targetScore;
        textResult.text = success ? $"Menang!\nScore: {score}" : $"Gagal!\nScore: {score}";

        if (success)
        {
            if (audioWin != null) audioSource.PlayOneShot(audioWin);
            if (rewardPrefab != null && rewardSpawnPoint != null)
                Instantiate(rewardPrefab, rewardSpawnPoint.position, rewardSpawnPoint.rotation);
        }
        else
        {
            if (audioLose != null) audioSource.PlayOneShot(audioLose);
        }

        ResetCans();
        ResetBall();
        StartCoroutine(ResetAfterDelay());
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SetIdleState();
    }

    private void ResetCans()
    {
        for (int i = 0; i < cans.Length; i++)
        {
            cans[i].transform.position = initialCanPositions[i];
            cans[i].transform.rotation = initialCanRotations[i];

            Rigidbody rb = cans[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    private void ResetBall()
    {
        if (ball != null)
        {
            ball.transform.position = initialBallPosition;
            ball.transform.rotation = initialBallRotation;

            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    public void AddScore(int amount)
    {
        if (!gameActive) return;
        score += amount;
    }

    public int GetScore() => score;
}
