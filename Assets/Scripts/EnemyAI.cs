using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public LayerMask whatsIsGround, whatIsPlayer;
    public GameObject projectile;
    public float shootingForce = 12f;

    //Searching for sales
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;

    private void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Start()
    {
        Invoke("SearchWalkPoint", 15f); //this assures that the agent won't get stuck

    }

    private void Update()
    {
        bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }



    private void Patroling()
    {
        //Debug.Log("Patroling");
        if (walkPoint == new Vector3(0,0,0) || !walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            //Debug.Log("WalkpointSet True: going to walkpoint");
            agent.SetDestination(walkPoint);
        }


        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Debug.Log("Distance to walkpoint: " + distanceToWalkPoint.magnitude);

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 5f)
            walkPointSet = false;

    }

    private void SearchWalkPoint()
    {
        //Debug.Log("Searching for new WalkPoint!");
        agent.isStopped = true;
        agent.ResetPath();
        walkPointSet = false;

        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x+randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatsIsGround))
            walkPointSet = true;
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
            // CALL SHOOT!
            Rigidbody rb = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootingForce, ForceMode.Impulse); 


            //Debug.Log("Shooting!");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    public void TakeDamage(int damage)
    {

        //Here we should drop some Item
        //Debug.Log("Taking damage!");
    }
}
