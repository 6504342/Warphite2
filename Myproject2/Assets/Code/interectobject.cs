using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class interectobject : MonoBehaviour
{
    public Transform Item;
    public AudioSource breaksound;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack")) 
        {
             breaksound.Play();
             Item.gameObject.SetActive(false);
             Invoke("destroyobject", 1f);
        }
    }
    public void destroyobject() 
    {
        Object.Destroy(gameObject);
    }
}
