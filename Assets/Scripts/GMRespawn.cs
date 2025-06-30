using System.Collections;
using UnityEngine;
public class GMRespawn : MonoBehaviour
{
    public static GMRespawn Instance;
    public Transform currentRespawnPoint;

    Transform pTrans;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        pTrans = player.transform;
        RespawnPlayer();
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        currentRespawnPoint = newPoint;
    }

    public void RespawnPlayer()
    {
        if (currentRespawnPoint != null)
        {
            pTrans.position = currentRespawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No respawn point set!");
        }
    }
}
