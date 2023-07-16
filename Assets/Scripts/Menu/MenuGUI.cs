using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
public class MenuGUI : MonoBehaviour
{

    public GameObject PauseScreenGUI;
    public GameObject SettingsScreenGUI;
    public GameObject MenuScreenGUI;
    public GameObject QuistionarScreen;
    public GameObject QuistionarContinue;
    public GameObject QuistionarEndGame;
    public GameObject EmergentGUI;
    public GameObject LinierGUI;
    public AudioSource click;
    public GameObject StartVideo;
    public VideoPlayer vp;


    public bool PauseBool = false;
    private bool MenuBool = false;
    private static string StartSceneName = "menu";
    private int escCounter = 0;
    public static bool EmergentGame;
    private static bool setGameOnce = false;
    static float gameType;
    static int GameCompletionCounter = 0;
    GameObject MainPuzzleControllerObject;
    static GameObject SaveData;
    static WriteJson WJD;
    MainPuzzleController mpc;
    bool runOnce=false;
    static bool StaticRunOnce = false;
    bool p1Active;
    bool p2Active;
    bool p3Active;
    bool gameIsCompleted;
    public static double UniqueID = 0;
    public MenuGUI My_instance;
    bool startSceneOnce = false;
    // Start is called before the first frame update
    void Start() {

        MenuBool = true;

        startSceneOnce = true;
       // MenuScreen();
    }
    private void Awake() {
        /* if (My_instance == null)
         {
             My_instance = this;
             Debug.LogError(My_instance);
         }else{
             Destroy(this.gameObject);
         }

         */
        DontDestroyOnLoad(gameObject);

    }
    public static void setGameType() {
        if (!setGameOnce) {
            setGameOnce = true;
            gameType = Random.value;


            EmergentGame = gameType > 0.5f ? true : false;
            //EmergentGame = true;
            Debug.LogError("value above 0.5 is Emergent: " + EmergentGame);
            if (UniqueID==0f) {
                UniqueID = Random.Range(1, 1000) + gameType;
                Debug.LogError("UniqeID " + UniqueID);
            }
        }else{
            EmergentGame = !EmergentGame;
            Debug.LogError(" setGameOnce: EmergentGame: " + EmergentGame);
        }
    }
    public double getUniqeID(){
        return UniqueID;
    }
    public bool getGameType() {
        return EmergentGame;
    }

    // Update is called once per frame
    void Update() {

        if (SceneManager.GetActiveScene().name == StartSceneName && startSceneOnce)
        {
           

            setGameType();
            setGameOnce = true;
            runOnce = true;
            Debug.LogError("game type is Emergernt: " + getGameType());

            // if 
            
            if (GameCompletionCounter != 0 && GameCompletionCounter < 3)
            {
                QuistionarEndScreen();
            }else{
                
                MenuScreen();
            }
            GameIsCompleted();
            Debug.LogError("GameCompletionCounter: " + GameCompletionCounter);
            startSceneOnce = false;
            
            



        }


        if (SceneManager.GetActiveScene().name != StartSceneName && Input.GetKeyDown(KeyCode.Escape)){
            pauseBtn();
            runOnce = false;
            
           


        }else if(SceneManager.GetActiveScene().name != StartSceneName ) {
            if (!runOnce){
                
                p1Active = mpc.startP1;
                p2Active = mpc.startP2;
                p3Active = mpc.startP3;
                MainPuzzleControllerObject = GameObject.FindGameObjectWithTag("MainPuzzleController");
                SaveData = GameObject.FindGameObjectWithTag("WriteData");
                //WJD = SaveData.GetComponent<WriteJasonData>();
                mpc = MainPuzzleControllerObject.GetComponent<MainPuzzleController>();
                runOnce = true;
            }
            startSceneOnce = true;
            //WJD.Emergent = getGameType();
            //Debug.LogError("my bool: " + getGameType() + " WJD.Emergent: " + WJD.Emergent);
            //gameIsCompleted = mpc.gameFinish;

            /* if (mpc.gameFinish)
             {
                 GameIsCompleted();
             }*/
        }

    }
    public void ClickSound(){
        if(!click.enabled){
            click.enabled = true;
        }else{
            click.Play();
        }
        
        /*if(!click.isPlaying){
            click.enabled = false;
        }*/
    }
    public void loadGame() {

        SceneManager.LoadSceneAsync(1);
        resumeBtn();
    }
    public void backBtn() {

        if (PauseBool) {
            PauseScreen();
        } else {
            MenuScreen();
        }
    }

