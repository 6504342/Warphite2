using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Collider2D platformCollider; // Collider ของ Platform

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // ตรวจสอบว่าตัวละครกำลังเคลื่อนที่ขึ้น (ความเร็วในแกน y มากกว่า 0) หรือไม่
        if (collision.CompareTag("Player") && collision.attachedRigidbody.velocity.y > 0)
        {
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), platformCollider, true);
            Debug.Log("work");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // คืนค่าเป็นปกติเมื่อออกจากการชนกัน
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), platformCollider, false);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // ถ้ากด "S" ให้ทะลุลงไปได้
            StartCoroutine(DisableCollisionTemporarily());
        }
    }

    IEnumerator DisableCollisionTemporarily()
    {
        // ปิดการชนชั่วคราวและเปิดกลับหลังจาก 0.5 วินาที
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), platformCollider, true);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), platformCollider, false);
    }
}
