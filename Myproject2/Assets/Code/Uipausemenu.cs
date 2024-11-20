using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Uipausemenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // เชื่อมโยงกับ UI ของเมนู Pause
    public GameObject SettingMenuUI;
    private bool isPaused = false;
    public Animator transitionmenu;
    public Animator transitionsoundmenu;

    void Start()
    {
        // ทำให้แอนิเมชันทำงานแบบไม่ขึ้นอยู่กับเวลาในเกม
        transitionmenu.updateMode = AnimatorUpdateMode.UnscaledTime;
        transitionsoundmenu.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  // กดปุ่ม ESC เพื่อหยุดเกม
        {
            if (isPaused)
            {
                Resume();  // ถ้ากำลังหยุดอยู่ ให้กลับมาเล่นต่อ
                SettingMenuUI.SetActive(false);
            }
            else
            {
                Pause();  // ถ้าไม่หยุดอยู่ ให้ทำการหยุดเกม
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);  // ปิดเมนู Pause
        Time.timeScale = 1f;           // กลับเวลาปกติ
        isPaused = false;              // เปลี่ยนสถานะเป็นไม่หยุดเกม
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);   // เปิดเมนู Pause
        Time.timeScale = 0f;           // หยุดเวลาในเกม
        isPaused = true;               // เปลี่ยนสถานะเป็นหยุดเกม
    }

    public void QuitGame()
    {
        Application.Quit();  // ปิดเกม (ใช้ได้เมื่อ build เป็น application เท่านั้น)
        Debug.Log("Quitting game...");  // ใช้เพื่อตรวจสอบใน Editor
    }
    public void ReturnToMenu(int level)
    {
        SceneManager.LoadScene("Menu"); // เปลี่ยนเป็นชื่อ Scene ของคุณ
    }
}
