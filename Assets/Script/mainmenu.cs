using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class mainmenu : MonoBehaviour
{
    public GameObject SB;
    public GameObject QB;
    public GameObject OB;

    void Start()
    {
        AddHighlightButton(SB);
        AddHighlightButton(QB);
        AddHighlightButton(OB);
    }

    void AddHighlightButton(GameObject button)
    {
        if (button != null)
        {
            HighlightButton highlightButton = button.AddComponent<HighlightButton>();
            highlightButton.highlightColor = Color.yellow;
        }
        else
        {
            Debug.LogError("버튼 오브젝트가 null입니다.");
        }
    }

    void Update()
    {

    }

    public void starts()
    {
        SceneManager.LoadScene("port");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Option()
    {
    }
}