using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Health and Damage")]
    public float enemySpeed;
    public float giveDamage = 2;

    [Header("Enemy Data")]
    public NavMeshAgent enemyAgent;
    public Transform playerTransform;
    public LayerMask playerLayer;
    public Transform lookPoint;
    public GameObject shootRaycastArea;

    [Header("Enemy shooting variables")]
    public float enemyshootInterval;
    bool isPreviouslyShot;


    [Header("Enemy States")]
    public float visionRadius;
    public float shootRadius;
    public bool isPlayerInVisionRadius;
    public bool isPlayerinShootRadius;
    public bool isPlayer = false;

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {

    }

    void Update()
    {
        isPlayerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
        isPlayerinShootRadius = Physics.CheckSphere(transform.position, shootRadius, playerLayer);

        if (isPlayerInVisionRadius && !isPlayerinShootRadius) PersuePlayerMode();
        if (isPlayerInVisionRadius && isPlayerinShootRadius) PlayerPlayerMode();
    }

    private void PersuePlayerMode()
    {
        if (enemyAgent.SetDestination(playerTransform.position))
        {
            //
        }
    }

    private void PlayerPlayerMode()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(lookPoint);

        if (!isPreviouslyShot)
        {
            if (Physics.Raycast(shootRaycastArea.transform.position, shootRaycastArea.transform.forward, out RaycastHit hit, shootRadius))
            {
                Debug.Log("Shoooting" + hit.transform.name);

                PlayerController player = hit.transform.GetComponent<PlayerController>();

                if (player != null)
                {
                    player.playerDamage(giveDamage);
                }
            }
        }

        isPreviouslyShot = true;
        Invoke(nameof(ActiveShooting), enemyshootInterval);

    }

    private void ActiveShooting()
    {
        isPreviouslyShot = false;
    }
}
