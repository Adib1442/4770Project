
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerSniper : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Transform SniperPos;

    public Transform payload;


    public LayerMask whatIsGround, whatIsPlayer, IsPayload;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange, payloadSightRange, escortRange;
    public bool playerInSightRange, playerInAttackRange, payloadInSightRange, payloadInEscortRange;

    private void Awake()
    {
        SniperPos = GameObject.Find("SniperPosition").transform;
        payload = GameObject.Find("Payload").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void UpdatePlayerTarget()
    {
    GameObject playerObj = GameObject.Find("PlayerObj");
    if (playerObj != null)
    {
        player = playerObj.transform;
    }
    }

    private void Update()
    {
        UpdatePlayerTarget();
         if(player != null){
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        payloadInSightRange = Physics.CheckSphere(transform.position, payloadSightRange, IsPayload);
        payloadInEscortRange = Physics.CheckSphere(transform.position, escortRange, IsPayload);


        if (!playerInSightRange && !playerInAttackRange && !payloadInSightRange && !payloadInEscortRange) Patroling();
        if (!playerInSightRange && !playerInAttackRange && payloadInSightRange && !payloadInEscortRange) Escort();
        if (!playerInSightRange && !playerInAttackRange && payloadInSightRange && payloadInEscortRange) Escorting();
        if (playerInSightRange && !playerInAttackRange && payloadInSightRange && payloadInEscortRange) Escorting();


        if (playerInSightRange) MoveToSnipPos();

        //if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
         }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }


    private void Escort()
    {
        agent.SetDestination(payload.position);
    }

    private void Escorting()
    {
        agent.SetDestination(transform.position);
    
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void MoveToSnipPos()
    {
        agent.SetDestination(SniperPos.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 35f, ForceMode.Impulse);
            rb.AddForce(transform.up * 3f, ForceMode.Impulse);
          

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, escortRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, payloadSightRange);
    }
}
