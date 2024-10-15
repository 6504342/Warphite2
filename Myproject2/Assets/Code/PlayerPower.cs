using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPower : MonoBehaviour
{
    public float maxPower = 100f;
    private float currentPower;
    public Slider powerSlider;
    public float powerIncreaseAmount = 10f;  // กำหนดค่าเพิ่มของพลังเมื่อเก็บไอเทม

    private List<EnemyHealth> enemies; // ลิสต์เก็บศัตรูทั้งหมดในฉาก

    void Start()
    {
        currentPower = 0f;  // เริ่มต้นที่ 0 หรือค่าที่ต้องการ
        powerSlider.maxValue = maxPower;
        powerSlider.value = currentPower;   // กำหนดค่าเริ่มต้นให้กับ slider

        // ดึงออบเจ็กต์ศัตรูทั้งหมดในฉากมาเก็บในลิสต์
        enemies = new List<EnemyHealth>(FindObjectsOfType<EnemyHealth>());
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerItemPower")) // ตรวจสอบว่าโดนไอเทมเพิ่มพลังหรือไม่
        {
            // เพิ่มพลังตามค่าที่กำหนดไว้
            currentPower += powerIncreaseAmount;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower); // จำกัดค่าให้อยู่ในขอบเขต 0 ถึง maxPower
            powerSlider.value = currentPower; // อัปเดต slider ให้แสดงผล

            // เรียกใช้ DamagePlayer กับศัตรูแต่ละตัวในลิสต์
            foreach (EnemyHealth enemy in enemies)
            {
                enemy.DamagePlayer();
            }

            // อาจเพิ่มการทำลายไอเทมหรือปิดการทำงานของมัน
            Destroy(collision.gameObject);
        }
    }
}
