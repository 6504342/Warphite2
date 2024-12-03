using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecretRoom : MonoBehaviour
{
    public Transform positionsecret;
    private GameObject transition;
    private PlayerControl playerteleport;

    void Start()
    {

    }

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
            //transition.SetActive(true);
            yield return new WaitForSeconds(2f);
            playerteleport.transform.position = positionsecret.transform.position;
            yield return new WaitForSeconds(2f);
            //transition.SetActive(false);

    }
}
