using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; } // 싱글톤 인스턴스 프로퍼티
    public GameObject Settingpanel; // 패널 오브젝트를 공개합니다.
    private bool Setting = false; // 초기값을 false로 설정합니다.
    public Slider HpBarSlider;
    protected float curHealth = 100; //* 현재 체력
    public float maxHealth = 100; //* 최대 체력
    public GameObject RE;
    public GameObject Bm;
    public GameObject QU;
    public GameObject panel;
    public int dia; // 다이아몬드 수
    public TextMeshProUGUI diacount; // 다이아몬드 수를 표시할 UI
    private RectTransform diacountParent; // TextMeshProUGUI의 부모 RectTransform
    private Vector2 initialPosition = new Vector2(510, -170); // 초기 위치를 저장할 변수
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

        // DontDestroyOnLoad를 UI와 timerText 오브젝트에도 적용
        if (UI != null)
        {
            DontDestroyOnLoad(UI);
        }
        if (timerText != null)
        {
            DontDestroyOnLoad(timerText.gameObject);
        }

        UI.SetActive(false); // 기본적으로 UI를 비활성화
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
            Settingpanel.SetActive(false); // 패널을 시작 시 비활성화합니다.
            panel.SetActive(false);
        }
        if (diacount != null)
        {
            diacountParent = diacount.GetComponentInParent<RectTransform>();
            if (diacountParent != null)
            {
                diacountParent.anchoredPosition = initialPosition; // 초기 위치 설정
            }
        }
        UpdateDiaCount(); // 초기 다이아몬드 수 업데이트

        // 버튼의 OnClick 이벤트를 동적으로 설정
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
            Setting = !Setting; // Setting 변수의 값을 토글합니다.
            if (Setting)
            {
                Time.timeScale = 0; // 게임 시간 멈춤
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(true); // 패널을 활성화합니다.
                    panel.SetActive(true);
                }
            }
            else
            {
                Time.timeScale = 1; // 게임 시간 재개
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(false); // 패널을 비활성화합니다.
                    panel.SetActive(false);
                }
                Cursor.visible = false;
            }
        }
    }

    public void CheckHp() //*HP 갱신
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

        UI.SetActive(false); // 씬을 넘어갈 때 UI를 비활성화
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
        Cursor.visible = false; // 마우스 커서 숨김
    }

    // 다이아몬드 수를 업데이트하는 메서드
    public void UpdateDiaCount()
    {
        if (diacount != null)
        {
            diacount.text = dia.ToString();

            // 다이아몬드 수의 자릿수에 따라 부모 RectTransform의 위치를 조정
            if (diacountParent != null)
            {
                int digitCount = dia.ToString().Length;
                float offset = digitCount * 10f; // 자릿수에 따라 이동할 거리 설정 (필요에 따라 조정)
                diacountParent.anchoredPosition = new Vector2(initialPosition.x - offset - 20, initialPosition.y);
            }
        }
    }

    // 씬이 로드될 때 호출되는 메서드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Easy, Hard, Normal 씬에서만 UI를 활성화
        if (scene.name == "Easy" || scene.name == "Hard" || scene.name == "Normal")
        {
            UI.SetActive(true); // UI 활성화
        }
        else
        {
            UI.SetActive(false); // 다른 씬에서는 UI 비활성화
        }

        if (Settingpanel != null)
        {
            Settingpanel.SetActive(false); // 씬이 로드될 때 패널은 기본적으로 비활성화
        }
        if (panel != null)
        {
            panel.SetActive(false); // 패널이 다시 활성화되지 않도록 보장
        }
        Cursor.visible = false; // 커서 숨기기
        Time.timeScale = 1; // 게임 시간 재개
    }

    // 씬이 언로드될 때 호출되는 메서드
    private void OnSceneUnloaded(Scene scene)
    {
        UI.SetActive(false); // 씬이 넘어갈 때 UI를 비활성화
    }
}
