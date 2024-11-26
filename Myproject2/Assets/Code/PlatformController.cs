using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Collider2D platformCollider; // Collider �ͧ Platform

    void Start()
    {
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        // ��Ǩ�ͺ��ҵ���Фá��ѧ����͹����� (���������᡹ y �ҡ���� 0) �������
        if (collision.CompareTag("Player") && collision.attachedRigidbody.velocity.y > 0)
        {
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), platformCollider, true);
            Debug.Log("work");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �׹����繻���������͡�ҡ��ê��ѹ
            Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), platformCollider, false);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // ��ҡ� "S" ������ŧ���
            StartCoroutine(DisableCollisionTemporarily());
        }
    }

    IEnumerator DisableCollisionTemporarily()
    {
        // �Դ��ê����Ǥ�������Դ��Ѻ��ѧ�ҡ 0.5 �Թҷ�
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), platformCollider, true);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), platformCollider, false);
    }
}
