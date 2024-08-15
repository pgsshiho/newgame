using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject dialoguePanel;
    private bool isPlayerInRange = false;
    public GameObject shop;

    // static 변수로 선언하여 모든 Shop 오브젝트가 동일한 값을 공유하도록 함
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

        UpdateCostText(); // 아이템 비용 텍스트를 업데이트
    }

    private void OnEnable()
    {
        // 오브젝트가 활성화될 때 텍스트에 비용 값을 적용
        UpdateCostText();
    }

    private void UpdateCostText()
    {
        // 텍스트를 갱신
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
                stageManager.UpdateDiaCount(); // 다이아몬드 수 업데이트
                meatCost += 50;

                UpdateCostText(); // 비용 텍스트 갱신
            }
        }
        else
        {
            ShowWarning("현재 체력이 이미 최대치입니다.");
        }
    }

    public void PurchaseCloth()
    {
        if (CanPurchase(clothCost))
        {
            stageManager.dia -= clothCost;
            stageManager.UpdateDiaCount(); // 다이아몬드 수 업데이트
            clothCost += 50;
            clothCount++;

            UpdateCostText(); // 비용 텍스트 갱신
        }
    }

    public void PurchaseSword()
    {
        if (CanPurchase(swordCost))
        {
            stageManager.dia -= swordCost;
            stageManager.UpdateDiaCount(); // 다이아몬드 수 업데이트
            swordCost += 50;
            swordCount++;
            UpdateCostText(); // 비용 텍스트 갱신
        }
    }

    private void ShowWarning(string message)
    {
        if (warning != null)
        {
            warning.text = message;
            warning.gameObject.SetActive(true);
            Invoke("HideWarning", 2f); // 2초 후에 경고 메시지를 숨깁니다.
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
            ShowWarning("돈이 부족합니다!");
            return false;
        }
    }
}
