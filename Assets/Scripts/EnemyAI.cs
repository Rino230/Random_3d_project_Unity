using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float attackDistance = 1.5f;

    [Header("Attack")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("References")]
    [SerializeField] private Transform player;

    private NavMeshAgent agent;
    private PlayerHealth playerHealth;

    private float nextAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (player == null || playerHealth == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position
        );

        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            Attack();
        }
    }

    private void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            playerHealth.TakeDamage(attackDamage);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
}
