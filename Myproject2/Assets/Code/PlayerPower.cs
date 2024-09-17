using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPower : MonoBehaviour
{
    public float maxPower = 100f;
    private float currentPower;
    public Slider powerSlider;
    public EnemyHealth enemyhealth;
    public float powerIncreaseAmount = 10f;  // ��˹���������ͧ��ѧ�����������

    void Start()
    {
        enemyhealth = FindObjectOfType<EnemyHealth>();
        currentPower = 0f;  // ������鹷�� 0 ���ͤ�ҷ���ͧ���
        powerSlider.maxValue = maxPower;
        powerSlider.value = currentPower;   // ��˹��������������Ѻ slider
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerItemPower")) // ��Ǩ�ͺ���ⴹ����������ѧ�������
        {
            // ������ѧ�����ҷ���˹����
            currentPower += powerIncreaseAmount;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower); // �ӡѴ����������㹢ͺࢵ 0 �֧ maxPower
            powerSlider.value = currentPower; // �ѻവ slider ����ʴ���
            enemyhealth.DamagePlayer();

            // �Ҩ������÷�����������ͻԴ��÷ӧҹ�ͧ�ѹ
            Destroy(collision.gameObject);
        }
    }
}

