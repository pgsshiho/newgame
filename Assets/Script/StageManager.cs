using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public GameObject Settingpanel; // �г� ������Ʈ�� �����մϴ�.
    bool Setting = false; // �ʱⰪ�� false�� �����մϴ�.
    public Slider HpBarSlider;
    protected float curHealth = 1000; //* ���� ü��
    public float maxHealth = 1000; //* �ִ� ü��
    public void SetHp(float amount) //*Hp����
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }

    void Start()
    {
        if (Settingpanel != null)
        {
            Settingpanel.SetActive(false); // �г��� ���� �� ��Ȱ��ȭ�մϴ�.
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Setting = !Setting; // Setting ������ ���� ����մϴ�.
            if (Setting)
            {
                Time.timeScale = 0; // ���� �ð� ����
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(true); // �г��� Ȱ��ȭ�մϴ�.
                }
            }
            else
            {
                Time.timeScale = 1; // ���� �ð� �簳
                if (Settingpanel != null)
                {
                    Settingpanel.SetActive(false); // �г��� ��Ȱ��ȭ�մϴ�.
                }
            }
        }
    }
    public void CheckHp() //*HP ����
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
