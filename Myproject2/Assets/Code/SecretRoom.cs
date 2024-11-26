using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretRoom : MonoBehaviour
{
    public Transform positionsecret;
    public GameObject transition;
    private PlayerControl playerteleport;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        playerteleport = FindAnyObjectByType<PlayerControl>();
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(playertosecret());
        }
    }
    
    public IEnumerator playertosecret()
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        playerteleport.transform.position = positionsecret.transform.position;
        yield return new WaitForSeconds(2f);
        transition.SetActive(false);
    }
}
