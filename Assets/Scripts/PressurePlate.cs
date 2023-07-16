using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject objective;
    public GameObject objectiveGoalPos;
    public Vector3 objectiveStartPos;
    public bool p3Complete;
    Outline ol;
    Outline ol_Objective;
    public bool bruteForce = false;
    public bool isActive;
    float controllY;
  
    // Start is called before the first frame update
    void Start()
    {
        ol = GetComponent<Outline>();
        ol_Objective = objectiveGoalPos.GetComponent<Outline>();
        objectiveStartPos = objective.transform.position;
        objective.SetActive(false);
        objectiveGoalPos.SetActive(true);
        ol.enabled = false;
        ol_Objective.enabled = false;
        objective.transform.position = objectiveStartPos;
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startPuzzle(bool start){
 
        if (start)
        {
            Debug.LogError(" PP startPuzzle start: " + start);
            objective.SetActive(true);
            objectiveGoalPos.SetActive(false);
            ol.enabled = true;
            ol_Objective.enabled = true;
          
        }
        if(objective.transform.position.y<objectiveStartPos.y-2){
            objective.transform.position = objectiveStartPos;
        }
    }
    public void resetPP(){
        objective.SetActive(false);
        objectiveGoalPos.SetActive(true);
        ol.enabled = false;
        ol_Objective.enabled = false;
        objective.transform.position = objectiveStartPos;
        p3Complete = false;
    }
    public void turnOff(){

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag== "Electric_Box_Objective"&&!p3Complete)
        {
            p3Complete = true;
            
           
        }
    }

    public bool Puzzle3Complete(){
        if(p3Complete){
            objectiveGoalPos.SetActive(true);
            objective.SetActive(false);
            objectiveGoalPos.GetComponent<MeshRenderer>().enabled = true;
            objective.GetComponent<MeshRenderer>().enabled=false;
            return objectiveGoalPos.GetComponent<MeshRenderer>().enabled && !objective.GetComponent<MeshRenderer>().enabled;

        }
        return false;
    
    }


}
