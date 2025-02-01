using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlayerAgent : MonoBehaviour
{
    [Header("Health and Damage")]
    public float playerAIHealth = 20f;
    private float playerAIPresentHealth;
    public float playerAISpeed;
    public float giveDamage = 2f;

    [Header("playerAI Data")]
    public NavMeshAgent playerAIAgent;
    public Transform enemyTransform;
    public LayerMask enemyLayer;
    public Transform lookPoint;
    public GameObject shootRaycastArea;
    public Transform spawnPoint;
    private CapsuleCollider playerCollider;

    [Header("playerAI shooting variables")]
    public float playerAIshootInterval;
    bool isPreviouslyShot;

    [Header("playerAI Animation and Effects")]
    public Animator playerAIAnimator;
    public ParticleSystem muzzleSpark;



    [Header("playerAI States")]
    public float visionRadius;
    public float shootRadius;
    public bool isEnemyInVisionRadius;
    public bool isEnemyinShootRadius;
    public bool isEnemy = false;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip shootAudioClip;

    private void Awake()
    {
        playerAIAgent = GetComponent<NavMeshAgent>();
        playerAIPresentHealth = playerAIHealth;
    }

    void Start()
    {
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        isEnemyInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, enemyLayer);
        isEnemyinShootRadius = Physics.CheckSphere(transform.position, shootRadius, enemyLayer);

        if (isEnemyInVisionRadius && !isEnemyinShootRadius) PersueEnemyMode();
        if (isEnemyInVisionRadius && isEnemyinShootRadius) ShootEnemyMode();
    }

    private void PersueEnemyMode()
    {
        if (playerAIAgent.SetDestination(enemyTransform.position))
        {
            playerAIAnimator.SetBool("Run", true);
            playerAIAnimator.SetBool("Shoot", false);
        }
        else
        {
            playerAIAnimator.SetBool("Run", false);
            playerAIAnimator.SetBool("Shoot", false);
        }
    }

    private void ShootEnemyMode()
    {
        playerAIAgent.SetDestination(transform.position);
        transform.LookAt(lookPoint);


        if (!isPreviouslyShot)
        {
            muzzleSpark.Play();
            audioSource.PlayOneShot(shootAudioClip);
            if (Physics.Raycast(shootRaycastArea.transform.position, shootRaycastArea.transform.forward, out RaycastHit hit, shootRadius))
            {
                Debug.Log("Shoooting" + hit.transform.name);

                if (hit.transform.TryGetComponent<EnemyAgent>(out var enemy))
                {
                    enemy.EnemyDamage(giveDamage);
                }

                playerAIAnimator.SetBool("Run", false);
                playerAIAnimator.SetBool("Shoot", true);
            }

            isPreviouslyShot = true;
            Invoke(nameof(ActiveShooting), playerAIshootInterval);
        }

    }

    private void ActiveShooting()
    {
        isPreviouslyShot = false;
    }

    public void PlayerAIDamage(float damage)
    {
        playerAIPresentHealth -= damage;

        if (playerAIPresentHealth <= 0)
        {
            StartCoroutine(PlayerAIRespawn());
        }
    }

    IEnumerator PlayerAIRespawn()
    {
        playerAIAgent.SetDestination(transform.position);
        playerAISpeed = 0f;
        shootRadius = 0f;
        visionRadius = 0f;
        isEnemyInVisionRadius = false;
        isEnemyinShootRadius = false;
        playerAIAnimator.SetBool("Die", true);
        playerAIAnimator.SetBool("Run", false);
        playerAIAnimator.SetBool("Shoot", false);
        ScoreManager.Instance.enemyKills += 1;
        playerCollider.enabled = false;

        yield return new WaitForSeconds(5f);

        playerCollider.enabled = true;
        playerAIPresentHealth = playerAIHealth;
        playerAISpeed = 1.5f;
        shootRadius = 10f;
        visionRadius = 100;
        isEnemyInVisionRadius = true;
        isEnemyinShootRadius = false;

        playerAIAnimator.SetBool("Die", false);
        playerAIAnimator.SetBool("Run", true);


        transform.position = spawnPoint.transform.position;
        PersueEnemyMode();
    }
}
