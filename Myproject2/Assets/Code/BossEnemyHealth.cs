using UnityEngine;
using System.Collections;

public class BossEnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // ค่าพลังชีวิตสูงสุดของศัตรู
    private float currentHealth;   // ค่าพลังชีวิตปัจจุบันของศัตรู
    public float deadtime = 3f;
    public Animator animatorenemy;
    public AudioSource audioSource;
    public AudioClip[] soundEffects;
    public float knockbackForce = 5f; // แรงที่ใช้ในการกระเด็น
    public int damagedoes = 1;
    [SerializeField] public GameObject slashing;
    private GameObject wingame;
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
            Vector2 collisionPoint = gameObject.transform.position;
            GameObject splast = Instantiate(slashing, collisionPoint, transform.rotation);
            Destroy(splast, 1.0f);
        }
    }
    

    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // ลดค่าพลังชีวิตตามความเสียหายที่ได้รับ
        maxHealth -= damage;
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
        yield return new WaitForSeconds(deadtime);
        Destroy(gameObject);
    }
    public void IsDead() 
    {
        Destroy(gameObject, 3f);
    }
    public void HeartBeat() 
    {
        audioSource.clip = soundEffects[1];
        audioSource.Play();
        
    }
    public void HandFast()
    {
        audioSource.clip = soundEffects[2];
        audioSource.Play();

    }
    public void StopAllSounds() 
    {
            audioSource.Stop(); 
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>(); 
            foreach (AudioSource source in allAudioSources) 
            {
                source.Stop(); 
            } 
        
    }
}
