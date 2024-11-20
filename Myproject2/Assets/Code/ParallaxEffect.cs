using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float parallaxEffectMultiplier;  // ค่าความช้าของ parallax effect (ยิ่งสูงจะยิ่งเร็ว)

    private Transform cameraTransform;  // อ้างอิงถึงกล้อง
    private Vector3 lastCameraPosition;  // ตำแหน่งกล้องครั้งล่าสุด
    private float textureUnitSizeX;  // ขนาดของ texture สำหรับตรวจสอบการต่อกันของฉาก

    void Start()
    {
        // หา Main Camera โดยอัตโนมัติ
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

