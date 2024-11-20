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
        if (isDead) yield break; // ��ش�ѧ��ѹ����µ������
        isDead = true; // ��駤��ʶҹ�����ѵ�ٵ������
        effect.EnemySoundHighpitch(1);

        animatorenemy.SetBool("Dead", true); // ������͹����ѹ��õ��
        rb.velocity = Vector2.zero; // ��ش�������͹���ͧ�ѵ��
        var monster = FindObjectOfType<MonsterPatrol>();
        if (monster != null)
        {
            monster.StopAllCoroutines(); // ��ش Coroutine �������ͧ MonsterPatrol
            Debug.Log("MonsterPatrol stopped");
        }

        // ��ش��÷ӧҹ�ͧ MonsterPatrol2
        var monster2 = FindObjectOfType<MonsterPatrol2>();
        if (monster2 != null)
        {
            monster2.StopAllCoroutines(); // ��ش Coroutine �������ͧ MonsterPatrol2
            Debug.Log("MonsterPatrol2 stopped");
        }
        // �ͨ������͹����ѹ��õ�¨��������
        yield return new WaitForSeconds(2.5f);
        bd.BossHealthReduce();
        Itemdrop.gameObject.SetActive(true);
        Destroy(gameObject); 
    }
}
