using System.Collections;
using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    public Transform waypoint;
    private PlayerControl player;
    private bool isCanwarp = true;

    void Start()
    {
        player = FindAnyObjectByType<PlayerControl>();

        if (waypoint == null)
        {
            Debug.LogError("Waypoint is not set. Please assign a Transform to the waypoint.");
        }

        if (player == null)
        {
            Debug.LogError("PlayerControl not found.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKey(KeyCode.W) && isCanwarp == true)
        {
            isCanwarp = false;
            StartCoroutine(WarpPlayer(waypoint));
        }
    }

    private IEnumerator WarpPlayer(Transform targetPortal)
    {
        //������ yield return ���� StartCoroutine(player.TransitionPlayer()); ������͹��������稡�͹����仵��
        StartCoroutine(player.TransitionPlayer());
        yield return new WaitForSeconds(1.5f); // ����������Ѻ����ʴ���
        player.transform.position = targetPortal.transform.position;
        yield return new WaitForSeconds(1.0f);
        isCanwarp = true;
    }
}

