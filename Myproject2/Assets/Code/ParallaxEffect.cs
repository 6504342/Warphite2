using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxEffectMultiplier;  // ��Ҥ�����Ңͧ parallax effect (����٧���������)

    private Transform cameraTransform;  // ��ҧ�ԧ�֧���ͧ
    private Vector3 lastCameraPosition;  // ���˹觡��ͧ��������ش
    private float textureUnitSizeX;  // ��Ҵ�ͧ texture ����Ѻ��Ǩ�ͺ��õ�͡ѹ�ͧ�ҡ

    void Start()
    {
        // �� Main Camera ���ѵ��ѵ�
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    void FixedUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier, deltaMovement.y * parallaxEffectMultiplier);
        lastCameraPosition = cameraTransform.position;
    }
}

