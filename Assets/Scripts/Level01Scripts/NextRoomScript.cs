using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public class NextRoomScript : MonoBehaviour
{
    public BoxCollider2D newCameraBounds;
    public Transform nextRespawnPoint;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        // Update camera confiner
        var vcam = FindAnyObjectByType<CinemachineCamera>();
        var confiner = vcam.GetComponent<CinemachineConfiner2D>();
        confiner.BoundingShape2D = newCameraBounds;
        confiner.InvalidateBoundingShapeCache();

        GMRespawn.Instance.SetRespawnPoint(nextRespawnPoint);
    }
}
