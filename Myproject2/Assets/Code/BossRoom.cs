using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject camBossRoom;
    private GameObject camlockroom;
    private GameObject maincam;
    public GameObject bossspawn;
    private Bossdata bd;

    private void Awake()
    {
        camlockroom = GameObject.FindWithTag("Finish");
        maincam = GameObject.FindWithTag("MainCamera");

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Check");
        if (collision.CompareTag("Player"))
        {
            camlockroom.SetActive(false);
            StartCoroutine(camrun());
        }
        else 
        {
            Debug.Log("not found");
        }

    }
    IEnumerator camrun() 
    {
        yield return new WaitForSeconds(0.1f);
        maincam.transform.position = camBossRoom.transform.position;
        yield return new WaitForSeconds(3f);
        bossspawn.SetActive(true);
        yield return new WaitForSeconds(1f);
        bd = GameObject.FindObjectOfType<Bossdata>();
        bd.BossHealthAffect();
        
    }

}
