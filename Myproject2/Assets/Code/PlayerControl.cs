using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 7f;
    public float jumpForce = 7f;
    public float dashDuration = 0.5f;
    public Transform checkpoint;
    public Rigidbody2D rb;
    public Animator animator;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;  // �������ҷ������蹨ж١�����
    public float RespawnTime = 0.5f;
    public PlayerHealth died;
    [SerializeField] GameObject transition;



    bool canattack = true;
    bool isDashing = true;
    bool isGrounded;
    bool isMoving = true;
    bool isKnockedBack = false;  // ��Ǩ�ͺ��Ҽ����������ʶҹж١������������

    private void Start()
    {
        died = GetComponent<PlayerHealth>();
    }
    void Update()
    {
        if (!isKnockedBack && isMoving == true)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            animator.SetFloat("Speed", Mathf.Abs(moveInput));

            // ��á��ⴴ
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = Vector2.up * jumpForce;
            }
            if (Input.GetKeyDown(KeyCode.L)) 
            {
                moveSpeed = 15f;
            }

            // ������ E ������ҹ͹������ Attack
            if (Input.GetKeyDown(KeyCode.E) && isGrounded && canattack == true)
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
                if (canattack == true)
                {
                    animator.SetBool("AirAttack", true);
                    StartCoroutine(ComboTimer());
                }
            }

            // ����ѹ˹�ҵ����ȷҧ����Թ
            if (moveInput != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) && isDashing == true)
            {
                StartCoroutine(Dash());
            } 
        }
    }
    private IEnumerator Dash()
    {
        isDashing = false;          // ��駤����ҡ��ѧ���
        moveSpeed += dashSpeed;    // �����������Ǿ��
        animator.speed = 3;

        yield return new WaitForSeconds(dashDuration);  // �ͨ����Ҩо�觤ú��������

        moveSpeed -= dashSpeed;    // ��Ѻ价��������ǻ���        // ��駤�������ش�������
        animator.speed = 1.5f;
        yield return new WaitForSeconds(1f);
        isDashing = true;
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
        canattack = false;
        yield return new WaitForSeconds(0.3f);
        canattack = true;
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("AirAttack", false);
        StopCoroutine(ComboTimer());
    }

    public void Dead()
    {
        rb.velocity = Vector2.zero;
        isMoving = false;
        animator.SetTrigger("Dead");
        StartCoroutine(Respawn());
    }
    public IEnumerator Respawn() 
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(RespawnTime);
        died.Respawn();
        animator.SetTrigger("Dead");
        rb.position = checkpoint.gameObject.transform.position;
        isMoving = true;
        yield return new WaitForSeconds(RespawnTime);
        transition.SetActive(false);
    }
}
