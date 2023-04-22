
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerBlue : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask IsGround, IsPlayer;

    public Vector3 wp;
    bool wpSet;
    public float wpRange;

    public float AttackDelay;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        //player = GameObject.Find("Capsule").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void UpdatePlayerTarget()
    { 
    GameObject RedSniper = GameObject.Find("RedSniper");
    GameObject Capsule = GameObject.Find("Capsule");
    if (Capsule != null)
    {
        player = Capsule.transform;
    }else if (RedSniper != null ){
        player = RedSniper.transform;

    }
    }

    private void Update()
    {
        UpdatePlayerTarget();
        if(player != null){
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, IsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, IsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!wpSet) Searchwp();

        if (wpSet)
            agent.SetDestination(wp);

        Vector3 distanceTowp = transform.position - wp;

        if (distanceTowp.magnitude < 1f)
            wpSet = false;
    }
    private void Searchwp()
    {
        float randomZ = Random.Range(-wpRange, wpRange);
        float randomX = Random.Range(-wpRange, wpRange);

        wp = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(wp, -transform.up, 2f, IsGround))
            wpSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {

        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 40f, ForceMode.Impulse);
            rb.AddForce(transform.up * 3f, ForceMode.Impulse);

            alreadyAttacked = true;
            
            Invoke(nameof(ResetAttack), AttackDelay);
             
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
    }
}
