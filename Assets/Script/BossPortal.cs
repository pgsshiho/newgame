using UnityEngine;

public class BossPortal : MonoBehaviour
{
    public GameObject Boss;
    public GameObject shop; // Shop 오브젝트를 직접 할당
    private bool isPlayerInRange = false;
    private bool stageChanged = false;

    void Start()
    {
        // Boss 비활성화, Shop 활성화
        if (Boss != null)
        {
            Boss.SetActive(false);
        }
        else
        {
            Debug.LogError("Boss 오브젝트가 할당되지 않았습니다.");
        }

        if (shop != null)
        {
            shop.SetActive(true); // 초기에는 Shop 활성화
        }
        else
        {
            Debug.LogError("Shop 오브젝트가 할당되지 않았습니다.");
        }
    }

    void Update()
    {
        if (isPlayerInRange && !stageChanged)
        {
            HandleStageTransition(); // 포탈을 통해 Boss 스테이지로 이동
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
                HandleStageTransition(); // 스테이지 전환을 즉시 처리
            }
            else
            {
                Debug.LogWarning("Shop 오브젝트가 null입니다.");
            }
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
        TransitionToBoss();
    }

    private void TransitionToBoss()
    {
        if (Boss != null)
        {
            Debug.Log("Activating Boss Stage");
            Boss.SetActive(true); // 보스 스테이지 활성화
            stageChanged = true; // 스테이지 변경 완료
        }
        else
        {
            Debug.LogError("TransitionToBoss 함수에서 Boss 오브젝트가 null입니다.");
        }
    }

    private void OnTimerEnd()
    {
        // 이 메서드는 더 이상 사용되지 않습니다.
    }
}
