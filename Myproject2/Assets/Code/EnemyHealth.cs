using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // ��Ҿ�ѧ���Ե�٧�ش�ͧ�ѵ��
    private float currentHealth;   // ��Ҿ�ѧ���Ե�Ѩ�غѹ�ͧ�ѵ��
    public Animator animatorenemy;
    public Transform Itemdrop;

    public float knockbackForce = 5f; // �ç�����㹡�á����
    public int damagedoes = 1;

    private Rigidbody2D rb;
    private bool isDead = false;  // ����õ�Ǩ�ͺ����ѵ�ٵ�������ѧ
    private Bossdata bd;
    public GameObject slashing;
    EnemySoundEffect effect;

    void Start()
    {
        effect = GetComponent<EnemySoundEffect>();
        currentHealth = maxHealth; // ��˹���Ҿ�ѧ���Ե�������
        rb = GetComponent<Rigidbody2D>(); // ��ҧ�ԧ�֧ Rigidbody2D �ͧ�ѵ��
        bd = FindObjectOfType<Bossdata>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") && !isDead) // ��Ǩ�ͺ���ⴹ���ըҡ Player �������
        {
            int damage = damagedoes;
            TakeDamage(damage);

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized; // �ҷ�ȷҧ��á���繶���
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // �����ç����繶���

            Vector2 collisionPoint = gameObject.transform.position;
            GameObject splast = Instantiate(slashing, collisionPoint, transform.rotation);
            Destroy(splast, 1.0f);
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage; // Ŵ��Ҿ�ѧ���Ե�������������·�����Ѻ
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ��ͧ�ѹ�������Ҿ�ѧ���Ե��ӡ��� 0 �����Թ��Ҿ�ѧ���Ե�٧�ش

        if (currentHealth <= 0 && !isDead) // ��Ǩ�ͺ��Ҿ�ѧ���Ե�����������ѧ�����
        {
            StartCoroutine(Die()); // ���¡�ѧ��ѹ����͵��
        }
    }

    public void DamagePlayer(int dmg)
    {
        damagedoes = dmg; // ��������������·������蹷���
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
