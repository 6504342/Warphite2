using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 7f;
    public float jumpForce = 7f;
    public float dashDuration = 0.5f;
    public Rigidbody2D rb;
    public Animator animator;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;  // ระยะเวลาที่ผู้เล่นจะถูกกระเด็น
    public float RespawnTime = 0.5f;
    public PlayerHealth died;
    [SerializeField] GameObject transition;
    private Vector2 currentRespawnPoint;  // ตำแหน่งการเกิดใหม่ปัจจุบัน
    public Vector2 defaultRespawnPoint;   // ตำแหน่งเริ่มต้น
    PlayerSoundEffect effect;
    public GameObject[] slashing;
    public LayerMask enemyLayer;
    public GameObject WINGAME;
    private int lifeplayer = 5;
    private BossEnemyHealth bossreset;
    private BossRoom camcomeback;
    [SerializeField] GameObject deadimage;

    bool canattack = true;
    bool isDashing = true;
    bool isGrounded;
    bool isMoving = true;
    bool isKnockedBack = false;  // ตรวจสอบว่าผู้เล่นอยู่ในสถานะถูกกระเด็นหรือไม่

    private void Start()
    {
        
        effect = GetComponent<PlayerSoundEffect>();
        died = GetComponent<PlayerHealth>();
        currentRespawnPoint = defaultRespawnPoint;
    }

    void Update()
    {
        if (!isKnockedBack && isMoving == true)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

            animator.SetFloat("Speed", Mathf.Abs(moveInput));

            // การกระโดด
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = Vector2.up * jumpForce;
                effect.PlaySoundHighpitch(3);
            }
            if (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.RightShift)) 
            {
                moveSpeed = 15f;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                rb.gravityScale = 5f;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                rb.gravityScale = 1f;
            }

            // กดปุ่ม E เพื่อใช้งานอนิเมชั่น Attack
            if (Input.GetKeyDown(KeyCode.E) && isGrounded && canattack == true && rb.velocity == Vector2.zero)
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
            if (Input.GetKeyDown(KeyCode.M) && Input.GetKey(KeyCode.RightShift)) 
            {
                Dead();
            }

            // การหันหน้าตามทิศทางที่เดิน
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
        isDashing = false;          // ตั้งค่าว่ากำลังพุ่ง
        moveSpeed += dashSpeed;    // เพิ่มความเร็วพุ่ง
        animator.speed = 3;

        yield return new WaitForSeconds(dashDuration);  // รอจนกว่าจะพุ่งครบระยะเวลา

        moveSpeed -= dashSpeed;    // กลับไปที่ความเร็วปกติ
        animator.speed = 1.5f;
        yield return new WaitForSeconds(1f);
        isDashing = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
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
            effect.PlaySoundHighpitch(2);

            Vector2 Collisionplayer = gameObject.transform.position;
            GameObject slashAtplayer = Instantiate(slashing[3], Collisionplayer, transform.rotation);
            Destroy(slashAtplayer, 1.0f);
        }
        if (collision.CompareTag("Respawn"))
        {
            // บันทึกตำแหน่ง Respawn เมื่อชนกับจุด Respawn Point
            currentRespawnPoint = collision.transform.position;
        }
        if (collision.CompareTag("Wingame"))
        {
            WINGAME.SetActive(true);
        }
        if (((1 << collision.gameObject.layer) & enemyLayer) != 0)
        {
            Vector2 collisionPoint = collision.transform.position;
            int slashingeffect = Random.Range(0, 3);
            GameObject slashingInstance = Instantiate(slashing[slashingeffect], collisionPoint, transform.rotation);

            if (transform.localScale.x < 0)
            {
                slashingInstance.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            Destroy(slashingInstance, 1.0f);
            effect.PlaySoundHighpitch(1);
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
        canattack = false;
        effect.PlaySoundLowpitch(0);
        yield return new WaitForSeconds(0.3f);
        canattack = true;
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
        animator.SetBool("AirAttack", false);
    }

    public void Dead()
    {
        rb.velocity = Vector2.zero;
        isMoving = false;
        animator.SetTrigger("Dead");
        bossreset = FindFirstObjectByType<BossEnemyHealth>();
        if (bossreset  != null )
         {
            bossreset.IsDead();
        }
    
        StartCoroutine(Respawn());
    }
    
    public IEnumerator Respawn() 
    {
        transition.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        transition.SetActive(true);
        lifeplayer--;
        died.textlife(lifetextdecrease: "x " + lifeplayer);
        yield return new WaitForSeconds(RespawnTime);
        if (lifeplayer == 0) 
        {
            deadimage.gameObject.SetActive(true);
            isMoving = false;
            yield break;
        }
        camcomeback = FindFirstObjectByType<BossRoom>();
        if (camcomeback != null) 
        {
            camcomeback.camback();
        }
        died.Respawn();
        animator.SetTrigger("Dead");
        transform.position = currentRespawnPoint;
        isMoving = true;
        yield return new WaitForSeconds(RespawnTime);
        transition.SetActive(false);
    }
    public IEnumerator TransitionPlayer() 
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(4f);
        transition.SetActive(false);
    }
}

