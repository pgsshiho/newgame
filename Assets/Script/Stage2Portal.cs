using TMPro;
using UnityEngine;

public class Stage2Portal : MonoBehaviour
{
    public GameObject Stage2;
    public GameObject shop; // Shop 오브젝트를 직접 할당
    public TextMeshProUGUI timers;
    private Timer timer; // Timer 인스턴스
    private bool isPlayerInRange = false;
    private bool stageChanged = false;

    void Start()
    {
        // Stage2 비활성화, Shop 활성화
        Stage2.SetActive(false);

        if (shop != null)
        {
            shop.SetActive(true); // 초기에는 Shop 활성화
        }

        timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.OnTimerEnd += OnTimerEnd; // 타이머 종료 시 이벤트 등록
        }
    }

    void Update()
    {
        if (isPlayerInRange && !stageChanged)
        {
            HandleStageTransition(); // 포탈을 통해 Stage2로 이동
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            stageChanged = false;

            // 포탈에 닿으면 Shop 비활성화
            if (shop != null)
            {
                shop.SetActive(false);
            }

            // Stage2 활성화 처리
            HandleStageTransition();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    private void HandleStageTransition()
    {
        if (timer != null)
        {
            timer.ResetTimer(10f); // 타이머 리셋
            timer.StartTimer(); // 타이머 시작
            timers.gameObject.SetActive(true);
        }

        TransitionToStage(Stage2);
    }

    private void TransitionToStage(GameObject nextStage)
    {
        Debug.Log("Activating Stage 2");
        nextStage.SetActive(true); // Stage 2 활성화
        stageChanged = true; // 스테이지 변경 완료
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer ended, returning to shop.");

        Stage2.SetActive(false);

        if (shop != null)
        {
            shop.SetActive(true); // Shop 활성화
        }

        stageChanged = false;
    }
}
