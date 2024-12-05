using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;     // ��Ҿ�ѧ���Ե�٧�ش�ͧ������
    public float damageteken = 10f;
    private float currentHealth;       // ��Ҿ�ѧ���Ե�Ѩ�غѹ�ͧ������
    public Slider healthSlider;        // ��ҧ�ԧ�֧ UI Slider
    private PlayerControl playerControl;
    private bool isDead = false;       // ����õ�Ǩ�ͺʶҹС�õ�¢ͧ������
    PlayerSoundEffect SoundEffectItem;
    public Text lifetext;


    void Start()
    {
        lifetext.text = "x 5";
        playerControl = GetComponent<PlayerControl>();
        SoundEffectItem = GetComponent<PlayerSoundEffect>();
        currentHealth = maxHealth;     // ��˹����������鹢ͧ��ѧ���Ե
        healthSlider.maxValue = maxHealth; // ��˹���Ҿ�ѧ���Ե�٧�ش� Slider
        healthSlider.value = currentHealth; // �ʴ���Ҿ�ѧ���Ե�������� Slider
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // ��Ҽ����蹵�����Ǩ��������ö�١������

        if (collision.gameObject.CompareTag("EnemyAttack")) // ��Ǩ�ͺ��� GameObject ��誹�� Tag �� "Enemy" �������
        {
            TakeDamage(-damageteken); // Ŵ��ѧ���Եŧ�����ҷ���˹� (�� 10)
        }
        else if (collision.gameObject.CompareTag("PlayerItemHealth")) // ��Ǩ�ͺ�����������鹾�ѧ
        {
            TakeDamage(50); // ������Ҿ�ѧ���Ե
            Destroy(collision.gameObject); // ź���������������
            SoundEffectItem.PlaySoundHighpitch(4);
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth += damage; // ��������Ŵ��ѧ���Ե�Ѩ�غѹ�����Ҥ����������
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ��ͧ�ѹ�������Ҿ�ѧ���Ե��ӡ��� 0 �����Թ��Ҿ�ѧ���Ե�٧�ش
        healthSlider.value = currentHealth; // ��Ѻ��ا���� Slider ����ʹ���ͧ�Ѻ��Ҿ�ѧ���Ե�Ѩ�غѹ

        if (currentHealth <= 0)
        {
            Die(); // ���¡�ѧ��ѹ����͵��
        }
    }

    void Die()
    {
        isDead = true; // ��駤����Ҽ����蹵������

        playerControl.Dead(); // ���¡�ѧ��ѹ��õ�¨ҡ PlayerControl
        Debug.Log("Player Died!");
        // �س�Ҩ�������ѧ��ѹ Respawn �����ʴ� UI ��õ��������
    }

    // �ѧ��ѹ����Ѻ����Դ����ͧ������ (�����)
    public void Respawn()
    {
        isDead = false; // ����ʶҹС�õ��
        currentHealth = maxHealth; // ���絾�ѧ���Ե
        healthSlider.value = currentHealth; // ��Ѻ��ا���� Slider
        Debug.Log("Player Respawned!");
        // �����ѧ��ѹ��� � �蹡�����¼�����仨ش�Դ����������
    }
    public void textlife(string lifetextdecrease) 
    {
        lifetext.text = lifetextdecrease;
    }
}
