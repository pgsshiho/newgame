using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject Settingpanel; // �г� ������Ʈ�� �����մϴ�.
    bool Setting = false; // �ʱⰪ�� false�� �����մϴ�.

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
}
