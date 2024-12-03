using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private GameObject player; // ���������Ѻ��ҧ�ԧ Player

    // Start is called before the first frame update
    void Start()
    {
        // �� Player �ѵ��ѵ�
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
        SceneManager.LoadScene("Level2"); // ��Ŵ Scene ���
        yield return new WaitForSeconds(2f);
    }
}