    public void QuistionarEndScreen(){
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        Cursor.lockState = CursorLockMode.None;
        QuistionarScreen.SetActive(true);
        Debug.LogError(" GameCompletionCounter: " + GameCompletionCounter);
        if (GameCompletionCounter % 2==1){
            StartVideo.SetActive(true);
            if(!vp.isPlaying){
                StartVideo.SetActive(false);
            }
            QuistionarContinue.SetActive(true);
            QuistionarEndGame.SetActive(false);
        
        }else{
            StartVideo.SetActive(true);
            if (!vp.isPlaying)
            {
                StartVideo.SetActive(false);
            }
            QuistionarContinue.SetActive(false);
            QuistionarEndGame.SetActive(true);
        }
    }

    public void pauseBtn() {

        escCounter++;
        if (escCounter % 2 != 0) {
            PauseScreen();
        } else {
            resumeBtn();
        }
    }
    public void resetPuzzle(){
        if(p1Active && !p2Active && !p3Active){
            mpc.ExecuteCommandFunction("p1");

        }else if (p1Active && p2Active && !p3Active){
            mpc.ExecuteCommandFunction("p2");

        }else if (p1Active && p2Active && p3Active){
            mpc.ExecuteCommandFunction("p3");
        }
        resumeBtn();

    }
    void GameIsCompleted(){
        GameCompletionCounter++;
        Debug.LogError(" load game gameIsCompleted " + gameIsCompleted);
       // SceneManager.LoadSceneAsync(0);
    }
    public void resumeBtn(){

        MenuScreenGUI.SetActive(false);
        SettingsScreenGUI.SetActive(false);
        PauseScreenGUI.SetActive(false);
        QuistionarScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        if(Time.timeScale != 1){
            Time.timeScale = 1;
        }
        if (PauseBool) {
            PauseBool = false;
        }       
    }

    public void MenuScreen(){

        //setGameType();
        GameCompletionCounter = 0;
        Cursor.lockState = CursorLockMode.None;
        MenuScreenGUI.SetActive(true);        
        SettingsScreenGUI.SetActive(false);
        PauseScreenGUI.SetActive(false);
        QuistionarScreen.SetActive(false);
        Debug.LogError("###### EMERGENT :" + EmergentGame);
        if (EmergentGame)
        {
            Debug.LogError("I AM EMERGENT!!!!");
            EmergentGUI.SetActive(true);
            LinierGUI.SetActive(false);
        }else if (!EmergentGame){
            EmergentGUI.SetActive(false);
            LinierGUI.SetActive(true);

        }
    }

    public void SettingsScreen(){

        SettingsScreenGUI.SetActive(true);
        MenuScreenGUI.SetActive(false);
        PauseScreenGUI.SetActive(false);
    }

    public void PauseScreen(){
    
        Cursor.lockState = CursorLockMode.None;
        PauseScreenGUI.SetActive(true);
        SettingsScreenGUI.SetActive(false);
        MenuScreenGUI.SetActive(false);
        if (Time.timeScale != 0){
            Time.timeScale = 0;           
        }
        if(!PauseBool){
            PauseBool = true;
        }
    }

    public bool GetEmergentType(){
        return EmergentGame;
    }

    public void exitGame(){

        Debug.LogError("Game is exiting_ only works in a Build");
        Application.Quit();
        
    }
}
