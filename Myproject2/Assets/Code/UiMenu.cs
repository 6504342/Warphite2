using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UiMenu : MonoBehaviour
{
    public void PlayGame(int level)
    {
        SceneManager.LoadScene("Level" + level); // ����¹�繪��� Scene �ͧ�س
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game!");
    }
}
