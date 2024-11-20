using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Uipausemenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // ������§�Ѻ UI �ͧ���� Pause
    public GameObject SettingMenuUI;
    private bool isPaused = false;
    public Animator transitionmenu;
    public Animator transitionsoundmenu;

    void Start()
    {
        // ������͹����ѹ�ӧҹẺ���������Ѻ�������
        transitionmenu.updateMode = AnimatorUpdateMode.UnscaledTime;
        transitionsoundmenu.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  // ������ ESC ������ش��
        {
            if (isPaused)
            {
                Resume();  // ��ҡ��ѧ��ش���� ����Ѻ����蹵��
                SettingMenuUI.SetActive(false);
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
    public void ReturnToMenu(int level)
    {
        SceneManager.LoadScene("Menu"); // ����¹�繪��� Scene �ͧ�س
    }
}
