using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class interectobject : MonoBehaviour
{
    public Transform Item;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack")) 
        {
             Object.Destroy(gameObject);
        }
    }
}
