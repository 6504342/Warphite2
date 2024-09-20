using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Rigidbody2D rb;
    public Animator animator;
    [SerializeField] public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;  // �������ҷ������蹨ж١�����

    bool isGrounded;
    bool isKnockedBack = false;  // ��Ǩ�ͺ��Ҽ����������ʶҹж١������������

    void Update()
    {
        if (!isKnockedBack) // �����������ʶҹС���� �������ö�Ǻ����������͹�����
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            animator.SetFloat("Speed", Mathf.Abs(moveInput));

            // ��á��ⴴ
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = Vector2.up * jumpForce;
            }

            // ������ E ������ҹ͹������ Attack
            if (Input.GetKeyDown(KeyCode.E) && isGrounded == true)
            {
                int randomNumber = Random.Range(1, 3);
                if (randomNumber == 1)
                {
                    animator.SetBool("Attack1", true);
                    StartCoroutine(ComboTimer());
                }
                else
                {
                    animator.SetBool("Attack2", true);
                    StartCoroutine(ComboTimer());
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && isGrounded == false)
            {
                animator.SetBool("AirAttack", true);
                StartCoroutine(ComboTimer());
            }

            // ����ѹ˹�ҵ����ȷҧ����Թ
            if (moveInput != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            isGrounded = true;
            animator.SetBool("isGround", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            isGrounded = false;
            animator.SetBool("isGround", false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAttack"))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized; // �ҷ�ȷҧ��á���繶���
            StartCoroutine(Knockback(knockbackDirection));  // ���¡�� Coroutine ���ͨѴ��á�á����
        }
    }

    public IEnumerator Knockback(Vector2 knockbackDirection)
    {
        isKnockedBack = true;  // ��ͧ�ѹ���������������͹��袳С����
        rb.velocity = Vector2.zero;  // ���絤������ǡ�͹�����
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // �����ç����繶���

        yield return new WaitForSeconds(knockbackDuration);  // ������á��������شŧ

        isKnockedBack = false;  // ͹حҵ�������蹡�Ѻ������͹�����
    }

    public IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("AirAttack", false);
        StopCoroutine(ComboTimer());
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
}
