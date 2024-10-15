using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class changetosave : MonoBehaviour
{
    public Transform Playerspawnpoint;
    public GameObject transition;
    void Awake()
    {
        // ��Ǩ�ͺ����� GameObject ����������������ѧ ���ͻ�ͧ�ѹ��ë�ӫ�͹
        if (FindObjectsOfType<changetosave>().Length > 1)
        {
            Destroy(gameObject); // ������ҡ���� 1 �ѹ ������� GameObject ������ͻ�ͧ�ѹ��ë�ӫ�͹
        }
        else
        {
            DontDestroyOnLoad(gameObject); // ����� GameObject ������١��������������¹ Scene
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
