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
    public int damagedoesadd = 1 ;
    PlayerSoundEffect SoundEffectItem;

    private List<EnemyHealth> enemies; // ลิสต์เก็บศัตรูทั้งหมดในฉาก
    private List<BossEnemyHealth> Bossenemies;

    void Start()
    {
        SoundEffectItem = GetComponent<PlayerSoundEffect>();
        currentPower = 0f;  // เริ่มต้นที่ 0 หรือค่าที่ต้องการ
        powerSlider.maxValue = maxPower;
        powerSlider.value = currentPower;   // กำหนดค่าเริ่มต้นให้กับ slider

        // ดึงออบเจ็กต์ศัตรูทั้งหมดในฉากมาเก็บในลิสต์
        enemies = new List<EnemyHealth>(FindObjectsOfType<EnemyHealth>());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.RightShift)) 
        {
            damagedoesadd = 9999;
            foreach (EnemyHealth enemy in enemies)
            {

                enemy.DamagePlayer(dmg: damagedoesadd);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        enemies = new List<EnemyHealth>(FindObjectsOfType<EnemyHealth>());
        Bossenemies = new List<BossEnemyHealth>(FindObjectsOfType<BossEnemyHealth>());
        foreach (EnemyHealth enemy in enemies)
        {
            enemy.DamagePlayer(dmg: damagedoesadd);
        }
        foreach (BossEnemyHealth enemy in Bossenemies)
        {
            enemy.DamagePlayer(dmg: damagedoesadd);
        }
        if (collision.CompareTag("PlayerItemPower")) // ตรวจสอบว่าโดนไอเทมเพิ่มพลังหรือไม่
        {
            // เพิ่มพลังตามค่าที่กำหนดไว้
            currentPower += powerIncreaseAmount;
            currentPower = Mathf.Clamp(currentPower, 0, maxPower); // จำกัดค่าให้อยู่ในขอบเขต 0 ถึง maxPower
            powerSlider.value = currentPower; // อัปเดต slider ให้แสดงผล
            damagedoesadd++;

            // เรียกใช้ DamagePlayer กับศัตรูแต่ละตัวในลิสต์
            foreach (EnemyHealth enemy in enemies)
            {
               
                enemy.DamagePlayer(dmg : damagedoesadd);
            }
            SoundEffectItem.PlaySoundHighpitch(4);
            // อาจเพิ่มการทำลายไอเทมหรือปิดการทำงานของมัน
            Destroy(collision.gameObject);
        }
        
    }
}
