using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // We need this for the NavMesh
using UnityEngine.SceneManagement; // We need this for the scene manager

public class AiCar : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agentCar;
   
    private  List<Vector3> WaypointList;
    public bool forward;
    public Transform patrolPoints;
    private int currentPoint = 0;
    // Awake is called before Start()
    void Awake()
    {
        WaypointList = new List<Vector3>();
        int counter = 0;
       
        for (int i = 0; i < patrolPoints.transform.childCount; i++) {
            WaypointList.Insert(i, patrolPoints.transform.GetChild(i).transform.position);
        }
        if(!forward){
            WaypointList.Reverse();
        }
       
        int startPos=0;
        for (int i=0;i<WaypointList.Count; i++){
            if(Vector3.Distance(WaypointList[i], transform.position)<20)
            {
                //Debug.LogError( "index " +i + "distance " + Vector3.Distance(WaypointList[i], transform.position) + " name: " + this.name);
                startPos = i;
            }
        }
        // Added these lines to automatically add components in the inspector when the script is activated
        rb = GetComponent<Rigidbody>();
        agentCar = GetComponent<NavMeshAgent>();

        // Checks if the NavMesh has been added to the agent/enemy
        if (agentCar == null)
        {
            Debug.LogError("The NavMeshAgent isn't attached to " + gameObject.name);
        }
        // Checks if the Rigidbody has been added to the agent/enemy
        if (rb == null)
        {
            Debug.LogError("The Rigidbody isn't attached to " + gameObject.name);
        }
        currentPoint = startPos;
        agentCar.destination = WaypointList[startPos];
    }

    // everytime you work with physics you want to use fixed update instead of update!
    void FixedUpdate()
    {
        Driving();
    }

   
    
    void Driving()
    {    
            if (agentCar.remainingDistance < 2f)
           // if (agentCar.transform.position == patrolPoints[currentPoint].transform.position)
            {
                currentPoint++;
                if (currentPoint >= WaypointList.Count)
                {
                    currentPoint = 0;
                }

                 agentCar.destination = WaypointList[currentPoint];
            }   
    }
}