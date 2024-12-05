using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private GameObject player; // ตัวแปรสำหรับอ้างอิง Player
    private PlayerControl playertransit;
    private bool isCanwarp = true;

    // Start is called before the first frame update
    void Start()
    {
        // หา Player อัตโนมัติ
        player = GameObject.FindGameObjectWithTag("Player");
        playertransit = FindAnyObjectByType<PlayerControl>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (Input.GetKey(KeyCode.W)) && isCanwarp == true)
        {
                isCanwarp = false;
                StartCoroutine(ChangeToLevel2());
        }
    }
    public IEnumerator ChangeToLevel2()
    {
        StartCoroutine(playertransit.TransitionPlayer());
        yield return new WaitForSeconds(2f);
        player.transform.position = new Vector2(0, 0);
        isCanwarp = true;
        SceneManager.LoadScene("Level2"); // โหลด Scene ใหม
        yield return new WaitForSeconds(2f);
    }
}
