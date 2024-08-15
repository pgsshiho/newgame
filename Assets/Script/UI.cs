using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 현재 UI 오브젝트가 파괴되지 않도록 설정
    }

    void OnEnable()
    {
        // 씬이 로드될 때마다 호출될 메서드 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // 씬 로드 이벤트에서 메서드 등록 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬이 로드될 때 호출되는 메서드
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(false); // 씬이 로드될 때 UI를 비활성화
    }
}
