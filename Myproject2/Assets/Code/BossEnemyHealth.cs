using UnityEngine;
using System.Collections;

public class BossEnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;  // ��Ҿ�ѧ���Ե�٧�ش�ͧ�ѵ��
    private float currentHealth;   // ��Ҿ�ѧ���Ե�Ѩ�غѹ�ͧ�ѵ��
    public float deadtime = 3f;
    public Animator animatorenemy;
    public AudioSource audioSource;
    public AudioClip[] soundEffects;
    public float knockbackForce = 5f; // �ç�����㹡�á����
    public int damagedoes = 1;
    [SerializeField] public GameObject slashing;
    private GameObject wingame;
    private Rigidbody2D rb;
    private bool isDead = false;  // ����õ�Ǩ�ͺ����ѵ�ٵ�������ѧ

    void Start()
    {
        currentHealth = maxHealth; // ��˹���Ҿ�ѧ���Ե�������
        rb = GetComponent<Rigidbody2D>(); // ��ҧ�ԧ�֧ Rigidbody2D �ͧ�ѵ��

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack") && !isDead) // ��Ǩ�ͺ���ⴹ���ըҡ Player �������
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
        currentHealth -= damage; // Ŵ��Ҿ�ѧ���Ե�������������·�����Ѻ
        maxHealth -= damage;
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

        animatorenemy.SetBool("Dead", true); // ������͹����ѹ��õ��
        rb.velocity = Vector2.zero; // ��ش�������͹���ͧ�ѵ��
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
