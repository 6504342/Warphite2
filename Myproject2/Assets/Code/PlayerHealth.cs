using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;     // ค่าพลังชีวิตสูงสุดของผู้เล่น
    public float damageteken = 10f;
    private float currentHealth;       // ค่าพลังชีวิตปัจจุบันของผู้เล่น
    public Slider healthSlider;        // อ้างอิงถึง UI Slider
    private PlayerControl playerControl;
    private bool isDead = false;       // ตัวแปรตรวจสอบสถานะการตายของผู้เล่น
    PlayerSoundEffect SoundEffectItem;
    public Text lifetext;


    void Start()
    {
        lifetext.text = "x 5";
        playerControl = GetComponent<PlayerControl>();
        SoundEffectItem = GetComponent<PlayerSoundEffect>();
        currentHealth = maxHealth;     // กำหนดค่าเริ่มต้นของพลังชีวิต
        healthSlider.maxValue = maxHealth; // กำหนดค่าพลังชีวิตสูงสุดใน Slider
        healthSlider.value = currentHealth; // แสดงค่าพลังชีวิตเริ่มต้นใน Slider
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // ถ้าผู้เล่นตายแล้วจะไม่สามารถถูกโจมตีได้

        if (collision.gameObject.CompareTag("EnemyAttack")) // ตรวจสอบว่า GameObject ที่ชนมี Tag เป็น "Enemy" หรือไม่
        {
            TakeDamage(-damageteken); // ลดพลังชีวิตลงตามค่าที่กำหนด (เช่น 10)
        }
        else if (collision.gameObject.CompareTag("PlayerItemHealth")) // ตรวจสอบการเก็บไอเทมฟื้นพลัง
        {
            TakeDamage(50); // เพิ่มค่าพลังชีวิต
            Destroy(collision.gameObject); // ลบไอเทมเมื่อเก็บแล้ว
            SoundEffectItem.PlaySoundHighpitch(4);
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth += damage; // เพิ่มหรือลดพลังชีวิตปัจจุบันตามค่าความเสียหาย
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ป้องกันไม่ให้ค่าพลังชีวิตต่ำกว่า 0 หรือเกินค่าพลังชีวิตสูงสุด
        healthSlider.value = currentHealth; // ปรับปรุงค่าใน Slider ให้สอดคล้องกับค่าพลังชีวิตปัจจุบัน

        if (currentHealth <= 0)
        {
            Die(); // เรียกฟังก์ชันเมื่อตาย
        }
    }

    void Die()
    {
        isDead = true; // ตั้งค่าว่าผู้เล่นตายแล้ว

        playerControl.Dead(); // เรียกฟังก์ชันการตายจาก PlayerControl
        Debug.Log("Player Died!");
        // คุณอาจจะเพิ่มฟังก์ชัน Respawn หรือแสดง UI การตายได้ที่นี่
    }

    // ฟังก์ชันสำหรับการเกิดใหม่ของผู้เล่น (ถ้ามี)
    public void Respawn()
    {
        isDead = false; // รีเซ็ตสถานะการตาย
        currentHealth = maxHealth; // รีเซ็ตพลังชีวิต
        healthSlider.value = currentHealth; // ปรับปรุงค่าใน Slider
        Debug.Log("Player Respawned!");
        // เพิ่มฟังก์ชันอื่น ๆ เช่นการย้ายผู้เล่นไปจุดเกิดใหม่ได้ที่นี่
    }
    public void textlife(string lifetextdecrease) 
    {
        lifetext.text = lifetextdecrease;
    }
}
