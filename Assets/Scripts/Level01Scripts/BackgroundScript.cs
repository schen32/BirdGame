using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxMultiplier = 0.5f;
    Transform cameraTransform;
    Vector3 previousCameraPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - previousCameraPosition;
        transform.position -= new Vector3(deltaMovement.x * parallaxMultiplier, deltaMovement.y * parallaxMultiplier, 0);
        previousCameraPosition = cameraTransform.position;
    }
}
