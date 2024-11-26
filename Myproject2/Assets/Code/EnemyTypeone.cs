using System.Collections;
using UnityEngine;

public class MonsterPatrol : MonoBehaviour
{
    public float moveSpeed = 2f;            // ��������㹡���Թ�ͧ�͹�����
    public LayerMask groundLayer;           // �������ͧ��������ŵ�����
    public LayerMask playerLayer;           // �������ͧ������
    public Transform groundCheck;           // ���˹觢ͧ�ش��Ǩ�ͺ���
    public Transform playerCheckFront;      // �ش��Ǩ�Ѻ�����蹴�ҹ˹�� (����տ��)
    public Transform playerCheck;           // �ش��Ǩ�Ѻ�����蹷���ͧ��ҹ (���������)
    public Animator animatorenemy;
    public float detectionDistanceFront = 3f; // ���з���Ǩ�Ѻ�����蹴�ҹ˹�� (�տ��)
    public float detectionDistance = 5f;    // ���з���Ǩ�Ѻ�����蹷���ͧ��ҹ (������)
    public float stopDistance = 1.5f;       // ���з�����ش����������������
    public float edgePauseTime = 1f;        // ���ҷ�����ش��͹����¹��ȷҧ����Ͷ֧�ͺ
    public float attackDuration = 1f;       // ���ҷ�������ըФ�����

    private bool movingRight = true;        // ����õ�Ǩ�ͺ�������͹������/���
    private Rigidbody2D rb;
    private bool isAtEdge = false;          // ��Ǩ�ͺ����͹�����������ͺ�������
    private bool isWaiting = false;         // ��Ǩ�ͺ����͹�������ѧ���������
    private bool isAttacking = false;       // ����õ�Ǩ�ͺ����͹�������ѧ����
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
        // ����͹�������ѧ���� �����ش�������͹���
        if (isAttacking)
        {
            rb.velocity = Vector2.zero;
            return;  // �͡�ҡ�ѧ��ѹ update �����������͹���������ҧ��������ҧ����
        }

        // ��Ǩ�ͺ����ռ��������������������� (�� Raycast)
        Vector2 rayDirection = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D playerHitFront = Physics2D.Raycast(playerCheckFront.position, rayDirection, detectionDistanceFront, playerLayer);
        RaycastHit2D playerHit = Physics2D.Raycast(playerCheck.position, rayDirection, detectionDistance, playerLayer);

        // ��Ǩ�ͺ����վ�������ҹ��ҧ������� (�� Raycast)
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
                rb.velocity = Vector2.zero; // ��ش����������������
                animatorenemy.SetFloat("Walkspeed", 0);
                StartCoroutine(Attack()); // ������������
            }
            if (distanceToPlayer == groundLayer)
            {
                Flip();
            }
        }
        else if (playerHit.collider != null)
        {
            MoveTowardsPlayer(playerHit.collider.transform); // �Թ��Ҽ����蹵�����������
        }
        else
        {
            if (!isWaiting)
            {
                Patrol();
            }
        }

        // �������վ�������ش�Թ���˹�ǧ��������͹������¹��ȷҧ
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

    // �ѧ��������Ѻ�Թ��Ҽ�����
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

    // �ѧ��������Ѻ����ԹẺ���� (�Թ���)
    void Patrol()
    {
        bool isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, groundLayer);

        if (!isGrounded && !isAtEdge)
        {
            StartCoroutine(WaitBeforeFlip());
        }

        if (!isAtEdge) // ����͹������͹��������������ͺ
        {
            rb.velocity = new Vector2(moveSpeed * (movingRight ? 1 : -1), rb.velocity.y);
        }
    }

    // �ѧ��������Ѻ����¹��ȷҧ�������͹���
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    // �ѧ����˹�ǧ��������͹������¹��ȷҧ������͹�����֧�ͺ
    IEnumerator WaitBeforeFlip()
    {

        isAtEdge = true;   // �͡����͹�����֧�ͺ����
        isWaiting = true;  // �͡����͹�������ѧ��
        rb.velocity = Vector2.zero;  // ��ش�������͹���
        yield return new WaitForSeconds(edgePauseTime); // ˹�ǧ���ҵ������˹�
        Flip();   // ����¹��ȷҧ
        isWaiting = false; // ��ش�����
        isAtEdge = false;  // ���������ͺ����
    }

    // �ѧ���蹨Ѵ��á������
    IEnumerator Attack()
    {
        
        isAttacking = true;  // �͹�������ѧ����
        animatorenemy.SetBool("EnemyAttack", true); // ������������
        EnemyH.AttackEnemySound();
        rb.velocity = Vector2.zero;  // ��ش�������͹��������ҧ����
        yield return new WaitForSeconds(attackDuration); // �ͨ��������ҡ�����ը����
        animatorenemy.SetBool("EnemyAttack", false); // ���������
        isAttacking = false; // ������������
    }

    // �Ҵ Raycast � Scene ���ͪ��µ�Ǩ�ͺ
    private void OnDrawGizmos()
    {
        // ��鹵�Ǩ�ͺ��� (��ᴧ)
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 1f);

        // �Ҵ��� Ray ����Ѻ��Ǩ�Ѻ�����蹴�ҹ˹�� (�տ��)
        Vector3 rayDirectionFront = movingRight ? Vector3.right : Vector3.left;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheckFront.position, playerCheckFront.position + rayDirectionFront * detectionDistanceFront);

        // �Ҵ��� Ray ����Ѻ��Ǩ�Ѻ�����蹷�駴�ҹ˹����д�ҹ��ѧ (������)
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + Vector3.right * detectionDistance);  // ��ҹ���
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + Vector3.left * detectionDistance);   // ��ҹ����
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }
}
