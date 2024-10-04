using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // ค่าพลังชีวิตสูงสุดของผู้เล่น
    private float currentHealth;   // ค่าพลังชีวิตปัจจุบันของผู้เล่น
    public Slider healthSlider;    // อ้างอิงถึง UI Slider
    private PlayerControl playerControl;


    void Start()
    {

        playerControl = GetComponent<PlayerControl>();
        currentHealth = maxHealth; // กำหนดค่าเริ่มต้นของพลังชีวิต
        healthSlider.maxValue = maxHealth; // กำหนดค่าพลังชีวิตสูงสุดใน Slider
        healthSlider.value = currentHealth; // แสดงค่าพลังชีวิตเริ่มต้นใน Slider
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack")) // ตรวจสอบว่า GameObject ที่ชนมี Tag เป็น "Enemy" หรือไม่
        {

            TakeDamage(-10f); // ลดพลังชีวิตลงตามค่าที่กำหนด (เช่น 10)
        }
        if (collision.gameObject.CompareTag("PlayerItemHealth")) 
        {
            TakeDamage(50f);
            Destroy(collision.gameObject);
        }
    }
    void TakeDamage(float damage)
    {
        currentHealth += damage; // ลดพลังชีวิตปัจจุบันลงตามค่าความเสียหาย
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ป้องกันไม่ให้ค่าพลังชีวิตต่ำกว่า 0 หรือเกินค่าพลังชีวิตสูงสุด
        healthSlider.value = currentHealth; // ปรับปรุงค่าใน Slider ให้สอดคล้องกับค่าพลังชีวิตปัจจุบัน

        if (currentHealth <= 0)
        {
            Die(); // เรียกฟังก์ชันเมื่อตาย (สามารถกำหนดเพิ่มเติมตามที่คุณต้องการ)
        }
    }

    void Die()
    {
        playerControl.Dead();
        Debug.Log("Player Died!");
    }
}
