using UnityEngine;
using TMPro; // ถ้าใช้ TextMeshPro

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
        Destroy(gameObject, lifeTime); // ทำลาย object หลังจากที่มันอยู่เป็นเวลาหนึ่ง
    }

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0); // ทำให้ข้อความเคลื่อนที่ขึ้น
        textColor.a -= disappearSpeed * Time.deltaTime; // ค่อยๆทำให้ข้อความโปร่งแสงขึ้น
        Hittext.material.color = textColor;

        if (textColor.a <= 0)
        {
            Destroy(gameObject); // ทำลาย object เมื่อมันโปร่งแสงจนหมด
        }
    }

    public void Setup(int damageAmount)
    {
        damageAmount += 1;
         // กำหนดข้อความเป็นค่าดาเมจ
    }
}
