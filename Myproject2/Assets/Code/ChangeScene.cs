using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private GameObject player; // ตัวแปรสำหรับอ้างอิง Player

    // Start is called before the first frame update
    void Start()
    {
        // หา Player อัตโนมัติ
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(ChangeToLevel2());
        }
    }

    public IEnumerator ChangeToLevel2()
    {

        yield return new WaitForSeconds(2f);
        player.transform.position = new Vector2(0, 0);
        SceneManager.LoadScene("Level2"); // โหลด Scene ใหม
        yield return new WaitForSeconds(2f);
    }
}
