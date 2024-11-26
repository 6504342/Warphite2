using System.Collections;
using UnityEngine;

public class MonsterPatrol2 : MonoBehaviour
{
    public float moveSpeed = 2f;             // ��������㹡���Թ�ͧ�͹�����
    public LayerMask groundLayer;            // �������ͧ��������ŵ�����
    public LayerMask playerLayer;            // �������ͧ������
    public Transform groundCheck;            // �ش��Ǩ�Ѻ��鹴�ҹ��ҧ
    public Transform playerCheckFront;       // �ش��Ǩ�Ѻ�����蹴�ҹ˹�� (����տ��)
    public Transform playerCheck;            // �ش��Ǩ�Ѻ�����蹷���ͧ��ҹ (���������)
    public Transform startpositionenemy;     // ���˹�������鹢ͧ�͹�����
    public Animator animatorenemy;
    public float detectionDistanceFront = 3f; // ���з���Ǩ�Ѻ�����蹴�ҹ˹�� (�տ��)
    public float detectionDistance = 5f;     // ���з���Ǩ�Ѻ�����蹷���ͧ��ҹ (������)
    public float stopDistance = 1.5f;        // ���з�����ش����������������
    public float attackDuration = 1f;        // ���ҷ�������ըФ�����
    public float returnSpeed = 2f;           // ��������㹡���Թ��Ѻ��ѧ���˹��������
    public float edgePauseTime = 1f;

    private bool movingRight = true;         // ��Ǩ�ͺ��ȷҧ�������͹���
    private Vector2 startingPosition;        // ���˹�������鹢ͧ�͹�����
    private Rigidbody2D rb;
    private bool isAttacking = false;        // ����õ�Ǩ�ͺ����͹�������ѧ����
    private bool isReturning = false;        // ����õ�Ǩ�ͺ����͹�������ѧ��Ѻ��ѧ���˹����
    EnemySoundEffect effect;
    EnemyHealth  EnemyH;


    void Start()
    {
        EnemyH = GetComponent<EnemyHealth>();
        effect = GetComponent<EnemySoundEffect>();
        rb = GetComponent<Rigidbody2D>();
        startingPosition = startpositionenemy.position; // �ѹ�֡���˹�������鹢ͧ�͹�����
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
                    MoveTowardsPlayer(playerHitFront.collider.transform); // �Թ����Ҽ�����
                }
                else
                {
                    rb.velocity = Vector2.zero; // ��ش����������������
                    animatorenemy.SetFloat("Walkspeed", 0);
                    StartCoroutine(Attack()); // ������������
                }
            }
            // ��Ҿ���������������������
            else if (playerHit.collider != null && isGrounded)
            {
                MoveTowardsPlayer(playerHit.collider.transform); // �Թ��Ҽ����蹵�����������
            }
            else if (playerHitBack.collider != null && isGrounded)
            {
                MoveTowardsPlayer(playerHitBack.collider.transform); // �Թ��Ҽ����蹷�������ҹ��ѧ
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

    // �ѧ��������Ѻ�Թ��Ҽ�����
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

    // �ѧ��������Ѻ����Թ��Ѻ价����˹��������
    void ReturnToStartPosition()
    {
        float distanceToStart = Vector2.Distance(transform.position, startingPosition);

        if (distanceToStart > 0.1f) // ����͹�������ҧ�ҡ���˹��������
        {
            isReturning = true;
            animatorenemy.SetFloat("Walkspeed", 1);
            // �礷�ȷҧ����Թ��о�ԡ����͹��������١��ͧ
            if ((startingPosition.x < transform.position.x && movingRight) || (startingPosition.x > transform.position.x && !movingRight))
            {
                Flip();
            }

            transform.position = Vector2.MoveTowards(transform.position, startingPosition, returnSpeed * Time.deltaTime);
        }
        else
        {
            isReturning = false; // �͹������Ѻ�Ҷ֧���˹��������
            animatorenemy.SetFloat("Walkspeed", 0);
        }
    }

    // �ѧ���蹾�ԡ����͹�����
    void Flip()
    {
        movingRight = !movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1; // ����¹��Ңͧ᡹ X ���;�ԡ���
        transform.localScale = scale;
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
        // �Ҵ��� Ray ����Ѻ��Ǩ�Ѻ��� (��ᴧ)
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
}
