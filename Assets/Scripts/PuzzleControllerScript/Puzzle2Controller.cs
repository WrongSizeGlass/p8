using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle2Controller : MonoBehaviour
{
    public List<GameObject> basketGroup;
    public GameObject waterpump;
    public GameObject fountain;
    private WaterRender wr;
    public List<Vector3> BasketPosList;
    private Vector3 WaterPumpStartPos;
    public bool respawn =false;
    public bool skipLvl = false;
    public Transform playerSpawn;
    public Transform player;
    public bool isWaterpumping;
    public int basketCounter=0;
    Outline ol;
    public bool runOnce = false;
    bool runOnce1 = false;
    public int water;
    public bool isPumping;
    public bool hasRuned = false;
    // Start is called before the first frame update
    void Start()
    {
        
        BasketPosList = new List<Vector3>();
        for(int i=0; i< basketGroup.Count; i++){
            BasketPosList.Insert(i, basketGroup[i].transform.position);
            basketGroup[i].SetActive(false);            
        }
        WaterPumpStartPos = basketGroup[basketGroup.Count-1].transform.position;
        wr = fountain.GetComponent<WaterRender>();
        
        //waterpump.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        basketCounter = wr.waterCounter;
        isPumping = wr.isPumping();
        //Debug.Log(isPumping);
    }

    public void startPuzzle2(bool start){
        wr.RenderWater(!start);
        //waterpump.SetActive(start);
        if(!hasRuned && !puzzle2Complete())
        {
            for (int i = 0; i < basketGroup.Count; i++)
            {
                if(i<8 && basketGroup[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled==true){
                    basketGroup[i].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                }
                basketGroup[i].SetActive(true);
            }
            hasRuned = true;
            basketCounter = 0;
        }

        if (isPumping && hasRuned || basketCounter>1 && hasRuned ){
            for (int i = 0; i < 8; i++)
            {                
                basketGroup[i].GetComponent<Outline>().enabled = false;                
            }
            hasRuned = false;
        }
        if (runOnce)// are used in resetP2
            runOnce = false;   


            

    }

    public void resetP2(bool spawnPlayer){
        if (water > 1){
            
            wr.waterCounter = 0;
            water = 0;
        }
        if (!runOnce){

            for (int i = 0; i < basketGroup.Count; i++)
            {
                if (basketGroup[i].transform.position != BasketPosList[i]) {
                    basketGroup[i].transform.position = BasketPosList[i];
                }
            }
            runOnce = true;
            //wr.isPumping() = false;
            isPumping = false;

            if (hasRuned)// are used in startPuzzle2
                hasRuned = false;

            
        }
        if (spawnPlayer)
        {
            player.transform.position = playerSpawn.position;
            player.transform.rotation = playerSpawn.rotation;
        }
        //startPuzzle2(true);
    }

    public void skipPuzzle2(bool skip){
        skipLvl = skip;
        if(skipLvl){
            wr.waterCounter = 2;
        }else{
            wr.waterCounter = 0;
        }
    }
    public bool puzzle2Complete(){
       if(basketCounter>1){
            return true;
       }
       if(isPumping){
            return true;
       }
        return false;
        

    }
}
