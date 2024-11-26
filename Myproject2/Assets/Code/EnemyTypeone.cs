using System.Collections;
using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;            // ความเร็วในการเดินของมอนสเตอร์
    public LayerMask groundLayer;           // เลเยอร์ของพื้นหรือแพลตฟอร์ม
    public LayerMask playerLayer;           // เลเยอร์ของผู้เล่น
    public Transform groundCheck;           // ตำแหน่งของจุดตรวจสอบพื้น
    public Transform playerCheckFront;      // จุดตรวจจับผู้เล่นด้านหน้า (เส้นสีฟ้า)
    public Transform playerCheck;           // จุดตรวจจับผู้เล่นทั้งสองด้าน (เส้นสีเขียว)
    public Animator animatorenemy;
    public float detectionDistanceFront = 3f; // ระยะที่ตรวจจับผู้เล่นด้านหน้า (สีฟ้า)
    public float detectionDistance = 5f;    // ระยะที่ตรวจจับผู้เล่นทั้งสองด้าน (สีเขียว)
    public float stopDistance = 1.5f;       // ระยะที่จะหยุดเมื่อเข้าใกล้ผู้เล่น
    public float edgePauseTime = 1f;        // เวลาที่จะหยุดก่อนเปลี่ยนทิศทางเมื่อถึงขอบ
    public float attackDuration = 1f;       // เวลาที่การโจมตีจะคงอยู่

    private bool movingRight = true;        // ตัวแปรตรวจสอบการเคลื่อนที่ซ้าย/ขวา
    private Rigidbody2D rb;
    private bool isAtEdge = false;          // ตรวจสอบว่ามอนสเตอร์อยู่ที่ขอบหรือไม่
    private bool isWaiting = false;         // ตรวจสอบว่ามอนสเตอร์กำลังรอหรือไม่
    private bool isAttacking = false;       // ตัวแปรตรวจสอบว่ามอนสเตอร์กำลังโจมตี
    EnemySoundEffect effect;
    EnemyHealth EnemyH;

    void Start()
    {
        EnemyH = GetComponent<EnemyHealth>();
        effect = GetComponent<EnemySoundEffect>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ถ้ามอนสเตอร์กำลังโจมตี ให้หยุดการเคลื่อนไหว
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;  // ออกจากฟังก์ชัน update เพื่อไม่ให้มอนสเตอร์ทำอย่างอื่นระหว่างโจมตี
        }

        // ตรวจสอบว่ามีผู้เล่นอยู่ในระยะหรือไม่ (ใช้ Raycast)
        Vector2 rayDirection = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D playerHitFront = Physics2D.Raycast(playerCheckFront.position, rayDirection, detectionDistanceFront, playerLayer);
        RaycastHit2D playerHit = Physics2D.Raycast(playerCheck.position, rayDirection, detectionDistance, playerLayer);

        // ตรวจสอบว่ามีพื้นอยู่ด้านล่างหรือไม่ (ใช้ Raycast)
        bool isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);

        if (playerHitFront.collider != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerHitFront.collider.transform.position);

            if (distanceToPlayer > stopDistance)
            {
                MoveTowardsPlayer(playerHitFront.collider.transform);
            }
            else
            {
                rb.velocity = Vector2.zero; // หยุดเมื่อเข้าใกล้ผู้เล่น
                animatorenemy.SetFloat("Walkspeed", 0);
                StartCoroutine(Attack()); // เริ่มการโจมตี
            }
            if (distanceToPlayer == groundLayer)
            {
                Flip();
            }
        }
        else if (playerHit.collider != null)
        {
            MoveTowardsPlayer(playerHit.collider.transform); // เดินไปหาผู้เล่นตามเส้นสีเขียว
        }
        else
        {
            if (!isWaiting)
            {
                Patrol();
            }
        }

        // ถ้าไม่มีพื้นให้หยุดเดินและหน่วงเวลาไว้ก่อนจะเปลี่ยนทิศทาง
        if (!isGrounded && !isAtEdge)
        {
            StartCoroutine(WaitBeforeFlip());
        }

        if (rb.velocity == Vector2.zero)
        {
            animatorenemy.SetFloat("Walkspeed", 0);
        }
        else
        {
            animatorenemy.SetFloat("Walkspeed", 1);
        }
    }

    // ฟังก์ชั่นสำหรับเดินไปหาผู้เล่น
    void MoveTowardsPlayer(Transform playerTransform)
    {
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position.x < playerTransform.position.x && !movingRight)
        {
            Flip();
        }
        else if (transform.position.x > playerTransform.position.x && movingRight)
        {
            Flip();
        }
    }

    // ฟังก์ชั่นสำหรับการเดินแบบปกติ (เดินไปมา)
    void Patrol()
    {
        bool isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);

        if (!isGrounded && !isAtEdge)
        {
            StartCoroutine(WaitBeforeFlip());
        }

        if (!isAtEdge) // เคลื่อนที่ถ้ามอนสเตอร์ไม่อยู่ที่ขอบ
        {
            rb.velocity = new Vector2(moveSpeed * (movingRight ? 1 : -1), rb.velocity.y);
        }
    }

    // ฟังก์ชั่นสำหรับเปลี่ยนทิศทางการเคลื่อนที่
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // ฟังก์ชั่นหน่วงเวลาไว้ก่อนจะเปลี่ยนทิศทางเมื่อมอนสเตอร์ถึงขอบ
    IEnumerator WaitBeforeFlip()
    {

        isAtEdge = true;   // บอกว่ามอนสเตอร์ถึงขอบแล้ว
        isWaiting = true;  // บอกว่ามอนสเตอร์กำลังรอ
        rb.velocity = Vector2.zero;  // หยุดการเคลื่อนที่
        yield return new WaitForSeconds(edgePauseTime); // หน่วงเวลาตามที่กำหนด
        Flip();   // เปลี่ยนทิศทาง
        isWaiting = false; // หยุดการรอ
        isAtEdge = false;  // ไม่อยู่ที่ขอบแล้ว
    }

    // ฟังก์ชั่นจัดการการโจมตี
    IEnumerator Attack()
    {
        
        isAttacking = true;  // มอนสเตอร์กำลังโจมตี
        animatorenemy.SetBool("EnemyAttack", true); // เริ่มการโจมตี
        EnemyH.AttackEnemySound();
        rb.velocity = Vector2.zero;  // หยุดการเคลื่อนไหวระหว่างโจมตี
        yield return new WaitForSeconds(attackDuration); // รอจนกว่าเวลาการโจมตีจะหมด
        animatorenemy.SetBool("EnemyAttack", false); // จบการโจมตี
        isAttacking = false; // โจมตีเสร็จแล้ว
    }

    // วาด Raycast ใน Scene เพื่อช่วยตรวจสอบ
    private void OnDrawGizmos()
    {
        // เส้นตรวจสอบพื้น (สีแดง)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 1f);

        // วาดเส้น Ray สำหรับตรวจจับผู้เล่นด้านหน้า (สีฟ้า)
        Vector3 rayDirectionFront = movingRight ? Vector3.right : Vector3.left;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheckFront.position, playerCheckFront.position + rayDirectionFront * detectionDistanceFront);

        // วาดเส้น Ray สำหรับตรวจจับผู้เล่นทั้งด้านหน้าและด้านหลัง (สีเขียว)
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + Vector3.right * detectionDistance);  // ด้านขวา
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + Vector3.left * detectionDistance);   // ด้านซ้าย
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }
}
