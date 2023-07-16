using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Controller : MonoBehaviour
{
    
    public int collection=0;
    public bool isActive;
    public GameObject puzzleObjectsGroup;
    public bool spawn = false;
    public Transform playerSpawnPos;
    public Transform player;
    public bool turnOffOnce = false;
    public bool turnOnOnce = false;
    //private Vector3 groupPos;
    public List<Vector3> D_PosList;
// Start is called before the first frame update
    void Start()
    {
        D_PosList = new List<Vector3>();
        for (int i =0; i<puzzleObjectsGroup.transform.childCount; i++){          
            D_PosList.Insert(i, puzzleObjectsGroup.transform.GetChild(i).position);                     
        }
        collection = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if(puzzle1Complete()) { Debug.LogError("PUZZLE 1 COMPLETE !!! "); }
     //   Debug.LogError(collection);
    }
   public void startPuzzle1(bool start){
        // puzzleObjectsGroup.transform.position = groupPos;


        if (start && spawn)
        {
            for (int i = 0; i < puzzleObjectsGroup.transform.childCount; i++)
            {
                puzzleObjectsGroup.transform.GetChild(i).position = D_PosList[i];
            }
            spawn = false;
            turnOffOnce = false;
        }
        puzzleObjectsGroup.SetActive(start);
       if(!turnOffOnce && collection>4){
            for (int i = 0; i < puzzleObjectsGroup.transform.childCount; i++)
            {
                puzzleObjectsGroup.transform.GetChild(i).gameObject.SetActive(false);//position = new Vector3(0, -100, 0);
            }
            turnOffOnce = true;
            turnOnOnce = false;
       }
    }
    public void resetP1(bool spawnPlayer)
    {
        if (spawnPlayer)
        {
            player.transform.position = playerSpawnPos.position;
            player.transform.rotation = playerSpawnPos.rotation;
        }
        if (!turnOnOnce){
                for (int i = 0; i<puzzleObjectsGroup.transform.childCount; i++)
                {
                    puzzleObjectsGroup.transform.GetChild(i).gameObject.SetActive(true);//position = new Vector3(0, -100, 0);
                }

            turnOnOnce = true;
        }
        spawn = true;
        collection = 0;
    }


    public void skipPuzzle1(){
        collection = 5;
        if (collection>4) {
            for (int i = 0; i < puzzleObjectsGroup.transform.childCount; i++)
            {
                puzzleObjectsGroup.transform.GetChild(i).position = new Vector3(0, -100, 0);
            }
        }
       
    }
    public bool puzzle1Complete(){
        return collection >4;
    }

}
