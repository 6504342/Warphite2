using System.Collections;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject camBossRoom;
    private GameObject camlockroom;
    private GameObject maincam;
    public GameObject bossspawn;
    public GameObject bosswarppoint;
    public GameObject musicbg;
    public GameObject musicbgboss;

    private Bossdata bd;
    private bool isCamrunRunning = false; // ตัวแปรสถานะ

    private void Awake()
    {
        camlockroom = GameObject.FindWithTag("Finish");
        maincam = GameObject.FindWithTag("MainCamera");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isCamrunRunning) // ตรวจสอบสถานะก่อนเรียกใช้ Coroutine
        {
            camlockroom.SetActive(false);
            musicbg.SetActive(false);
            StartCoroutine(camrun());
        }
        else
        {
            Debug.Log("not found");
        }
    }
    public void camback() 
    {
        camlockroom.SetActive(true);
        musicbgboss.SetActive(false);
        musicbg.SetActive(true);
    }

    private IEnumerator camrun()
    {
        isCamrunRunning = true; // ตั้งค่าสถานะเป็นกำลังทำงาน

        yield return new WaitForSeconds(0.1f);
        maincam.transform.position = camBossRoom.transform.position;
        yield return new WaitForSeconds(3f);
        Vector2 Bosspoint = bosswarppoint.transform.position;
        GameObject BossSpawnPoint = Instantiate(bossspawn, Bosspoint, transform.rotation);
        yield return new WaitForSeconds(3f);
        bd = GameObject.FindObjectOfType<Bossdata>();
        bd.BossHealthAffect();
        musicbgboss.SetActive(true);

        isCamrunRunning = false; // ตั้งค่าสถานะเป็นไม่กำลังทำงาน
    }
}
