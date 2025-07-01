using System.Collections;
using UnityEngine;
public class GMRespawn : MonoBehaviour
{
    public static GMRespawn Instance;
    public Transform currentRespawnPoint;
    public ParticleSystem respawnParticles;
    public AudioClip deathSound;
    public AudioClip respawnSound;

    AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();

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
            StartCoroutine(RespawnCoroutine());
        }
        else
        {
            Debug.LogWarning("No respawn point set!");
        }
    }

    private IEnumerator RespawnCoroutine()
    {
        // Play explosion or particles BEFORE teleporting
        respawnParticles.transform.position = pTrans.position;
        respawnParticles.Play();

        audioSource.clip = deathSound;
        audioSource.volume = 1.0f;
        audioSource.Play();

        // Optionally disable player visuals/input here
        pTrans.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f); // wait 1 second

        // Move player to new position
        pTrans.position = currentRespawnPoint.position;

        // Optionally reset velocity here
        var rb = pTrans.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        audioSource.clip = respawnSound;
        audioSource.volume = 0.6f;
        audioSource.Play();

        respawnParticles.transform.position = pTrans.position;
        respawnParticles.Play();

        // Re-enable player
        pTrans.gameObject.SetActive(true);
    }

}
