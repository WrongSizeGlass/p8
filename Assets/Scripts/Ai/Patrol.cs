using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // We need this for the NavMesh
using UnityEngine.SceneManagement;
using System;

public class Patrol : MonoBehaviour
{
    #region Variables

    [Header("Patrol stuff...")]
    [Tooltip("Array indicating patrol route. Gameobject is the target destination")]
    public Transform[] patrolPoints;
    // Refer to the array index of patrolPoints
    private int currentPoint = 0;

    [Header("AI stuff...")]
    public Transform player;
    public Transform playerAni;
    private Rigidbody rb;
    private NavMeshAgent agent;
    public Transform raypos;

    [Header("FoV stuff...")]
    [Range(0, 180)] public float maxAngle;
    public float maxRadius;
    private bool playerInRange = false;
    public float patroleSpeed = 1.5f;
    public float AlertSpeed = 1.5f;
    public float InvestigateSpeed = 1.5f;
    public float ChaseSpeed = 2.2f;
    // For counting down between changing states
    private int currentState = 0;

    // Used for agent investigation
    private Transform lastPlayerLocation;
    RaycastHit hit;
    private PlayerAniScript pas;
    bool stealth = false;
    #endregion

    // This is only to visualize the agent's FoV
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        // The reason why we use transform.up is because work with the horizontal axis (green rotation)
        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.up) * transform.forward * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.up) * transform.forward * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (playerInRange == true)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        // Draws line to the player, but line goes outsite FoV
        Gizmos.DrawLine(transform.position, player.position);
        // Draws line to the player, but line stays within FoV
        //Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.forward * maxRadius);
    }
    public bool rayHit(Vector3 pos, Vector3 dir)
    {
        Debug.DrawRay(pos, dir);
        try
        {
            return Physics.Raycast(pos, dir, out hit, 0.4f);
        }
        catch (Exception e)
        {
            return false;
        }
    }
    // Agent's Field of View (FoV)
    bool inFov(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[10];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);

        // Basically this is used to see if the player is within the agent's FoV
        for (int i = 0; i < count ; i++)
        {
            if (overlaps[i] != null)
            {
                if (overlaps[i].transform == target)
                {
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    // Just to make sure the y direction is always 0, so that height is not a factor
                    directionBetween.y *= 0;

                    float angle = Vector3.Angle(checkingObject.forward, directionBetween);

                    if (angle <= maxAngle)
                    {
                        //
                            Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        

                        // Only updates as long as player is within FoV
                        lastPlayerLocation = player;
                        //Debug.Log("Last player location: " + lastPlayerLocation.transform.position);
                       

                       /* if (rayHit(checkingObject.position, target.position - checkingObject.position) && hit.collider.tag== target.tag) {

                            Debug.LogError(hit.collider.name);
                            return true;
                        }*/

                        if (Physics.Raycast(ray, out hit, maxRadius,7))
                        {
                            Debug.LogWarning("hitting ? " + true + " name " + hit.collider.name);
                            if (hit.collider.tag == target.tag)
                            {
                                Debug.LogError(hit.collider.name);
                                return true;
                            }
                        }
                    }
                }
            }
        }
        // If none of our checks are true; the player isn't within agent's FoV 
        // and is returned as false
        return false;
    }

    //bool lookingAtPlayer = false; // What is this used for? I commented it out and no errors are given
    int IncreaseCurrentState(int counter, float timeInSec, int value, bool playerInRange)
    {
        if (counter % Mathf.Round(timeInSec / Time.fixedDeltaTime) == 0 && playerInRange == true && !stealth)
        {
            value++;
            // Debug.Log("reset currentStatev to: " + currentState);
            Debug.LogWarning("not stealth: " + currentState + " for every " + timeInSec + " sec" );
        }
        if (counter % Mathf.Round(((timeInSec*2)) / Time.fixedDeltaTime) == 0 && playerInRange == true && stealth)
        {
            value ++;
             Debug.LogWarning("stealth: " + currentState + " for every " + (timeInSec * 2) + " sec");
        }
        // currentState = IntRange(currentState, 0, 100); // Does this need to be here?
        return value;
    }

    int DecreaseCurrentState(int counter, float timeInSec, int value, bool playerInRange)
    {
        if (counter % Mathf.Round(timeInSec / Time.fixedDeltaTime) == 0 && playerInRange == false)
        {
            value --;
            //Debug.Log("decreasing currentState by: " + currentState);
        }
        //currentState = IntRange(currentState, 0, 100); // Does this need to be here?
        return value;
    }

    // What does this do?
    int IntRange(int value, int min, int max)
    {
        if (value < min)
        {
            value = min;
        }
        else if (value > max)
        {
            value = max;
        }
        return value;
    }

    // Awake is called before Start()
    void Awake()
    {
        // Added these lines to automatically add components in the inspector when the script is activated
        player = GameObject.Find("player").transform;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        pas = playerAni.GetComponent<PlayerAniScript>();
        audioSource = GetComponent<AudioSource>();
        
        // Checks if the NavMesh has been added to the agent/enemy
        if (agent == null)
        {
            Debug.LogError("The NavMeshAgent isn't attached to " + gameObject.name);
        }
        // Checks if the Rigidbody has been added to the agent/enemy
        if (rb == null)
        {
            Debug.LogError("The Rigidbody isn't attached to " + gameObject.name);
        }
    }

    void Start()
    {
        InvokeRepeating("CallFootsteps", 0, 1);
    }

    int FixedCounter = 0;
    void FixedUpdate()
    {

        stealth = pas.crouch;
        if (stealth) { Debug.LogError(" stealth " + stealth); }
        playerInRange = inFov(transform, player, maxAngle, maxRadius);
        //Debug.LogWarning("Player is in range " + playerInRange);
        currentState = playerInRange ? IncreaseCurrentState(FixedCounter, 0.2f, currentState, playerInRange) : DecreaseCurrentState(FixedCounter, 0.2f, currentState, playerInRange);
       // Debug.Log("playerInRange  " + playerInRange);
        currentState = IntRange(currentState, 0, 100);
        //Debug.LogWarning(currentState);

        FixedCounter++;

        if (playerInRange)
        {
            transform.LookAt(player, Vector3.left);
        }
       
        switch (agentStateIndex(currentState))
        {
            case 0:
                Patrolling();
                agent.speed = patroleSpeed;
                //Debug.LogWarning("Patrol");
                agentIsMoving = true;
                break;
            case 1:
                agent.speed = AlertSpeed;
                //Debug.LogWarning("Alert");
                agentIsMoving = false;
                break;
            case 3:
                Investigating();
                agent.speed =InvestigateSpeed;
                //Debug.LogWarning("Investigate");
                agentIsMoving = true;
                break;
            case 4:
                Chasing();
                agent.speed = ChaseSpeed;
                //Debug.LogWarning("Chase");
                agentIsMoving = true;
                break;
        }
    }

    int agentStateIndex(int index)
    {

        if (BoolRange(index, 0, 2))
        {
            return 0;
        }
        /*else if (BoolRange(index, 3, 9))
        {
            return 1;
        }
        else if (BoolRange(index, 10, 19) && !playerInRange)
        {
            return 3;
        }*/
        else
            return 4;
    }

    void Patrolling()
    {
        //Debug.Log("Distance to current node is " + agent.remainingDistance);

        // Agent goes to next patrol point after reaching its current node
        if (agent.remainingDistance < 0.5f)
        {
            // Destination for the agent
            agent.destination = patrolPoints[currentPoint++].position;
            //Debug.Log("currentPoint node: " + currentPoint);
        }
        // Restart the current patrol point back to the first node
        if (currentPoint >= patrolPoints.Length)
        {
            currentPoint = 0;
            //Debug.Log("currentPoint node reset to: " + currentPoint);
        }
    }

    void Chasing()
    {
        agent.SetDestination(player.position);
        //Debug.Log("Agent is chasing the player");
        //Debug.Log("Current player location: " + player.transform.position);
    }

    void Investigating()
    {
        agent.SetDestination(lastPlayerLocation.position);
    }

    /*
    public float restartDelay = 0.5f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            agent.speed = 0;
            Invoke("Restart", restartDelay); // resets the game when player is hit
            
            // We don't need to use invoke, 
            // it was just to have a small delay before resetting, 
            // but we should probably get do something nicer
        }
    }

    void Restart()
    {
        //Debug.Log("GAME OVER");
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene("StartMenu");
    }
    */

    bool agentIsMoving = false;
    AudioSource audioSource;
    public AudioClip footstepSound;

    void CallFootsteps()
    {
        if(agentIsMoving == true)
        {
            audioSource.PlayOneShot(footstepSound);
        }
    }

    // Used for the agent state index?
    bool BoolRange(int value, int min, int max)
    {
        return value >= min && value <= max ? true : false;
    }

    bool timer(int counter, float timeInSec)
    {
        if (counter % Mathf.Round(timeInSec / Time.fixedDeltaTime) == 0)
        {
            return true;
        }
        return false;
    }
}
