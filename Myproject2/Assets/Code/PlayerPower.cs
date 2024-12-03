using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPower : MonoBehaviour
{
    public float maxPower = 100f;
    private float currentPower;
    public Slider powerSlider;
    public float powerIncreaseAmount = 10f;  // ��˹���������ͧ��ѧ�����������
    public int damagedoesadd = 1 ;
    PlayerSoundEffect SoundEffectItem;

    private List<EnemyHealth> enemies; // ��ʵ����ѵ�ٷ�����㹩ҡ
    private List<BossEnemyHealth> Bossenemies;

    void Start()
    {
        SoundEffectItem = GetComponent<PlayerSoundEffect>();
        currentPower = 0f;  // ������鹷�� 0 ���ͤ�ҷ���ͧ���
        powerSlider.maxValue = maxPower;
        powerSlider.value = currentPower;   // ��˹��������������Ѻ slider

        // �֧�ͺ�硵��ѵ�ٷ�����㹩ҡ�������ʵ�
        enemies = new List<EnemyHealth>(FindObjectsOfType<EnemyHealth>());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.RightShift)) 
        {
            damagedoesadd = 9999;
            foreach (EnemyHealth enemy in enemies)
            {

                enemy.DamagePlayer(dmg: damagedoesadd);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        enemies = new List<EnemyHealth>(FindObjectsOfType<EnemyHealth>());
        Bossenemies = new List<BossEnemyHealth>(FindObjectsOfType<BossEnemyHealth>());
        foreach (EnemyHealth enemy in enemies)
        {
            enemy.DamagePlayer(dmg: damagedoesadd);
        }
        foreach (BossEnemyHealth enemy in Bossenemies)
        {
            enemy.DamagePlayer(dmg: damagedoesadd);
        }
        if (collision.CompareTag("PlayerItemPower")) // ��Ǩ�ͺ���ⴹ����������ѧ�������
        {
            // ������ѧ�����ҷ���˹����
            currentPower += powerIncreaseAmount;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower); // �ӡѴ����������㹢ͺࢵ 0 �֧ maxPower
            powerSlider.value = currentPower; // �ѻവ slider ����ʴ���
            damagedoesadd++;

            // ���¡�� DamagePlayer �Ѻ�ѵ�����е�����ʵ�
            foreach (EnemyHealth enemy in enemies)
            {
               
                enemy.DamagePlayer(dmg : damagedoesadd);
            }
            SoundEffectItem.PlaySoundHighpitch(4);
            // �Ҩ������÷�����������ͻԴ��÷ӧҹ�ͧ�ѹ
            Destroy(collision.gameObject);
        }
        
    }
}
