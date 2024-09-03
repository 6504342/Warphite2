using UnityEngine;
using TMPro; // ����� TextMeshPro

public class DamagePopup : MonoBehaviour
{
    public float disappearSpeed = 3f;
    public float moveSpeed = 1f;
    public float lifeTime = 1f;

    [SerializeField] Renderer Hittext;
    private Color textColor;

    void Start()
    {
        Hittext = GetComponent<Renderer>();
        textColor = Hittext.material.color;
        Destroy(gameObject, lifeTime); // ����� object ��ѧ�ҡ����ѹ����������˹��
    }

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0); // ������ͤ�������͹�����
        textColor.a -= disappearSpeed * Time.deltaTime; // ����������ͤ�������ʧ���
        Hittext.material.color = textColor;

        if (textColor.a <= 0)
        {
            Destroy(gameObject); // ����� object ������ѹ����ʧ�����
        }
    }

    public void Setup(int damageAmount)
    {
        damageAmount += 1;
         // ��˹���ͤ����繤�Ҵ����
    }
}
