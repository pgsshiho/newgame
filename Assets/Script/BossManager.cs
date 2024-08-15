using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // TextMeshProUGUI�� ����ϱ� ���� �߰�
using UnityEngine.UI;  // Slider�� ����ϱ� ���� �߰�

public class BossManager : MonoBehaviour
{
    public BossEasy boss;  // BossEasy ��ũ��Ʈ ����
    public TextMeshProUGUI bossHealthText;  // ���� ü���� ǥ���� �ؽ�Ʈ
    public Slider bossHealthSlider;  // ���� ü���� ǥ���� �����̴�

    private Timer timer;  // Timer ��ũ��Ʈ ����

    void Start()
    {
        timer = FindObjectOfType<Timer>();
        if (timer == null)
        {
            Debug.LogError("Timer ��ũ��Ʈ�� ã�� �� �����ϴ�. ���� Timer�� �ִ��� Ȯ���ϼ���.");
        }
        else
        {
            // Ÿ�̸Ӱ� ����� �� ���� �̺�Ʈ ó�� ����
            timer.OnTimerEnd += OnTimerEnd;
        }

        if (bossHealthText == null)
        {
            Debug.LogError("bossHealthText�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (bossHealthSlider == null)
        {
            Debug.LogError("bossHealthSlider�� �Ҵ���� �ʾҽ��ϴ�.");
        }
        if (boss != null)
        {
            UpdateBossHealthUI();
        }
    }

    void Update()
    {
        // ������ ü���� ���������� UI�� �ݿ�
        if (boss != null)
        {
            UpdateBossHealthUI();
        }
    }

    // Ÿ�̸Ӱ� ����Ǿ��� �� ȣ��Ǵ� �޼���
    private void OnTimerEnd()
    {
        Debug.Log("Ÿ�̸Ӱ� ����Ǿ����ϴ�. ���� ���� �̺�Ʈ�� ó���� �� �ֽ��ϴ�.");
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

