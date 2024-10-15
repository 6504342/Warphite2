using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class changetosave : MonoBehaviour
{
    public Transform Playerspawnpoint;
    public GameObject transition;
    void Awake()
    {
        // ตรวจสอบว่ามี GameObject นี้อยู่แล้วหรือยัง เพื่อป้องกันการซ้ำซ้อน
        if (FindObjectsOfType<changetosave>().Length > 1)
        {
            Destroy(gameObject); // ถ้ามีมากกว่า 1 อัน ให้ทำลาย GameObject นี้เพื่อป้องกันการซ้ำซ้อน
        }
        else
        {
            DontDestroyOnLoad(gameObject); // ทำให้ GameObject นี้ไม่ถูกทำลายเมื่อเปลี่ยน Scene
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(ChangeToLevel2());
        }
    }
    public IEnumerator ChangeToLevel2()
    {
        transition.SetActive(true);
        yield return new WaitForSeconds(2f);
        Playerspawnpoint.transform.position = new Vector2(0, 0);
        SceneManager.LoadScene("Level2");;
        yield return new WaitForSeconds(2f);
        transition.SetActive(false);
    }
}
