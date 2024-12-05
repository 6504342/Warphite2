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
        //ถ้าใส่ yield return แล้ว StartCoroutine(player.TransitionPlayer()); จะเล่นไอนี้ให้เสร็จก่อนค่อยไปต่อ
        StartCoroutine(player.TransitionPlayer());
        yield return new WaitForSeconds(1.5f); // รอเวลาสำหรับการแสดงผล
        player.transform.position = targetPortal.transform.position;
        yield return new WaitForSeconds(1.0f);
        isCanwarp = true;
    }
}

