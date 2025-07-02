using UnityEngine;
using UnityEngine.UI;

public class BackgroundLooper : MonoBehaviour
{
    private float worldWidth;
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        // Convert the UI element's width from RectTransform to world units
        RectTransform rt = GetComponent<RectTransform>();
        float canvasScale = GetComponentInParent<Canvas>().scaleFactor;
        worldWidth = rt.rect.width / canvasScale;
    }

    void Update()
    {
        float camX = cameraTransform.position.x;
        float diff = camX - transform.position.x;

        if (diff >= worldWidth)
        {
            transform.position += Vector3.right * worldWidth * 2f;
        }
        else if (diff <= -worldWidth)
        {
            transform.position += Vector3.left * worldWidth * 2f;
        }
    }
}
