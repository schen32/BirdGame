using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float smoothSpeed = 5f;
    Transform pTrans;

    void Start()
    {
        pTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(pTrans.position.x, transform.position.y, transform.position.z);
    }

}
