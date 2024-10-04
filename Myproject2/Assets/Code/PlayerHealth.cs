using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // ��Ҿ�ѧ���Ե�٧�ش�ͧ������
    private float currentHealth;   // ��Ҿ�ѧ���Ե�Ѩ�غѹ�ͧ������
    public Slider healthSlider;    // ��ҧ�ԧ�֧ UI Slider
    private PlayerControl playerControl;


    void Start()
    {

        playerControl = GetComponent<PlayerControl>();
        currentHealth = maxHealth; // ��˹����������鹢ͧ��ѧ���Ե
        healthSlider.maxValue = maxHealth; // ��˹���Ҿ�ѧ���Ե�٧�ش� Slider
        healthSlider.value = currentHealth; // �ʴ���Ҿ�ѧ���Ե�������� Slider
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack")) // ��Ǩ�ͺ��� GameObject ��誹�� Tag �� "Enemy" �������
        {

            TakeDamage(-10f); // Ŵ��ѧ���Եŧ�����ҷ���˹� (�� 10)
        }
        if (collision.gameObject.CompareTag("PlayerItemHealth")) 
        {
            TakeDamage(50f);
            Destroy(collision.gameObject);
        }
    }
    void TakeDamage(float damage)
    {
        currentHealth += damage; // Ŵ��ѧ���Ե�Ѩ�غѹŧ�����Ҥ����������
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ��ͧ�ѹ�������Ҿ�ѧ���Ե��ӡ��� 0 �����Թ��Ҿ�ѧ���Ե�٧�ش
        healthSlider.value = currentHealth; // ��Ѻ��ا���� Slider ����ʹ���ͧ�Ѻ��Ҿ�ѧ���Ե�Ѩ�غѹ

        if (currentHealth <= 0)
        {
            Die(); // ���¡�ѧ��ѹ����͵�� (����ö��˹��������������س��ͧ���)
        }
    }

    void Die()
    {
        playerControl.Dead();
        Debug.Log("Player Died!");
    }
}
