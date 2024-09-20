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
    public float knockbackDuration = 0.2f;  // ระยะเวลาที่ผู้เล่นจะถูกกระเด็น

    bool isGrounded;
    bool isKnockedBack = false;  // ตรวจสอบว่าผู้เล่นอยู่ในสถานะถูกกระเด็นหรือไม่

    void Update()
    {
        if (!isKnockedBack) // ถ้าไม่อยู่ในสถานะกระเด็น ให้สามารถควบคุมการเคลื่อนไหวได้
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            animator.SetFloat("Speed", Mathf.Abs(moveInput));

            // การกระโดด
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = Vector2.up * jumpForce;
            }

            // กดปุ่ม E เพื่อใช้งานอนิเมชั่น Attack
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

            // การหันหน้าตามทิศทางที่เดิน
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
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized; // หาทิศทางการกระเด็นถอยไป
            StartCoroutine(Knockback(knockbackDirection));  // เรียกใช้ Coroutine เพื่อจัดการการกระเด็น
        }
    }

    public IEnumerator Knockback(Vector2 knockbackDirection)
    {
        isKnockedBack = true;  // ป้องกันไม่ให้ผู้เล่นเคลื่อนที่ขณะกระเด็น
        rb.velocity = Vector2.zero;  // รีเซ็ตความเร็วก่อนกระเด็น
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // เพิ่มแรงกระเด็นถอยไป

        yield return new WaitForSeconds(knockbackDuration);  // รอให้การกระเด็นสิ้นสุดลง

        isKnockedBack = false;  // อนุญาตให้ผู้เล่นกลับมาเคลื่อนไหวได้
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
