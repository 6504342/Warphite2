using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // ค่าพลังชีวิตสูงสุดของศัตรู
    private float currentHealth;   // ค่าพลังชีวิตปัจจุบันของศัตรู
    public float deadtime = 3f;
    public Animator animatorenemy;
    public Transform Itemdrop;

    public float knockbackForce = 5f; // แรงที่ใช้ในการกระเด็น
    public int damagedoes = 1;

    private Rigidbody2D rb;
    private bool isDead = false;  // ตัวแปรตรวจสอบว่าศัตรูตายหรือยัง

    void Start()
    {
        currentHealth = maxHealth; // กำหนดค่าพลังชีวิตเริ่มต้น
        rb = GetComponent<Rigidbody2D>(); // อ้างอิงถึง Rigidbody2D ของศัตรู
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") && !isDead) // ตรวจสอบว่าโดนโจมตีจาก Player หรือไม่
        {
            int damage = damagedoes;
            TakeDamage(damage);

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized; // หาทิศทางการกระเด็นถอยไป
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // เพิ่มแรงกระเด็นถอยไป
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage; // ลดค่าพลังชีวิตตามความเสียหายที่ได้รับ
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ป้องกันไม่ให้ค่าพลังชีวิตต่ำกว่า 0 หรือเกินค่าพลังชีวิตสูงสุด

        if (currentHealth <= 0 && !isDead) // ตรวจสอบว่าพลังชีวิตหมดแล้วและยังไม่ตาย
        {
            StartCoroutine(Die()); // เรียกฟังก์ชันเมื่อตาย
        }
    }

    public void DamagePlayer(int dmg)
    {
        damagedoes = dmg; // เพิ่มความเสียหายที่ผู้เล่นทำได้
    }

    public IEnumerator Die()
    {
        if (isDead) yield break; // หยุดฟังก์ชันถ้าเคยตายแล้ว
        isDead = true; // ตั้งค่าสถานะว่าศัตรูตายแล้ว

        animatorenemy.SetBool("Dead", true); // เริ่มแอนิเมชันการตาย
        rb.velocity = Vector2.zero; // หยุดการเคลื่อนที่ของศัตรู

        // รอจนกว่าแอนิเมชันการตายจะเล่นเสร็จ
        yield return new WaitForSeconds(deadtime);
        Itemdrop.gameObject.SetActive(true);
        Destroy(gameObject); 
    }
}
