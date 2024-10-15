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

    private List<EnemyHealth> enemies; // ��ʵ����ѵ�ٷ�����㹩ҡ

    void Start()
    {
        currentPower = 0f;  // ������鹷�� 0 ���ͤ�ҷ���ͧ���
        powerSlider.maxValue = maxPower;
        powerSlider.value = currentPower;   // ��˹��������������Ѻ slider

        // �֧�ͺ�硵��ѵ�ٷ�����㹩ҡ�������ʵ�
        enemies = new List<EnemyHealth>(FindObjectsOfType<EnemyHealth>());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerItemPower")) // ��Ǩ�ͺ���ⴹ����������ѧ�������
        {
            // ������ѧ�����ҷ���˹����
            currentPower += powerIncreaseAmount;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower); // �ӡѴ����������㹢ͺࢵ 0 �֧ maxPower
            powerSlider.value = currentPower; // �ѻവ slider ����ʴ���

            // ���¡�� DamagePlayer �Ѻ�ѵ�����е�����ʵ�
            foreach (EnemyHealth enemy in enemies)
            {
                enemy.DamagePlayer();
            }

            // �Ҩ������÷�����������ͻԴ��÷ӧҹ�ͧ�ѹ
            Destroy(collision.gameObject);
        }
    }
}
