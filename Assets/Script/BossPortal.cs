using UnityEngine;

public class BossPortal : MonoBehaviour
{
    public GameObject Boss;
    public GameObject shop; // Shop ������Ʈ�� ���� �Ҵ�
    private bool isPlayerInRange = false;
    private bool stageChanged = false;

    void Start()
    {
        // Boss ��Ȱ��ȭ, Shop Ȱ��ȭ
        if (Boss != null)
        {
            Boss.SetActive(false);
        }
        else
        {
            Debug.LogError("Boss ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (shop != null)
        {
            shop.SetActive(true); // �ʱ⿡�� Shop Ȱ��ȭ
        }
        else
        {
            Debug.LogError("Shop ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    void Update()
    {
        if (isPlayerInRange && !stageChanged)
        {
            HandleStageTransition(); // ��Ż�� ���� Boss ���������� �̵�
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
                HandleStageTransition(); // �������� ��ȯ�� ��� ó��
            }
            else
            {
                Debug.LogWarning("Shop ������Ʈ�� null�Դϴ�.");
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
            Boss.SetActive(true); // ���� �������� Ȱ��ȭ
            stageChanged = true; // �������� ���� �Ϸ�
        }
        else
        {
            Debug.LogError("TransitionToBoss �Լ����� Boss ������Ʈ�� null�Դϴ�.");
        }
    }

    private void OnTimerEnd()
    {
        // �� �޼���� �� �̻� ������ �ʽ��ϴ�.
    }
}
