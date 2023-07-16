using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
public class PuzzleTimer : MonoBehaviour
{
    public List<Vector3> playerPositionList;
    private Vector3 playerPosition;

    public GameObject MainPuzlleControllerObject;
    private MainPuzzleController mpc;

    public GameObject writeJsonObj;
    private WriteJson writeJason;

    public GameObject NeighboorPuzzle_1;
    public GameObject NeighboorPuzzle_2;

    private PuzzleTimer NP_1;
    private PuzzleTimer NP_2;

    public GameObject bgmusic;
    private BGMusic bg;

    public int basketCounter = 0;
    public bool WaterPumpBool = false;
    public int PlayerTimeCounter = 0;
    private int fixedCounter = 0;
    public int selfID;
   // PuzzleTimer myTimer;
    public string MyName;
    private string selfIDString = "";

    public bool MyPuzzle = false;
    public bool NextPuzzle = false;
    private bool isInside = false;
    private bool isEmergent = false;
    private bool writeOnce = false;

    private MethodInfo MyPuzzleInfo;
    private MethodInfo NextPuzzleInfo;

    private MethodInfo IsAPuzzlePlaying_1;
    private MethodInfo IsAPuzzlePlaying_2;

    private MethodInfo IsPlayerInside_1;
    private MethodInfo IsPlayerInside_2;

    private MethodInfo GetNeighboorID_1;
    private MethodInfo GetNeighboorID_2;

    bool PlayerInsideBefore=false;
    bool PlayerInsideAfter=false;

    int NeighboorBeforeID = 0;
    int NeighboorAfterID = 0;

    public int ECC_1 = 0;
    public int ECC_2 = 0;
    public GameObject eventCapsule1;
    public GameObject eventCapsule2;
    private rotateObject eventCap1;
    private rotateObject eventCap2;
    private bool hasPlayed = false;
    private bool GetValuesOnce = false;
    private int nextID;
    private bool checkEventOnce1 = false;
    private bool checkEventOnce2 = false;
    public bool CanIPlayMusic = false;
    AudioSource audioSource;
    public AudioClip emergentStart;
    private int before = 0;
    private int after = 0;
    
    public Transform Dgroub;
    public Transform Pgroub;
    public Transform Fgroub;
    List<Transform> Doughnuts_EC_SpacePositions;
    List<Transform> Park_EC_SpacePositions;
    List<Transform> Factory_EC_SpacePositions;


    
    //public AudioClip emergentSolution;

    void Start(){
        points = new List<int>();
        names = new List<string>();
        childscripts = new List<rotateObject>();
        Doughnuts_EC_SpacePositions = new List<Transform>();
        Park_EC_SpacePositions = new List<Transform>();
        Factory_EC_SpacePositions = new List<Transform>();
        audioSource = GetComponent<AudioSource>();
        eventCap1 = eventCapsule1.GetComponent<rotateObject>();
        eventCap2 = eventCapsule2.GetComponent<rotateObject>();
        NP_1 = NeighboorPuzzle_1.GetComponent<PuzzleTimer>();
        NP_2 = NeighboorPuzzle_2.GetComponent<PuzzleTimer>();
        bg = bgmusic.GetComponent<BGMusic>();
        writeJason = writeJsonObj.GetComponent<WriteJson>();
        playerPositionList = new List<Vector3>();
        mpc = MainPuzlleControllerObject.GetComponent<MainPuzzleController>();


        for (int i =0; i< Dgroub.childCount; i++){
            Doughnuts_EC_SpacePositions.Insert(i, Dgroub.GetChild(i).transform);
        }
        for (int i = 0; i < Pgroub.childCount; i++){
            Park_EC_SpacePositions.Insert(i, Pgroub.GetChild(i).transform);
        }
        for (int i = 0; i < Fgroub.childCount; i++){
            Factory_EC_SpacePositions.Insert(i, Fgroub.GetChild(i).transform);
        }
        for (int i = 0; i < this.gameObject.name.Length; i++){
            if (Char.IsDigit(this.gameObject.name[i])){
                selfIDString += this.gameObject.name[i];
                MyName = this.gameObject.name.Split((char)selfIDString[0])[0];
                selfID = int.Parse(selfIDString);
            }
        }
        nextID = selfID + 1;

        MyPuzzleInfo = mpc.GetType().GetMethod(MyName + selfID + "_Boolean");
        NextPuzzleInfo = mpc.GetType().GetMethod(MyName + nextID + "_Boolean");

        before = selfID - 1 >= 1 ? selfID - 1 : 3;
        after = selfID + 1 <= 3 ? selfID + 1 : 1;

    }
    private bool GetPuzzleActiveInfo(MethodInfo Method, bool PuzzleBool){
        var getMyPuzzleBool = Method.Invoke(mpc, null);
        PuzzleBool = Convert.ToBoolean(getMyPuzzleBool);
        return PuzzleBool;
    }
    private bool DoesMyNeighboorPlayMusic(MethodInfo Method,PuzzleTimer Neighboor,bool PuzzleBool){
        var getMyPuzzleBool = Method.Invoke(Neighboor, null);
        PuzzleBool = Convert.ToBoolean(getMyPuzzleBool);
        return PuzzleBool;
    }
    private bool IsPlayerInsideMe(MethodInfo Method, PuzzleTimer Neighboor, bool PuzzleBool)
    {
        var getMyPuzzleBool = Method.Invoke(Neighboor, null);
        PuzzleBool = Convert.ToBoolean(getMyPuzzleBool);
        return PuzzleBool;
    }
    private int GetMyNeighboorID(MethodInfo Method, PuzzleTimer Neighboor, int NeighboorID)
    {
        var getMyNeighboorID = Method.Invoke(Neighboor, null);
        NeighboorID = Convert.ToInt32(getMyNeighboorID);
        return NeighboorID;
    }


