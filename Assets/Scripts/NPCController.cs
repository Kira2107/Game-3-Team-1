using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum NPCState
{
    WANDER,
    SEEK,
    POSSESSED,
    DEAD
}

public class NPCController : MonoBehaviour
{
    public NPCState currentState = NPCState.WANDER;

    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    public float seekTime = 5f;  
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;
    public float angle = 0f;
    private Vector3 lastKnownDisturbance;
    private bool hasSeenBody = false;  

    public bool isPossessed = false;

    // allows you to set what lights to look at in editor
    public List<Light> lightsInLevel = new List<Light>();

    private NavMeshAgent agent;
    [SerializeField] private EnemySpawner enemySpawner;

    public bool InRoomWithPlayer { get; set; }

    void Awake()
    {
        GetComponent<NavMeshAgent>().enabled = false;

    }

    void Start()
    {
        StartCoroutine(StartGame());
        InRoomWithPlayer = false;
    }

IEnumerator StartGame()
{
    yield return new WaitForSeconds(2f);

    agent = GetComponent<NavMeshAgent>(); // Assign the agent before enabling
    agent.Warp(patrolPoints[0].position); // Assign player position before enabling
    agent.enabled = true; // Now enable the agent

    if (patrolPoints.Length == 0)
    {
        Debug.LogError("No patrol points found for NPC!");
        yield break; // Exit the coroutine to avoid errors
    }

    GoToNextPatrolPoint();
}


    void Update()
    {
        switch (currentState)
        {
            case NPCState.WANDER:
                WanderBehavior();
                break;
            case NPCState.SEEK:
                SeekBehavior();
                break;
            case NPCState.POSSESSED:
                PossessedBehavior();
                break;
            case NPCState.DEAD:
                DeadBehavior();
                break;
        }
    }

    void WanderBehavior()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        if(LightsOff())
        {
            agent.destination = patrolPoints[currentPatrolIndex].position;
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
        else if (InRoomWithPlayer)
        {
            EnterSeekState(FindObjectOfType<PlayerController>().Position);
        }

    }

    public void EnterSeekState(Vector3 disturbanceLocation)
    {
        if (currentState == NPCState.SEEK) return;

        lastKnownDisturbance = disturbanceLocation;
        currentState = NPCState.SEEK;
        agent.destination = disturbanceLocation;
        Invoke(nameof(ReturnToWander), seekTime);
    }

    void SeekBehavior()
    {
        if(LightsOff())
        {
            ReturnToWander();
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            Debug.Log("NPC is investigating...");
        }
    }

    void ReturnToWander()
    {
        currentState = NPCState.WANDER;
        GoToNextPatrolPoint();
    }

    public void EnterPossessedState()
    {
        if (currentState == NPCState.DEAD) return;

        isPossessed = true;
        currentState = NPCState.POSSESSED;
        
        //Disable the NavMeshAgent to stop automated movement
        agent.isStopped = true;
        agent.enabled = false;
    }

    bool LightsOff()
    {
        int numLightsOn = 0;

        foreach(Light light in lightsInLevel)
        {
            // if any light is on, the player cannot possess the enemy
            if(light.enabled)
            {
                return false;
            }
        }

        return true;
         
    }

    void PossessedBehavior()
    {
        //Get player input (assuming the player uses WASD/Arrow keys for movement)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //Move the NPC (same as player)
         Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        if (moveDirection.magnitude >= 0.1f)
        {
            //Rotate the NPC to face the direction of movement
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;

            //Smoothly rotate the NPC towards the target angle
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angle, rotationSpeed);

            //Apply the rotation
            transform.rotation = Quaternion.Euler(0, angle, 0);

            //Move the NPC
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    public void ExitPossessedState()
    {
        isPossessed = false;
        EnterDeadState();
    }

    public void EnterDeadState()
    {
        currentState = NPCState.DEAD;
        agent.isStopped = true;
        agent.enabled = true; //Re-enable the NavMeshAgent
        Debug.Log("NPC is now dead.");
    }

    void DeadBehavior()
    {
    }

    void CheckForDeadBodies()
    {
        Collider[] bodies = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("DeadBody"));
        foreach (Collider body in bodies)
        {
            if (!hasSeenBody)
            {
                hasSeenBody = true;
                EnterSeekState(body.transform.position);
                Debug.Log("NPC saw a dead body!");
                return;
            }
        }
    }

    public void AlertNPC(Vector3 disturbanceLocation)
    {
        EnterSeekState(disturbanceLocation);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Player has been caught by {gameObject.name}!");
            FindObjectOfType<GameOverManager>().GameOver();
        }
    }
}
