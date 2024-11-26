using System.Collections;
using UnityEngine;

public class MonsterPatrol2 : MonoBehaviour
{
    public float moveSpeed = 2f;             // ความเร็วในการเดินของมอนสเตอร์
    public LayerMask groundLayer;            // เลเยอร์ของพื้นหรือแพลตฟอร์ม
    public LayerMask playerLayer;            // เลเยอร์ของผู้เล่น
    public Transform groundCheck;            // จุดตรวจจับพื้นด้านล่าง
    public Transform playerCheckFront;       // จุดตรวจจับผู้เล่นด้านหน้า (เส้นสีฟ้า)
    public Transform playerCheck;            // จุดตรวจจับผู้เล่นทั้งสองด้าน (เส้นสีเขียว)
    public Transform startpositionenemy;     // ตำแหน่งเริ่มต้นของมอนสเตอร์
    public Animator animatorenemy;
    public float detectionDistanceFront = 3f; // ระยะที่ตรวจจับผู้เล่นด้านหน้า (สีฟ้า)
    public float detectionDistance = 5f;     // ระยะที่ตรวจจับผู้เล่นทั้งสองด้าน (สีเขียว)
    public float stopDistance = 1.5f;        // ระยะที่จะหยุดเมื่อเข้าใกล้ผู้เล่น
    public float attackDuration = 1f;        // เวลาที่การโจมตีจะคงอยู่
    public float returnSpeed = 2f;           // ความเร็วในการเดินกลับไปยังตำแหน่งเริ่มต้น
    public float edgePauseTime = 1f;

    private bool movingRight = true;         // ตรวจสอบทิศทางการเคลื่อนไหว
    private Vector2 startingPosition;        // ตำแหน่งเริ่มต้นของมอนสเตอร์
    private Rigidbody2D rb;
    private bool isAttacking = false;        // ตัวแปรตรวจสอบว่ามอนสเตอร์กำลังโจมตี
    private bool isReturning = false;        // ตัวแปรตรวจสอบว่ามอนสเตอร์กำลังกลับไปยังตำแหน่งเดิม
    EnemySoundEffect effect;
    EnemyHealth  EnemyH;


    void Start()
    {
        EnemyH = GetComponent<EnemyHealth>();
        effect = GetComponent<EnemySoundEffect>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = startpositionenemy.position; // บันทึกตำแหน่งเริ่มต้นของมอนสเตอร์
    }

    void Update()
    {
        if (isAttacking)
        {
            return;
        }
        Vector2 rayDirection = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D playerHitFront = Physics2D.Raycast(playerCheckFront.position, rayDirection, detectionDistanceFront, playerLayer);
        RaycastHit2D playerHit = Physics2D.Raycast(playerCheck.position, rayDirection, detectionDistance, playerLayer);
        RaycastHit2D playerHitBack = Physics2D.Raycast(playerCheck.position, -rayDirection, detectionDistance, playerLayer);
        bool isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);
        if (!isReturning)
        {
            if (playerHitFront.collider != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerHitFront.collider.transform.position);

                if (distanceToPlayer > stopDistance)
                {
                    MoveTowardsPlayer(playerHitFront.collider.transform); // เดินเข้าหาผู้เล่น
                }
                else
                {
                    rb.velocity = Vector2.zero; // หยุดเมื่อเข้าใกล้ผู้เล่น
                    animatorenemy.SetFloat("Walkspeed", 0);
                    StartCoroutine(Attack()); // เริ่มการโจมตี
                }
            }
            // ถ้าพบผู้เล่นในระยะเส้นสีเขียว
            else if (playerHit.collider != null && isGrounded)
            {
                MoveTowardsPlayer(playerHit.collider.transform); // เดินไปหาผู้เล่นตามเส้นสีเขียว
            }
            else if (playerHitBack.collider != null && isGrounded)
            {
                MoveTowardsPlayer(playerHitBack.collider.transform); // เดินไปหาผู้เล่นที่อยู่ด้านหลัง
            }
            else
            { 
                ReturnToStartPosition();

            }
            if (!isGrounded)
            {
                ReturnToStartPosition();
            }
        }
        if (isReturning) 
        {
            ReturnToStartPosition();
        }
        


    }

    // ฟังก์ชั่นสำหรับเดินไปหาผู้เล่น
    void MoveTowardsPlayer(Transform playerTransform)
    {
        Vector2 targetPosition = new Vector2(playerTransform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        animatorenemy.SetFloat("Walkspeed", 1);

        if (transform.position.x < playerTransform.position.x && !movingRight)
        {
            Flip();
        }
        else if (transform.position.x > playerTransform.position.x && movingRight)
        {
            Flip();
        }
    }

    // ฟังก์ชั่นสำหรับการเดินกลับไปที่ตำแหน่งเริ่มต้น
    void ReturnToStartPosition()
    {
        float distanceToStart = Vector2.Distance(transform.position, startingPosition);

        if (distanceToStart > 0.1f) // ถ้ามอนสเตอร์ห่างจากตำแหน่งเริ่มต้น
        {
            isReturning = true;
            animatorenemy.SetFloat("Walkspeed", 1);
            // เช็คทิศทางการเดินและพลิกตัวมอนสเตอร์ให้ถูกต้อง
            if ((startingPosition.x < transform.position.x && movingRight) || (startingPosition.x > transform.position.x && !movingRight))
            {
                Flip();
            }

            transform.position = Vector2.MoveTowards(transform.position, startingPosition, returnSpeed * Time.deltaTime);
        }
        else
        {
            isReturning = false; // มอนสเตอร์กลับมาถึงตำแหน่งเดิมแล้ว
            animatorenemy.SetFloat("Walkspeed", 0);
        }
    }

    // ฟังก์ชั่นพลิกตัวมอนสเตอร์
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // เปลี่ยนค่าของแกน X เพื่อพลิกตัว
        transform.localScale = scale;
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
        // วาดเส้น Ray สำหรับตรวจจับพื้น (สีแดง)
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
}