    // Update is called once per frame
    private void FixedUpdate() {
        isEmergent = mpc.isEmergentBool();
        WaterPumpBool = mpc.WaterPumpIsPumping;
        basketCounter = mpc.basketCollection;
        if (!GetValuesOnce){
            if (isEmergent){
                nextID = 4;
            }

            MyPuzzleInfo = mpc.GetType().GetMethod(MyName + selfID + "_Boolean");
            NextPuzzleInfo = mpc.GetType().GetMethod(MyName + nextID + "_Boolean");


            IsAPuzzlePlaying_1 = NP_1.GetType().GetMethod(MyName + before + "_isPlaying");
            IsPlayerInside_1 = NP_1.GetType().GetMethod(MyName + before + "_isInside");
            GetNeighboorID_1 = NP_1.GetType().GetMethod(MyName + before + "_MyID");

            IsAPuzzlePlaying_2 = NP_2.GetType().GetMethod(MyName + after + "_isPlaying");
            IsPlayerInside_2 = NP_2.GetType().GetMethod(MyName + after + "_isInside");
            GetNeighboorID_2 = NP_2.GetType().GetMethod(MyName + after + "_MyID");


            GetValuesOnce = true;
        }
        if (!isEmergent){
            NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);
            MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle) && !NextPuzzle;
        }else{
            NextPuzzle = GetPuzzleActiveInfo(NextPuzzleInfo, NextPuzzle);
            MyPuzzle = GetPuzzleActiveInfo(MyPuzzleInfo, MyPuzzle);

            PlayerInsideBefore = IsPlayerInsideMe(IsPlayerInside_1,NP_1,PlayerInsideBefore);
            PlayerInsideAfter = IsPlayerInsideMe(IsPlayerInside_2,NP_2,PlayerInsideAfter);

            NeighboorBeforeID = GetMyNeighboorID(GetNeighboorID_1,NP_1,NeighboorBeforeID);
            NeighboorAfterID = GetMyNeighboorID(GetNeighboorID_2,NP_2,NeighboorAfterID);
        }
        ECC_2 = points.Sum();
       // Debug.Log("ECC_2 sum: " + ECC_2);
        CanIPlayMusic = !DoesMyNeighboorPlayMusic(IsAPuzzlePlaying_1, NP_1, CanIPlayMusic) && !DoesMyNeighboorPlayMusic(IsAPuzzlePlaying_2, NP_2, CanIPlayMusic);
        eventCap1.canPlay = CanIPlayMusic && !audioSource.isPlaying && bg.IamPlayingTheThemeSong();
        eventCap2.canPlay = CanIPlayMusic && !audioSource.isPlaying && bg.IamPlayingTheThemeSong();
        if (!MyPuzzle && !writeOnce && playerPositionList.Count > 0){
          
            if (selfID == 1){
                writeJason.writeJason1 = true;
            }
            else if (selfID == 2){
                writeJason.writeJason2 = true;
            }
            else if (selfID == 3){
                writeJason.writeJason3 = true;
            }
           
            writeOnce = true;
        }
        if (MyPuzzle && isInside){            
            fixedCounter++;          
        }
        if (!MyPuzzle && !isInside && isEmergent){
            
            if (PlayerInsideBefore)
            {               
                if (NeighboorBeforeID == 1)
                { // before 2 = 1 
                    Debug.Log("before " + gameObject.name + " Line 221");
                    // this is event cap 2 this works
                    if (!eventCap1.IamTriggered())
                    {
                        eventCapsule1.transform.position = Doughnuts_EC_SpacePositions[selfID - 2].position;
                    }
                    if (!eventCap2.IamTriggered())
                    {
                        eventCapsule2.transform.position = Doughnuts_EC_SpacePositions[selfID].position;
                    }
                }
                if (NeighboorBeforeID == 2)
                { // before 3 = 2 
                  // this is event cap 3
                    Debug.Log("before " + gameObject.name + " Line235");
                    if (!eventCap1.IamTriggered())
                        eventCapsule1.transform.position = selfID==1? Park_EC_SpacePositions[selfID - 1].position: Park_EC_SpacePositions[selfID - 2].position;

                    if (!eventCap2.IamTriggered())
                        eventCapsule2.transform.position = selfID == 1 ? Park_EC_SpacePositions[selfID + 1].position : Park_EC_SpacePositions[selfID ].position;
                }
                if (NeighboorBeforeID == 3)//this works
                { // before 3 = 1     
                    Debug.Log("before " + gameObject.name + " Line 244");
                    if (!eventCap1.IamTriggered())
                        eventCapsule1.transform.position = Factory_EC_SpacePositions[selfID - 1].position;

                    if (!eventCap2.IamTriggered())
                        eventCapsule2.transform.position = Factory_EC_SpacePositions[selfID + 1].position;
                }
            }
            if (PlayerInsideAfter)
            {             
                if (NeighboorAfterID == 1)// this works
                {
                    // event cap 3
                    if (!eventCap1.IamTriggered())
                    {
                        eventCapsule1.transform.position = Doughnuts_EC_SpacePositions[selfID - 2].position;
                    }
                    if (!eventCap2.IamTriggered())
                    {
                        eventCapsule2.transform.position = Doughnuts_EC_SpacePositions[selfID].position;
                    }
                }
                if (NeighboorAfterID == 2)
                {              
                    // Event cap 1                  
                    if (!eventCap1.IamTriggered())
                        eventCapsule1.transform.position = Park_EC_SpacePositions[0].position;

                    if (!eventCap2.IamTriggered())
                        eventCapsule2.transform.position = Park_EC_SpacePositions[2].position;
                }
                if (NeighboorAfterID == 3)
                { 
                    // Event cap 2 this works
                    if (!eventCap1.IamTriggered())
                        eventCapsule1.transform.position = Factory_EC_SpacePositions[selfID - 1].position;

                    if (!eventCap2.IamTriggered())
                        eventCapsule2.transform.position = Factory_EC_SpacePositions[selfID + 1].position;

                }
            }          
        }
    }


    bool newEventCapsules;
    private void OnTriggerStay(Collider other){
       /*if (MyPuzzle && other.tag == transform.GetChild(0).tag && other.)
        {
            Debug.Log("NEW CHILD !! " + other.name);
            other.transform.SetParent(this.transform);
        }*/
        if (MyPuzzle && other.tag == "Player"){
            isInside = true;
            playerPosition = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            PlayerTimer();
           
            if (!audioSource.isPlaying && !hasPlayed && CanIPlayMusic && bg.IamPlayingTheThemeSong() && isEmergent)
            {
                audioSource.PlayOneShot(emergentStart);
                hasPlayed = true;
            }

            if (isEmergent) {

                if (transform.childCount != names.Count) {
                    for (int i = 0; i < transform.childCount; i++) {

                        if (!names.Contains(transform.GetChild(i).name)) {
                            names.Insert(0, transform.GetChild(i).name);
                            childscripts.Insert(0, transform.GetChild(i).GetComponent<rotateObject>());
                            points.Insert(0, transform.GetChild(i).gameObject.GetComponent<rotateObject>().ECC);
                        }
                    }

                }
                for (int i = 0; i < childscripts.Count; i++) {                    
                    if(childscripts[i].IamTriggered())
                    {
                        points[i] = 1;
                    }                                            
                }                              
            }
        }
        /*if(isEmergent && other.tag=="Player"){
            for(int i =0; i<points.Count; i++){
                ECC_2 += points[i];
            }
            if(ECC_2> points.Count){
                ECC_2 = 0;
            }
        }*/
    }
    bool once=false;
    bool onceScript=false;
    List<GameObject> eventList;
    List<int> points;
    List<string> names;
    List<rotateObject> childscripts;
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"){
            isInside = false;
            hasPlayed = false;
            once = false;
            newEventCapsules = false;
        }
    }
    private void PlayerTimer(){
        if (fixedCounter % Mathf.Round(1f / Time.fixedDeltaTime) == 0){
            PlayerTimeCounter++;
            playerPositionList.Insert(0, playerPosition);
        }
    }

    public bool startP1_isPlaying(){
        return audioSource.isPlaying;
    }
    public bool startP2_isPlaying()
    {
        return audioSource.isPlaying;
    }
    public bool startP3_isPlaying()
    {
        return audioSource.isPlaying;
    }
    public bool startP1_isInside()
    {
        return isInside && MyPuzzle;
    }
    public bool startP2_isInside()
    {
        return isInside && MyPuzzle;
    }
    public bool startP3_isInside()
    {
        return isInside && MyPuzzle;
    }

    public int startP1_MyID(){
        return selfID;
    }
    public int startP2_MyID()
    {
        return selfID;
    }
    public int startP3_MyID()
    {
        return selfID;
    }

}
