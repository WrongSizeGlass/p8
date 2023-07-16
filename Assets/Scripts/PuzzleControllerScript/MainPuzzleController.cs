using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainPuzzleController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Puzzle1Controller;
    public GameObject Puzzle2Controller;
    public GameObject Puzzle3Controller;

    public GameObject WriteJson;
    WriteJson json;
    private GameObject GUIObject;
    private MenuGUI GUI;

    public InputField command;

    private Puzzle1Controller p1c;
    private Puzzle2Controller p2c;
    private Puzzle3Controller p3c;

    [HideInInspector] public bool startP1, startP2, startP3;
    [HideInInspector] public bool gameFinish = false;
    [HideInInspector] public bool WaterPumpIsPumping = false;
    [HideInInspector] public int basketCollection = 0;
    [HideInInspector] public int Dcounter;
    [HideInInspector] public bool p3com;

    private bool executeCommand;

    public string textCommand;

    int enterCounter= 2;

    private bool closeInputField = false;

    public Transform cheatP2Pos;
    public Transform cheatP3Pos;
    public Transform player;

    bool p1Once = false;
    bool p2Once = false;
    bool p3Once = false;

    private string lastCommand="";

    private int fixedCounter=0;
    private int fixedCounter2=0;
    
    private bool canContinue = false;
    private bool once=false;
    private bool getGUIOnce = false;
    private bool isEmergent = false;
    private bool setStatesOnce = false;
    int test=0;
    bool EXIT=false;
    void Start()
    {
        json = WriteJson.GetComponent<WriteJson>();
        stop = true;
        setStatesOnce = false;
        p1c = Puzzle1Controller.GetComponent<Puzzle1Controller>();
        p2c = Puzzle2Controller.GetComponent<Puzzle2Controller>();
        p3c = Puzzle3Controller.GetComponent<Puzzle3Controller>();
        startP1 = false;
        startP2 = false;
        startP3 = false;
        command = command.GetComponent<InputField>();
        gameFinish = false;
        EXIT = false;
    }
    private void FixedUpdate()
    {
        fixedCounter++;
        fixedCounter2++;
    }
    // Update is called once per frame
    public bool startP1_Boolean(){
        
        return startP1;
    }
    public bool startP2_Boolean()
    {
        return startP2;
    }
    public bool startP3_Boolean()
    {
        return startP3;
    }
    public bool startP4_Boolean(){
        return gameFinish;
    }
    private void LinaerGame(){
        p3com = p3c.Puzzle3Complete();
        startP1 = true;
        Commands();
        if (p3c.Puzzle3Complete())
        {
            startP3 = true;
            fixedCounter = 0;

            if (startP1 && startP2 && startP3)
            {
                gameFinish = true;
                canContinue = true;
                once = true;
                stop = false;
                test = 0;
            }
            /*if (canContinue){
                fixedCounter2 = 0;
                canContinue = false;
               
                
            }*/
            if ( json.Exit()){
                fixedCounter2 = 0;
               
                Debug.LogError("gameFinish" + gameFinish);
                EXIT = true;
                //SceneManager.LoadSceneAsync(0);
            }
        }

        if (p1c.collection > 4 && !p1Once)
        {
            startP2 = true;
            p1Once = true;
            if (executeCommand){
                activateP2(startP2);
            }
        }

        if (p1c.collection > 4 && !p2Once){
            p2Once = true;  
        }

        if (p2c.puzzle2Complete()){
            startP3 = true;
            p3Once = true;          
            activateP3(startP3);
        }
        if (!executeCommand && Delay(0.2f)){
            activateP1(startP1);
            activateP2(startP2);
            activateP3(startP3);

        }else{
            activateP1(startP1);
            activateP2(startP2);
            activateP3(startP3);
        }
    }
    public bool canEXIT(){
        return EXIT;
    }
    void EmergentGame(){
        p3com = p3c.Puzzle3Complete();
        if (!setStatesOnce){
            startP1 = true;
            startP2 = true;
            startP3 = true;
            setStatesOnce = true;
        }
        activateP1(startP1);
        activateP2(startP2);
        activateP3(startP3);
     //   Debug.LogError("Puzzle 1 is active: " + startP1 + "  collection: " + p1c.collection + " out of 5");
     //   Debug.LogError("Puzzle 2 is active: " + startP2 + "  condition water is pumping: " + p2c.isPumping + " condition basket counter >1 ? :" + p2c.basketCounter);
    //    Debug.LogError("Puzzle 3 is active: " + startP3 + "  is the objective in place? " + p3c.Puzzle3Complete());
        if (Dcounter > 4 && !p1Once){
            startP1 = false;
            p1Once = true;
        }
        if (WaterPumpIsPumping && !p2Once){
            startP2 = false;
            p2Once = true;

        }else if (basketCollection > 1 && !p2Once){
            startP2 = false;
            p2Once = true;
        }
        if (p3c.Puzzle3Complete() && !p3Once){
            startP3 = false;
            p3Once = true;
        }
        if(startP1==false && startP2 == false && startP3 == false){
            Debug.Log("Everything is false");
            gameFinish = true;
        }      
        if (gameFinish && !canContinue){
            canContinue = true;
            test = 0;
            stop = false;
            
        }
        if (Delay2(3.0f) && gameFinish && json.Exit()){
            fixedCounter2 = 0;

            EXIT = true;
            //SceneManager.LoadSceneAsync(0);
        }
    }
    public bool isEmergentBool(){
        return isEmergent;
    }
    public bool skip(){
        return json.CanSkipCutScenes();
    }
    void Update()
    {
        
        Dcounter = p1c.collection;
        WaterPumpIsPumping = p2c.isPumping;
        basketCollection = p2c.basketCounter;
     //   Debug.Log("can exit " + json.Exit()) ;
        if (!getGUIOnce){
            GUIObject = GameObject.FindGameObjectWithTag("GUI");
            GUI = GUIObject.GetComponent<MenuGUI>();
            getGUIOnce = true;
        }
       
        
        isEmergent = GUI.getGameType();
        Debug.LogError("basketCounter = " + basketCollection);
        Debug.LogError(" is isPumping: " + WaterPumpIsPumping);

        if (isEmergent){
           // Debug.LogError("isEmergent####________####");
            EmergentGame();
        }
        else{
          //  Debug.LogError("Linear %%%%%______%%%%%%");
            LinaerGame();
        }
        
        //LinaerGame();
        /*p3com = p3c.Puzzle3Complete();
        Dcounter = p1c.collection;
        Commands();
        if (p3c.Puzzle3Complete())
        {
            if (!once)
            {
                canContinue = true;
                once = true;
            }
            if (canContinue)
            {
                fixedCounter2 = 0;
                canContinue = false;
            }
            if (Delay2(3.0f) && !canContinue)
            {
                
                fixedCounter2 = 0;
                gameFinish = true;
                Debug.LogError("gameFinish"+gameFinish);
                SceneManager.LoadSceneAsync(0);
            }
        }

        if (p1c.collection<4 && !p1Once){
            startP1 = true;
            p1Once = true;
        }
    
        if (p1c.collection > 4 && !p2Once)
        {
            p2Once = true;
            startP2 = true;
            if (executeCommand) {
                activateP2(startP2);
            }
        }

        if (p2c.puzzle2Complete())
        {
            p3Once = true;
            startP3 = true;
            fixedCounter = 0;
            activateP3(startP3);
        }

        if (!executeCommand&&Delay(0.2f)) {
            activateP1(startP1);
            activateP2(startP2);
            activateP3(startP3);
        }else{
            activateP1(startP1);
            activateP2(startP2);
            activateP3(startP3);
        }*/


    }

    bool Delay(float time){
        return fixedCounter % Mathf.Round(time/ Time.fixedDeltaTime) == 0;
    }
    bool stop = true;
    bool Delay2(float time)
    {
        if (fixedCounter2 % Mathf.Round((time/3) / Time.fixedDeltaTime) == 0 && !stop){
            test++;
        }
        if(test==3){
            stop = true;
        }
        return stop;
    }

    public void Commands(){
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            enterCounter++;
            
        }
        if (enterCounter%2!=0){
            Cursor.lockState = CursorLockMode.None;
            command.enabled = true;
            command.image.enabled = true;
            command.textComponent.enabled = true;

            ExecuteCommandFunction();
            if (closeInputField) {              
                closeInputField = false;
                enterCounter++;          
            }
            
        }else{
            if (Time.timeScale==1) {
                Cursor.lockState = CursorLockMode.Locked;
            }
            command.text = "";
            command.enabled = false;
            command.image.enabled = false;
            command.textComponent.enabled = false;
            
        }
    }
    public void activateP1(bool start){
        if (start) {
            p1c.startPuzzle1(start);
        }
    }
    public void activateP2(bool start){
        if (start) {

            p2c.startPuzzle2(start);
        } 
    }
    public void activateP3(bool start){
        if (start)
        {
            p3c.startPuzzle3(start);
        }
    }
    public void skipP1(){
        if(p1c.collection!=5){
            p1c.collection = 5;
        }        
    }

    public void ExecuteCommandFunction(string optional="_optional_"){
        if (optional != "_optional_"){
            lastCommand = optional;
            Debug.LogError(optional);

        }else{
            lastCommand = command.text;
        }
        switch (lastCommand)
        {
            case "p1":
                p1c.resetP1(true);
                activateP1(true);
                p2c.basketCounter = 0;
                p2c.isPumping = false;
                startP3 = false;
                activateP3(false);
                p3c.resetP3(false);
                executeCommand = true;
                closeInputField = true;
                
               
               


                break;

            case "p2":
                p1c.resetP1(false);
                startP3 = false;
                skipP1();
                p2c.resetP2(true);
                p2c.basketCounter = 0;
                p2c.isPumping = false;
                p3Once = false;
                activateP3(false);
                p3c.resetP3(false);
                executeCommand = true;
                closeInputField = true;
                break;

            case "p3":

                p2c.basketCounter = 2;
                p3c.resetP3(true);
                startP3 = true;
                p3c.stop = false;
                activateP3(true);

                executeCommand = true;
                closeInputField = true;
                break;

            case "reset":
                closeInputField = true;
                SceneManager.LoadScene(1);
                break;

            case "status_p1":
                Debug.LogError("Puzzle 1 is active: "+startP1 + "  collection: " +  p1c.collection + " out of 5");
                executeCommand = true;
                closeInputField = true;
                break;

            case "status_p2":
                Debug.LogError("Puzzle 2 is active: "+startP2 + "  condition water is pumping: "+ p2c.isPumping + " condition basket counter >1 ? :" + p2c.basketCounter);
                executeCommand = true;
                closeInputField = true;
                break;

            case "status_p3":
                Debug.LogError("Puzzle 3 is active: " + startP3+"  is the objective in place? " + p3c.Puzzle3Complete());
                executeCommand = true;
                closeInputField = true;
                break;

            case "status_all":
                Debug.LogError("Puzzle 1 is active: " + startP1 + "  collection: " + p1c.collection + " out of 5");
                Debug.LogError("Puzzle 2 is active: " + startP2 + "  condition water is pumping: " + p2c.isPumping + " condition basket counter >1 ? :" + p2c.basketCounter);
                Debug.LogError("Puzzle 3 is active: " + startP3 + "  is the objective in place? " + p3c.Puzzle3Complete());
                executeCommand = true;
                closeInputField = true;
                break;

            case "cheat_p2":
                player.position = cheatP2Pos.position;
                executeCommand = true;
                closeInputField = true;
                break;

            case "cheat_p3":
                player.position = cheatP3Pos.position;
                executeCommand = true;
                closeInputField = true;
                break;
         
            default:
               
                break;

        }
       
    }


}
