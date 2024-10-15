using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform cameraTransform;  // ��ҧ�ԧ�֧���ͧ
    public float parallaxEffectMultiplier;  // ��Ҥ�����Ңͧ parallax effect (����٧���������)

    private Vector3 lastCameraPosition;  // ���˹觡��ͧ��������ش
    private float textureUnitSizeX;  // ��Ҵ�ͧ texture ����Ѻ��Ǩ�ͺ��õ�͡ѹ�ͧ�ҡ

    void Start()
    {
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
