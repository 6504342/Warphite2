using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // ค่าพลังชีวิตสูงสุดของศัตรู
    private float currentHealth;   // ค่าพลังชีวิตปัจจุบันของศัตรู

    public float knockbackForce = 5f; // แรงที่ใช้ในการกระเด็น

    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth; // กำหนดค่าพลังชีวิตเริ่มต้น
        rb = GetComponent<Rigidbody2D>(); // อ้างอิงถึง Rigidbody2D ของศัตรู
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack")) // ตรวจสอบว่าโดนโจมตีจาก Player หรือไม่
        {
            float damage = 1f; // ค่าความเสียหายที่ได้รับ (ปรับได้ตามต้องการ)
            TakeDamage(damage);

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized; // หาทิศทางการกระเด็นถอยไป
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // เพิ่มแรงกระเด็นถอยไป
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage; // ลดค่าพลังชีวิตตามความเสียหายที่ได้รับ
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ป้องกันไม่ให้ค่าพลังชีวิตต่ำกว่า 0 หรือเกินค่าพลังชีวิตสูงสุด

        if (currentHealth <= 0)
        {
            Die(); // เรียกฟังก์ชันเมื่อตาย
        }
    }

    void Die()
    {
        // สิ่งที่เกิดขึ้นเมื่อศัตรูตาย เช่น ลบ GameObject ทิ้ง
        Debug.Log("Enemy Died!");
        Destroy(gameObject); // ลบศัตรูเมื่อพลังชีวิตเหลือ 0
    }
}
