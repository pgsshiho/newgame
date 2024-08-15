using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject dialoguePanel;
    private bool isPlayerInRange = false;
    public GameObject shop;

    // static ������ �����Ͽ� ��� Shop ������Ʈ�� ������ ���� �����ϵ��� ��
    public static int meatCost = 50;
    public static int clothCost = 50;
    public static int swordCost = 50;
    public static int swordCount = 1;
    public static int clothCount = 10;

    public TextMeshProUGUI meatCostText;
    public TextMeshProUGUI clothCostText;
    public TextMeshProUGUI swordCostText;

    private StageManager stageManager;
    public TextMeshProUGUI warning;
    Player player;

    private void Awake()
    {
        stageManager = FindObjectOfType<StageManager>();
    }

    private void Start()
    {
        shop.SetActive(true);

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        UpdateCostText(); // ������ ��� �ؽ�Ʈ�� ������Ʈ
    }

    private void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�� �� �ؽ�Ʈ�� ��� ���� ����
        UpdateCostText();
    }

    private void UpdateCostText()
    {
        // �ؽ�Ʈ�� ����
        if (meatCostText != null)
        {
            meatCostText.text = meatCost.ToString();
        }

        if (clothCostText != null)
        {
            clothCostText.text = clothCost.ToString();
        }

        if (swordCostText != null)
        {
            swordCostText.text = swordCost.ToString();
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (dialoguePanel != null)
            {
                bool isActive = !dialoguePanel.activeSelf;
                dialoguePanel.SetActive(isActive);

                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.canMove = !isActive;
                }

                Cursor.visible = isActive;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (dialoguePanel != null && dialoguePanel.activeSelf)
            {
                dialoguePanel.SetActive(false);

                Player player = FindObjectOfType<Player>();
                if (player != null)
                {
                    player.canMove = true;
                }

                Cursor.visible = false;
            }
        }
    }

    public void PurchaseMeat()
    {
        Player player = FindObjectOfType<Player>();

        if (player != null && player.curHealth < player.maxHealth)
        {
            if (CanPurchase(meatCost))
            {
                stageManager.dia -= meatCost;
                stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
                meatCost += 50;

                UpdateCostText(); // ��� �ؽ�Ʈ ����
            }
        }
        else
        {
            ShowWarning("���� ü���� �̹� �ִ�ġ�Դϴ�.");
        }
    }

    public void PurchaseCloth()
    {
        if (CanPurchase(clothCost))
        {
            stageManager.dia -= clothCost;
            stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
            clothCost += 50;
            clothCount++;

            UpdateCostText(); // ��� �ؽ�Ʈ ����
        }
    }

    public void PurchaseSword()
    {
        if (CanPurchase(swordCost))
        {
            stageManager.dia -= swordCost;
            stageManager.UpdateDiaCount(); // ���̾Ƹ�� �� ������Ʈ
            swordCost += 50;
            swordCount++;
            UpdateCostText(); // ��� �ؽ�Ʈ ����
        }
    }

    private void ShowWarning(string message)
    {
        if (warning != null)
        {
            warning.text = message;
            warning.gameObject.SetActive(true);
            Invoke("HideWarning", 2f); // 2�� �Ŀ� ��� �޽����� ����ϴ�.
        }
    }

    private void HideWarning()
    {
        if (warning != null)
        {
            warning.gameObject.SetActive(false);
        }
    }

    private bool CanPurchase(int cost)
    {
        if (stageManager != null && stageManager.dia >= cost)
        {
            return true;
        }
        else
        {
            ShowWarning("���� �����մϴ�!");
            return false;
        }
    }
}
