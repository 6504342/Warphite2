using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public Rigidbody2D rb;
    public Animator animator;
    Coroutine attackCoroutine;

    bool isGrounded;

    void Update()
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
                ComboTimer();
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
            ComboTimer();
        }

        // การหันหน้าตามทิศทางที่เดิน
        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGround", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("isGround", false);
        }
    }
    public IEnumerator  ComboTimer()
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
