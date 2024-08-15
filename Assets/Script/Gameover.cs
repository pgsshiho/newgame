using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Gameover : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            SceneManager.LoadScene("mainmenu");
            Cursor.visible = true;
        }
    }
}
