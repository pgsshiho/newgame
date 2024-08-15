using TMPro;
using UnityEngine;

public class Stage2Portal : MonoBehaviour
{
    public GameObject Stage2;
    public GameObject shop; // Shop ������Ʈ�� ���� �Ҵ�
    public TextMeshProUGUI timers;
    private Timer timer; // Timer �ν��Ͻ�
    private bool isPlayerInRange = false;
    private bool stageChanged = false;

    void Start()
    {
        // Stage2 ��Ȱ��ȭ, Shop Ȱ��ȭ
        Stage2.SetActive(false);

        if (shop != null)
        {
            shop.SetActive(true); // �ʱ⿡�� Shop Ȱ��ȭ
        }

        timer = FindObjectOfType<Timer>();
        if (timer != null)
        {
            timer.OnTimerEnd += OnTimerEnd; // Ÿ�̸� ���� �� �̺�Ʈ ���
        }
    }

    void Update()
    {
        if (isPlayerInRange && !stageChanged)
        {
            HandleStageTransition(); // ��Ż�� ���� Stage2�� �̵�
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            stageChanged = false;

            // ��Ż�� ������ Shop ��Ȱ��ȭ
            if (shop != null)
            {
                shop.SetActive(false);
            }

            // Stage2 Ȱ��ȭ ó��
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
            timer.ResetTimer(10f); // Ÿ�̸� ����
            timer.StartTimer(); // Ÿ�̸� ����
            timers.gameObject.SetActive(true);
        }

        TransitionToStage(Stage2);
    }

    private void TransitionToStage(GameObject nextStage)
    {
        Debug.Log("Activating Stage 2");
        nextStage.SetActive(true); // Stage 2 Ȱ��ȭ
        stageChanged = true; // �������� ���� �Ϸ�
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer ended, returning to shop.");

        Stage2.SetActive(false);

        if (shop != null)
        {
            shop.SetActive(true); // Shop Ȱ��ȭ
        }

        stageChanged = false;
    }
}
