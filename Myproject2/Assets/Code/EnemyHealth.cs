using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // ค่าพลังชีวิตสูงสุดของศัตรู
    private float currentHealth;   // ค่าพลังชีวิตปัจจุบันของศัตรู
    public Animator animatorenemy;
    public Transform Itemdrop;

    public float knockbackForce = 5f; // แรงที่ใช้ในการกระเด็น
    public int damagedoes = 1;

    private Rigidbody2D rb;
    private bool isDead = false;  // ตัวแปรตรวจสอบว่าศัตรูตายหรือยัง
    private Bossdata bd;
    public GameObject slashing;
    EnemySoundEffect effect;

    void Start()
    {
        effect = GetComponent<EnemySoundEffect>();
        currentHealth = maxHealth; // กำหนดค่าพลังชีวิตเริ่มต้น
        rb = GetComponent<Rigidbody2D>(); // อ้างอิงถึง Rigidbody2D ของศัตรู
        bd = FindObjectOfType<Bossdata>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") && !isDead) // ตรวจสอบว่าโดนโจมตีจาก Player หรือไม่
        {
            int damage = damagedoes;
            TakeDamage(damage);

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized; // หาทิศทางการกระเด็นถอยไป
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // เพิ่มแรงกระเด็นถอยไป

            Vector2 collisionPoint = gameObject.transform.position;
            GameObject splast = Instantiate(slashing, collisionPoint, transform.rotation);
            Destroy(splast, 1.0f);
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
        if (isDead) yield break; 
        
        isDead = true;
        effect.EnemySoundLowpitch(1);


        animatorenemy.SetBool("Dead", true); 
        rb.velocity = Vector2.zero;
        if (Itemdrop != null)
        {
            StartCoroutine(ItemSpawn());
        }
        Invoke("destroyobject", 1f);
        yield return new WaitForSeconds(2.5f);
        bd.BossHealthReduce();
        Destroy(gameObject); 
    }

    public void AttackEnemySound()
    {
        if (!isDead) 
        {
            effect.EnemySoundHighpitch(0);
        }
    }
    public IEnumerator ItemSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 itemposition = gameObject.transform.position;
        Instantiate(Itemdrop, itemposition, transform.rotation);
        Debug.Log("work");
    }
}
