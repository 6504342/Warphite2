using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Bossdata : MonoBehaviour
{
    int bosshealth = 0;
    private BossEnemyHealth beh;
    public void BossHealthReduce() 
    {
        bosshealth += 50;
        Debug.Log(bosshealth);
    }
    public void BossHealthAffect() 
    {
        beh = FindAnyObjectByType<BossEnemyHealth>();
        beh.TakeDamage(damage: bosshealth);
        Debug.Log(bosshealth);
    }
}
