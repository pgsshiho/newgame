using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; } // �̱��� �ν��Ͻ� ������Ƽ
    public GameObject Settingpanel; // �г� ������Ʈ�� �����մϴ�.
    private bool Setting = false; // �ʱⰪ�� false�� �����մϴ�.
    public Slider HpBarSlider;
    protected float curHealth = 100; //* ���� ü��
    public float maxHealth = 100; //* �ִ� ü��
    public GameObject RE;
    public GameObject Bm;
    public GameObject QU;
    public GameObject panel;
    public int dia; // ���̾Ƹ�� ��
    public TextMeshProUGUI diacount; // ���̾Ƹ�� ���� ǥ���� UI
    private RectTransform diacountParent; // TextMeshProUGUI�� �θ� RectTransform
    private Vector2 initialPosition = new Vector2(510, -170); // �ʱ� ��ġ�� ������ ����
    public GameObject UI;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // DontDestroyOnLoad�� UI�� timerText ������Ʈ���� ����
        if (UI != null)
        {
            DontDestroyOnLoad(UI);
        }
        if (timerText != null)
        {
            DontDestroyOnLoad(timerText.gameObject);
        }

        UI.SetActive(false); // �⺻������ UI�� ��Ȱ��ȭ
        dia = 0;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void Start()
    {
        Cursor.visible = false;
        if (Settingpanel != null)
        {
            Settingpanel.SetActive(false); // �г��� ���� �� ��Ȱ��ȭ�մϴ�.
            panel.SetActive(false);
        }
        if (diacount != null)
        {
            diacountParent = diacount.GetComponentInParent<RectTransform>();
            if (diacountParent != null)
            {
                diacountParent.anchoredPosition = initialPosition; // �ʱ� ��ġ ����
            }
        }
        UpdateDiaCount(); // �ʱ� ���̾Ƹ�� �� ������Ʈ

        // ��ư�� OnClick �̺�Ʈ�� �������� ����
        if (RE != null)
        {
            RE.GetComponent<Button>().onClick.AddListener(Re);
        }
        if (Bm != null)
        {
            Bm.GetComponent<Button>().onClick.AddListener(BM);
        }
        if (QU != null)
        {
            QU.GetComponent<Button>().onClick.AddListener(quit);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Setting = !Setting; // Setting ������ ���� ����մϴ�.
            if (Setting)
            {
                Time.timeScale = 0; // ���� �ð� ����
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(true); // �г��� Ȱ��ȭ�մϴ�.
                    panel.SetActive(true);
                }
            }
            else
            {
                Time.timeScale = 1; // ���� �ð� �簳
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(false); // �г��� ��Ȱ��ȭ�մϴ�.
                    panel.SetActive(false);
                }
                Cursor.visible = false;
            }
        }
    }

    public void CheckHp() //*HP ����
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void Damage(float damage)
    {
        if (maxHealth == 0 || curHealth <= 0)
            return;
        curHealth -= damage;
        CheckHp();
        if (curHealth <= 0)
        {
            SceneManager.LoadScene("GAME OVER");
        }
    }

    public void BM()
    {
        SceneManager.LoadScene("Mainmenu");
        Settingpanel.SetActive(false);
        panel.SetActive(false);
        timerText.gameObject.SetActive(false);
        Cursor.visible = true;

        UI.SetActive(false); // ���� �Ѿ �� UI�� ��Ȱ��ȭ
    }

    public void quit()
    {
        Application.Quit();
    }

    public void Re()
    {
        Settingpanel.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false; // ���콺 Ŀ�� ����
    }

    // ���̾Ƹ�� ���� ������Ʈ�ϴ� �޼���
    public void UpdateDiaCount()
    {
        if (diacount != null)
        {
            diacount.text = dia.ToString();

            // ���̾Ƹ�� ���� �ڸ����� ���� �θ� RectTransform�� ��ġ�� ����
            if (diacountParent != null)
            {
                int digitCount = dia.ToString().Length;
                float offset = digitCount * 10f; // �ڸ����� ���� �̵��� �Ÿ� ���� (�ʿ信 ���� ����)
                diacountParent.anchoredPosition = new Vector2(initialPosition.x - offset - 20, initialPosition.y);
            }
        }
    }

    // ���� �ε�� �� ȣ��Ǵ� �޼���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Easy, Hard, Normal �������� UI�� Ȱ��ȭ
        if (scene.name == "Easy" || scene.name == "Hard" || scene.name == "Normal")
        {
            UI.SetActive(true); // UI Ȱ��ȭ
        }
        else
        {
            UI.SetActive(false); // �ٸ� �������� UI ��Ȱ��ȭ
        }

        if (Settingpanel != null)
        {
            Settingpanel.SetActive(false); // ���� �ε�� �� �г��� �⺻������ ��Ȱ��ȭ
        }
        if (panel != null)
        {
            panel.SetActive(false); // �г��� �ٽ� Ȱ��ȭ���� �ʵ��� ����
        }
        Cursor.visible = false; // Ŀ�� �����
        Time.timeScale = 1; // ���� �ð� �簳
    }

    // ���� ��ε�� �� ȣ��Ǵ� �޼���
    private void OnSceneUnloaded(Scene scene)
    {
        UI.SetActive(false); // ���� �Ѿ �� UI�� ��Ȱ��ȭ
    }
}
