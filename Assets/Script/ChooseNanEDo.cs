using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseNanEDo : MonoBehaviour
{
    public int NanEdo = 0;


    private void Start()
    {
        // 씬 로드 완료 후 버튼 이벤트를 동적으로 설정합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // 씬 로드 이벤트 핸들러 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 시 버튼 이벤트 설정
        SetButtonEvents();
    }

    public void SetButtonEvents()
    {
        SetButtonEvent("EasyButton", Easy);
        SetButtonEvent("NormalButton", Normal);
        SetButtonEvent("HardButton", Hard);
    }

    private void SetButtonEvent(string buttonName, UnityEngine.Events.UnityAction action)
    {
        GameObject buttonObject = GameObject.Find(buttonName);
        if (buttonObject != null)
        {
            Button button = buttonObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(action);
            }
            else
            {
                Debug.LogWarning($"{buttonName} does not have a Button component.");
            }
        }
        else
        {
            Debug.LogWarning($"{buttonName} not found.");
        }
    }

    public void Easy()
    {
        NanEdo = 0;
        Debug.Log("Easy button clicked. Loading Easy scene.");
        SceneManager.LoadScene("Easy");
    }

    public void Normal()
    {
        NanEdo = 1;
        Debug.Log("Normal button clicked. Loading Normal scene.");
        SceneManager.LoadScene("Nomal");
    }

    public void Hard()
    {
        NanEdo = 2;
        Debug.Log("Hard button clicked. Loading Hard scene.");
        SceneManager.LoadScene("Hard");
    }
}
