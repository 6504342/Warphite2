using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UiMenu : MonoBehaviour
{
    public void PlayGame(int level)
    {
        SceneManager.LoadScene("Level" + level); // เปลี่ยนเป็นชื่อ Scene ของคุณ
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
}
