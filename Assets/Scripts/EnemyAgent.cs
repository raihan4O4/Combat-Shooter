using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAgent : MonoBehaviour
{
    [Header("Health and Damage")]
    public float enemyHealth = 20f;
    private float enemyPresentHealth;
    public float enemySpeed;
    public float giveDamage = 5f;

    [Header("Enemy Data")]
    public NavMeshAgent enemyNavMeshAgent;
    public Transform playerTransform;
    public LayerMask playerLayer;
    public Transform lookPoint;
    public GameObject shootRaycastArea;
    public Transform spawnPoint;
    private CapsuleCollider enemyCollider;

    [Header("Enemy shooting variables")]
    public float enemyshootInterval;
    bool isPreviouslyShot;

    [Header("Enemy Animation and Effects")]
    public Animator enemyAnimator;
    public ParticleSystem muzzleSpark;


    [Header("Enemy States")]
    public float visionRadius;
    public float shootRadius;
    public bool isPlayerInVisionRadius;
    public bool isPlayerinShootRadius;
    public bool isPlayer = false;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootAudioClip;

    private void Awake()
    {
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
        enemyPresentHealth = enemyHealth;
    }

    void Start()
    {
        enemyCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        isPlayerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        isPlayerinShootRadius = Physics.CheckSphere(transform.position, shootRadius, playerLayer);

        if (isPlayerInVisionRadius && !isPlayerinShootRadius) PersuePlayerMode();
        if (isPlayerInVisionRadius && isPlayerinShootRadius) ShootPlayerMode();
    }

    private void PersuePlayerMode()
    {
        if (enemyNavMeshAgent.SetDestination(playerTransform.position))
        {
            enemyAnimator.SetBool("Run", true);
            enemyAnimator.SetBool("Shoot", false);
        }
        else
        {
            enemyAnimator.SetBool("Run", false);
            enemyAnimator.SetBool("Shoot", false);
        }
    }

    private void ShootPlayerMode()
    {
        enemyNavMeshAgent.SetDestination(transform.position);
        transform.LookAt(lookPoint);


        if (!isPreviouslyShot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootAudioClip);
            if (Physics.Raycast(shootRaycastArea.transform.position, shootRaycastArea.transform.forward, out RaycastHit hit, shootRadius))
            {
                Debug.Log("Shoooting" + hit.transform.name);

                if (isPlayer == true)
                {
                    if (hit.transform.TryGetComponent<PlayerController>(out var player))
                    {
                        player.PlayerDamage(giveDamage);
                    }
                }
                else
                {
                    if (hit.transform.TryGetComponent<PlayerAgent>(out var playerAI))
                    {
                        playerAI.PlayerAIDamage(giveDamage);
                    }
                }

                enemyAnimator.SetBool("Run", false);
                enemyAnimator.SetBool("Shoot", true);
            }

            isPreviouslyShot = true;
            Invoke(nameof(ActiveShooting), enemyshootInterval);
        }


    }

    private void ActiveShooting()
    {
        isPreviouslyShot = false;
    }

    public void EnemyDamage(float damage)
    {
        enemyPresentHealth -= damage;

        if (enemyPresentHealth <= 0)
        {
            StartCoroutine(EnemyRespawn());
        }
    }

    IEnumerator EnemyRespawn()
    {
        enemyNavMeshAgent.SetDestination(transform.position);
        enemySpeed = 0f;
        shootRadius = 0f;
        visionRadius = 0f;
        isPlayerInVisionRadius = false;
        isPlayerinShootRadius = false;
        enemyAnimator.SetBool("Die", true);
        enemyAnimator.SetBool("Run", false);
        enemyAnimator.SetBool("Shoot", false);
        ScoreManager.Instance.playerKills += 1;
        enemyCollider.enabled = false;

        yield return new WaitForSeconds(5f);

        enemyCollider.enabled = true;
        enemyPresentHealth = enemyHealth;
        enemySpeed = 1.5f;
        shootRadius = 10f;
        visionRadius = 100;
        isPlayerInVisionRadius = true;
        isPlayerinShootRadius = false;

        enemyAnimator.SetBool("Die", false);
        enemyAnimator.SetBool("Run", true);


        transform.position = spawnPoint.transform.position;
        PersuePlayerMode();
    }
}
