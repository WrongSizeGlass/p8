using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle3Controller : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] puzzleObjects;
   // public GameObject truck1;
   // public GameObject truck2;
    TruckScript t1;
    TruckScript t2;
    PressurePlate pp;
    private bool IamActive = true;
    public bool respawn = true;
    public bool bruteForce;
    public bool stop = false;
    void Start()
    {
        t1 = puzzleObjects[0].GetComponent<TruckScript>();
        t2 = puzzleObjects[1].GetComponent<TruckScript>();
        pp = puzzleObjects[5].GetComponent<PressurePlate>();
        
    }

    // Update is called once per frame
    void Update()
    {
        bruteForce = pp.bruteForce;
       // startPuzzle3(s);
    }
    public void startPuzzle3(bool start)
    {        
       
        if (start)
        {
           // Debug.LogError(start);
            /*if(stop){
                return;
            }*/
            if (!stop) {
                for (int i = 2; i < 5; i++)
                {
                    puzzleObjects[i].SetActive(false);
                }
                t1.startTrucks();
                t2.startTrucks();
                pp.startPuzzle(start);
            }
        }
        else{
            for (int i = 2; i < 5; i++)
            {
                puzzleObjects[i].SetActive(true);
            }
        }
        if(Puzzle3Complete()){

            for (int i = 2; i < 5; i++)
            {
                puzzleObjects[i].SetActive(true);
            }
            Debug.LogError("PUZZLE 3 COMPLETE !!! ");
        }
        

    }
    public void resetP3(bool spawnPlayer){
        t1.resetTrucks();
        t2.resetTrucks();
        

        
        if (spawnPlayer)
        {
            puzzleObjects[puzzleObjects.Length - 1].transform.position = puzzleObjects[puzzleObjects.Length - 2].transform.position;
            puzzleObjects[puzzleObjects.Length - 1].transform.rotation = puzzleObjects[puzzleObjects.Length - 2].transform.rotation;
        }
        pp.resetPP();
        Debug.LogError("RESET");
        stop = false;
    }


    public bool Puzzle3Complete(){
        return pp.Puzzle3Complete();
    }
}
