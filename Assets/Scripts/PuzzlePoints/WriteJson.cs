using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.SceneManagement;
public class PuzzleData
{

    public double UniqueID;
    public string Name;
    public int Time;
    public bool _Emergent;
    public int EventCapsuleCounter = 0;
    public int BasketCounter = 0;
    public bool WaterIsPumping = false;
    public List<string> Player_Positions;

    public PuzzleData(double UniqueID, string Name, int Time, bool Emergent, int EventCapsuleCounter)
    {
        this.UniqueID = UniqueID;
        this.Name = Name;
        this.Time = Time;
        this._Emergent = Emergent;
        this.Player_Positions = new List<string>();
        this.EventCapsuleCounter = EventCapsuleCounter;
    }
}

public class CollectSavedData
{
    public List<PuzzleData> CollectionList;
    public CollectSavedData()
    {

        this.CollectionList = new List<PuzzleData>();
    }

}

public class WriteJson : MonoBehaviour
{
    private static string path = "SaveFile.json";

    public GameObject GUIMenu;
    private MenuGUI MGUI;
    public bool Emergent = false;
    private bool MyEmergent = false;
    public GameObject p1TimerObject;
    public GameObject p2TimerObject;
    public GameObject p3TimerObject;
    private GameObject GUI;
    private static MenuGUI mGUI;
    private PuzzleTimer p1timer;
    private PuzzleTimer p2timer;
    private PuzzleTimer p3timer;

    private bool p1once = false;
    private bool p2once = false;
    private bool p3once = false;

    static CollectSavedData csd;
    public PuzzleData P1Data = null;
    public PuzzleData P2Data = null;
    public PuzzleData P3Data = null;

    List<string> MyList;
    List<string> p2pos;
    List<string> p3pos;

    
    /*public static PuzzleData P1Data_2 = null;
    public static PuzzleData P2Data_2 = null;
    public static PuzzleData P3Data_2 = null;*/

    static bool staticOnce = false;
    static bool staticWriteOnce = false;
    private bool JustOnce = false;
    public bool writeJason1 = false;
    public bool writeJason2 = false;
    public bool writeJason3 = false;
    static int indexCounter = 0;
    static double MyUniqueID;
    void Start()
    {

        GUI = GameObject.FindGameObjectWithTag("GUI");
        mGUI = GUI.GetComponent<MenuGUI>();

        MyList = new List<string>();
        // p2pos = new List<string>();
        //  p3pos = new List<string>();
        p1timer = p1TimerObject.GetComponent<PuzzleTimer>();
        p2timer = p2TimerObject.GetComponent<PuzzleTimer>();
        p3timer = p3TimerObject.GetComponent<PuzzleTimer>();
        p1once = false;
        p2once = false;
        p3once = false;


    }

    static bool getGameType()
    {
        return mGUI.getGameType();
    }
    static double getUniqueID()
    {
        return mGUI.getUniqeID();
    }

    public bool CanSkipCutScenes()
    {
        return csd.CollectionList.Count == 3;

    }

    private void Update()
    {



        if (!staticOnce)
        {
            MyUniqueID = getUniqueID();
            csd = new CollectSavedData();
            staticOnce = true;
        }
       
        
        // WritePuzzleDataBool(0);
        MyEmergent = getGameType();
        Debug.LogError("j1:" + writeJason1 + " j2: " + writeJason2 + " j3 " + writeJason3);

        if (!p1once && writeJason1)
        {
            if (P1Data == null && !p1once )
            {
                P1Data = new PuzzleData(MyUniqueID, p1timer.MyName + p1timer.selfID, p1timer.PlayerTimeCounter, MyEmergent, (p1timer.ECC_1 + p1timer.ECC_2));
                for (int i = 0; i < p1timer.playerPositionList.Count; i++)
                {
                    P1Data.Player_Positions.Insert(0, p1timer.playerPositionList[i].ToString());
                }
                csd.CollectionList.Insert(indexCounter, P1Data);
                indexCounter++;
                p1once = true;
                Debug.Log("event capsule score: " + (p1timer.ECC_1 + p1timer.ECC_2));
            }
        }
        if (!p2once && writeJason2)
        {
            if (P2Data == null && !p2once )
            {
                P2Data = new PuzzleData(MyUniqueID, p2timer.MyName + p2timer.selfID, p2timer.PlayerTimeCounter, MyEmergent, (p2timer.ECC_1 + p2timer.ECC_2));
                for (int i = 0; i < p2timer.playerPositionList.Count; i++)
                {
                    P2Data.Player_Positions.Insert(0, p2timer.playerPositionList[i].ToString());
                }
                P2Data.BasketCounter = p2timer.basketCounter;
                P2Data.WaterIsPumping = p2timer.WaterPumpBool;
                csd.CollectionList.Insert(indexCounter, P2Data);
                indexCounter++;
                p2once = true;
                Debug.Log("event capsule score: " + (p2timer.ECC_1 + p2timer.ECC_2));
            }
        }
        if (!p3once && writeJason3)
        {
            if (P3Data == null && !p3once )
            {
                P3Data = new PuzzleData(MyUniqueID, p3timer.MyName + p3timer.selfID, p3timer.PlayerTimeCounter, MyEmergent, (p3timer.ECC_1 + p3timer.ECC_2));
                for (int i = 0; i < p3timer.playerPositionList.Count; i++)
                {
                    P3Data.Player_Positions.Insert(0, p3timer.playerPositionList[i].ToString());
                }
                csd.CollectionList.Insert(indexCounter, P3Data);
                indexCounter++;
                p3once = true;
            }
        }
         if(csd.CollectionList.Count == 3 )
        {
            canExit = true;
         }if(csd.CollectionList.Count >3 && csd.CollectionList.Count <=5)
        {
            canExit = false;
         }
        if (csd.CollectionList.Count == 6 && !staticWriteOnce)
        {
            writeJson(csd);
            staticWriteOnce = true;
        }

    }
    static bool oneTime = false;
    public bool Exit(){
        return canExit;
    }
    public static bool canExit = false;
    static void writeJson(CollectSavedData ClassData)
    {
        Debug.LogError("Writing Jason");
        JsonData newData = new JsonData();

        newData = JsonMapper.ToJson(ClassData);

        string data = newData.ToString();

        File.WriteAllText(Application.dataPath + path, data);

        Debug.LogError(" Json is done ");
        //SceneManager.LoadSceneAsync(0);
        canExit = true;

    }
}


