using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // TextMeshProUGUI를 사용하기 위해 추가
using UnityEngine.UI;  // Slider를 사용하기 위해 추가

public class BossManager : MonoBehaviour
{
    public BossEasy boss;  // BossEasy 스크립트 참조
    public TextMeshProUGUI bossHealthText;  // 보스 체력을 표시할 텍스트
    public Slider bossHealthSlider;  // 보스 체력을 표시할 슬라이더

    private Timer timer;  // Timer 스크립트 참조

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer 스크립트를 찾을 수 없습니다. 씬에 Timer가 있는지 확인하세요.");
        }
        else
        {
            // 타이머가 종료될 때 관련 이벤트 처리 가능
            timer.OnTimerEnd += OnTimerEnd;
        }

        if (bossHealthText == null)
        {
            Debug.LogError("bossHealthText가 할당되지 않았습니다.");
        }
        if (bossHealthSlider == null)
        {
            Debug.LogError("bossHealthSlider가 할당되지 않았습니다.");
        }
        if (boss != null)
        {
            UpdateBossHealthUI();
        }
    }

    void Update()
    {
        // 보스의 체력을 지속적으로 UI에 반영
        if (boss != null)
        {
            UpdateBossHealthUI();
        }
    }

    // 타이머가 종료되었을 때 호출되는 메서드
    private void OnTimerEnd()
    {
        Debug.Log("타이머가 종료되었습니다. 보스 관련 이벤트를 처리할 수 있습니다.");
    }

    void OnDestroy()
    {
        if (timer != null)
        {
            timer.OnTimerEnd -= OnTimerEnd;
        }
    }

    public void UpdateBossHealthUI()
    {
        if (bossHealthText != null)
        {
            bossHealthText.text = $"{boss.curHealth}/{boss.maxHealth}";
        }

        if (bossHealthSlider != null)
        {
            bossHealthSlider.value = boss.curHealth / boss.maxHealth;
        }
    }
}

