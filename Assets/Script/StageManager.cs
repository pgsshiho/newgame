using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject Settingpanel; // 패널 오브젝트를 공개합니다.
    bool Setting = false; // 초기값을 false로 설정합니다.

    void Start()
    {
        if (Settingpanel != null)
        {
            Settingpanel.SetActive(false); // 패널을 시작 시 비활성화합니다.
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Setting = !Setting; // Setting 변수의 값을 토글합니다.
            if (Setting)
            {
                Time.timeScale = 0; // 게임 시간 멈춤
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(true); // 패널을 활성화합니다.
                }
            }
            else
            {
                Time.timeScale = 1; // 게임 시간 재개
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(false); // 패널을 비활성화합니다.
                }
            }
        }
    }   
}
