using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Timer : MonoBehaviour
{
    public static Timer Instance; // 싱글턴 인스턴스

    public TextMeshProUGUI timerText;
    public GameObject[] stages; // Stage1, Stage2, Stage3 오브젝트를 관리
    public GameObject[] shops;
    public TextMeshProUGUI enemyCountText;
    public Transform playerTransform;
    public GameObject boss;

    private int currentStageIndex = 0;
    private int currentShopIndex = 0;
    private float timeRemaining = 10f;
    private bool timerEnded = false;
    private bool isPaused = true; // 기본적으로 타이머가 일시정지 상태로 시작

    public Action OnTimerEnd;

    public bool TimerEnded
    {
        get { return timerEnded; }
    }

    void Awake()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 게임 시작 시 타이머 자동 시작
        StartTimer();  // 타이머를 게임 시작과 동시에 흐르게 함
    }

    void OnEnable()
    {
        // Stage1, Stage2, Stage3 중 하나가 활성화되었는지 확인하고 타이머 초기화 및 시작
        foreach (GameObject stage in stages)
        {
            if (stage != null && stage.activeSelf)
            {
                ResetTimer(10f); // 타이머를 초기화하고 시작
                break; // 하나라도 활성화되면 타이머를 초기화하고 종료
            }
        }
    }

    void OnDestroy()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        // 타이머가 일시정지 상태가 아니고 보스가 활성화되어 있지 않으면 타이머 갱신
        if (!isPaused && (boss == null || !boss.activeSelf))
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    timerEnded = true;

                    timerText?.gameObject.SetActive(false);

                    OnTimerEnd?.Invoke();

                    DestroyAllEnemies();
                    MovePlayerToOrigin();
                    GoToShop();

                    PauseTimer(); // 타이머 멈춤
                }

                if (timerText != null)
                {
                    UpdateTimerDisplay();
                }
            }
        }
    }

    public void ResetTimer(float newTime)
    {
        timeRemaining = newTime;
        timerEnded = false;
        StartTimer(); // 타이머를 리셋한 후 자동으로 시작되도록 함
    }

    public void StartTimer()
    {
        isPaused = false; // 타이머 재개
        UpdateTimerDisplay(); // 타이머가 시작될 때 즉시 화면에 반영
    }

    public void PauseTimer()
    {
        isPaused = true; // 타이머 일시정지
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is not assigned!");
        }
    }

    private void GoToShop()
    {
        if (currentStageIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(false);
        }

        currentStageIndex++;
        if (currentStageIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(true);
        }
        else
        {
            Debug.LogError("No more stages left to activate.");
        }

        foreach (var shop in shops)
        {
            shop.SetActive(false);
        }

        if (currentShopIndex < shops.Length)
        {
            shops[currentShopIndex].SetActive(true);
            currentShopIndex++;
        }
        else
        {
            Debug.LogError("No more shops left to activate.");
        }
    }

    private void DestroyAllEnemies()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.layer == enemyLayer)
            {
                Destroy(obj);
            }
        }

        if (enemyCountText != null)
        {
            enemyCountText.text = "0";
        }
    }

    private void MovePlayerToOrigin()
    {
        if (playerTransform != null)
        {
            playerTransform.position = new Vector2(0, 0);
        }
        else
        {
            Debug.LogError("Player Transform is not assigned!");
        }
    }

    // 씬이 로드될 때 호출되는 메서드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 이름이 Easy, Normal, Hard 중 하나일 때 타이머를 초기화하고 시작
        if (scene.name == "Easy" || scene.name == "Normal" || scene.name == "Hard")
        {
            ResetTimer(10f); // 타이머를 초기화하고 바로 시작
        }
    }
}
