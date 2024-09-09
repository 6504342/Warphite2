using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // ��Ҿ�ѧ���Ե�٧�ش�ͧ�ѵ��
    private float currentHealth;   // ��Ҿ�ѧ���Ե�Ѩ�غѹ�ͧ�ѵ��

    public float knockbackForce = 5f; // �ç�����㹡�á����

    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth; // ��˹���Ҿ�ѧ���Ե�������
        rb = GetComponent<Rigidbody2D>(); // ��ҧ�ԧ�֧ Rigidbody2D �ͧ�ѵ��
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack")) // ��Ǩ�ͺ���ⴹ���ըҡ Player �������
        {
            float damage = 1f; // ��Ҥ���������·�����Ѻ (��Ѻ������ͧ���)
            TakeDamage(damage);

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized; // �ҷ�ȷҧ��á���繶���
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // �����ç����繶���
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage; // Ŵ��Ҿ�ѧ���Ե�������������·�����Ѻ
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ��ͧ�ѹ�������Ҿ�ѧ���Ե��ӡ��� 0 �����Թ��Ҿ�ѧ���Ե�٧�ش

        if (currentHealth <= 0)
        {
            Die(); // ���¡�ѧ��ѹ����͵��
        }
    }

    void Die()
    {
        // ��觷���Դ���������ѵ�ٵ�� �� ź GameObject ���
        Debug.Log("Enemy Died!");
        Destroy(gameObject); // ź�ѵ������;�ѧ���Ե����� 0
    }
}
