using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class interectobject : MonoBehaviour
{
    public Transform Item;
    public Transform Particleeffect;
    public AudioSource breaksound;
    public GameObject itemdrop;
    private bool canAttack = true;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack") && canAttack == true) 
        {
             breaksound.pitch = Random.Range(0.8f,1f);
             breaksound.Play();
             Item.gameObject.SetActive(false);
             Particleeffect.gameObject.SetActive(true);
             canAttack = false;
            if (itemdrop != null)
            {
                StartCoroutine(ItemSpawn());
            }
            Invoke("destroyobject", 1f);
        }
    }
    public void destroyobject() 
    {
        Object.Destroy(gameObject);
    }
    public IEnumerator ItemSpawn() 
    {
        yield return new WaitForSeconds(0.5f);
        Vector2 itemposition = gameObject.transform.position;
        Instantiate(itemdrop, itemposition, transform.rotation);
        Debug.Log("work");
    }
}
