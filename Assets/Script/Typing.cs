using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Typing : MonoBehaviour
{
    public TMP_Text story;
    private string a;
    private string b;

    void Start()
    {
        // 대사
        a = "당신은 현재 이나라에 남은 마지막 기사입니다.";
        b = "당신은 최후의 기사로서 \n마지막 방어선을 지켜야만 합니다.";
        StartCoroutine(typingSequence());
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Start");
        }
    }

    IEnumerator typingSequence()
    {
        yield return StartCoroutine(typing(a, story));
        yield return new WaitForSeconds(0.6f);
        yield return StartCoroutine(typing(b, story));
        
    }

    IEnumerator typing(string talk, TMP_Text textComponent)
    {
        // 텍스트를 null 값으로 지정
        textComponent.text = string.Empty;

        for (int i = 0; i < talk.Length; i++)
        {
            textComponent.text += talk[i];
            // 속도
            yield return new WaitForSeconds(0.18f);
        }
    }
    
}