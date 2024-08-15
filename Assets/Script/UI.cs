using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // ���� UI ������Ʈ�� �ı����� �ʵ��� ����
    }

    void OnEnable()
    {
        // ���� �ε�� ������ ȣ��� �޼��� ���
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // �� �ε� �̺�Ʈ���� �޼��� ��� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ���� �ε�� �� ȣ��Ǵ� �޼���
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(false); // ���� �ε�� �� UI�� ��Ȱ��ȭ
    }
}
