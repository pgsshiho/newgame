using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject Settingpanel; // 패널 오브젝트를 공개합니다.
    bool Setting = false; // 초기값을 false로 설정합니다.
    public Slider HpBarSlider;
    protected float curHealth = 1000; //* 현재 체력
    public float maxHealth = 1000; //* 최대 체력
    public void SetHp(float amount) //*Hp설정
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }

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
}
