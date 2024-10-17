using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnmonster : MonoBehaviour
{
    [SerializeField] GameObject monsterspawn;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        monsterspawn.SetActive(true);
        Object.Destroy(gameObject);
    }
}
