using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChooseNanEDo : MonoBehaviour
{
    public GameObject EasyBotton;
    public GameObject NomalBotton;
    public GameObject HardBotton;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void Easy()
    {
        SceneManager.LoadScene("Easy");
    }
    public void Nomal()
    {
        SceneManager.LoadScene("Nomal");
    }
    public void Hard()
    {
        SceneManager.LoadScene("Hard");
    }
}
