using UnityEngine;
using System.Collections;

public class Uipausemenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // ������§�Ѻ UI �ͧ���� Pause
    private bool isPaused = false;
    public Animator transitionmenu;

    void Start()
    {
        // ������͹����ѹ�ӧҹẺ���������Ѻ�������
        transitionmenu.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  // ������ ESC ������ش��
        {
            if (isPaused)
            {
                Resume();  // ��ҡ��ѧ��ش���� ����Ѻ����蹵��
            }
            else
            {
                Pause();  // ��������ش���� ���ӡ����ش��
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);  // �Դ���� Pause
        Time.timeScale = 1f;           // ��Ѻ���һ���
        isPaused = false;              // ����¹ʶҹ��������ش��
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);   // �Դ���� Pause
        Time.timeScale = 0f;           // ��ش�������
        isPaused = true;               // ����¹ʶҹ�����ش��
    }

    public void QuitGame()
    {
        Application.Quit();  // �Դ�� (��������� build �� application ��ҹ��)
        Debug.Log("Quitting game...");  // �����͵�Ǩ�ͺ� Editor
    }
}
