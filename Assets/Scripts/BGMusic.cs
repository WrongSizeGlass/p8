using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
public class BGMusic : MonoBehaviour
{
    public AudioClip MainTheme;
    public AudioClip PuzzleCompleted;
    public AudioClip GameFinnish;
    public AudioClip SolutionDonut, SolutionPark, SolutionFactory;
    private AudioSource audio;

    public GameObject MainPuzzleControllerObject;
    private MainPuzzleController mpc;

    private bool P1Once =false;
    private bool P2Once =false;
    private bool P3Once =false;
    public GameObject skip;
    public GameObject p1Timer;
    public GameObject p2Timer;
    public GameObject p3Timer;
    PuzzleTimer p1;
    PuzzleTimer p2;
    PuzzleTimer p3;
    bool HasPlayedP1=false;
    bool HasPlayedP2=false;
    bool HasPlayedP3=false;
    bool P1Start;
    bool P2Start;
    bool P3Start;
    bool once = false;
    bool ICanPlay;
    bool theme = false;
    bool videoIsplaying;
    public GameObject videoObj;
    bool dougnatDone =false;
    bool ParkDone =false;
    //Raw Image to Show Video Images [Assign from the Editor]
    public RawImage image;
    //Video To Play [Assign from the Editor]
    public VideoClip IntroVideo;
    public VideoClip Donut_finnished;
    public VideoClip Park_finnish;
    public VideoClip videoToPlay_Factory;
    public VideoClip EmergentParkIntro;
    public VideoClip EmergentFactoryIntro;
    public VideoClip ExitClip;
    bool EmergentFactoryIntroBool = false;
    bool EmergentParkIntroBool = false;
    private VideoPlayer videoPlayer;
    private VideoSource videoSource;
    bool endIsPlaying = false;
    public VideoPlayer introplayer;
    bool introIsplaying = true;
    //Audio
    private AudioSource audioSource;
    public GameObject panel;
    bool canPlay1;
    bool canPlay2;
    bool canPlay3;
    // Start is called before the first frame update
    void Start()
    {
        
        p1 = p1Timer.GetComponent<PuzzleTimer>();
        p2 = p2Timer.GetComponent<PuzzleTimer>();
        p3 = p3Timer.GetComponent<PuzzleTimer>();
        endIsPlaying = false;
        Application.runInBackground = true;
        //videoObj.SetActive(false);
        image.enabled = false;

        mpc = MainPuzzleControllerObject.GetComponent<MainPuzzleController>();
        audio = GetComponent<AudioSource>();
        audio.volume = 0.15f;
        audio.loop = true;
        audio.PlayOneShot(MainTheme);
        introplayer.clip = IntroVideo;
        introIsplaying = true;

    }

    // Update is called once per frame
    void Update()
    {
        // these 2 booleans cover p1 p2 p3 !isPlaying 
        ICanPlay = p1.CanIPlayMusic && p2.CanIPlayMusic && IamPlayingTheThemeSong();
       // Debug.Log("Intro is playing " + introplayer.time);

        if(introplayer.clip == IntroVideo && !introplayer.isPlaying && introIsplaying && introplayer.time>2 || Input.GetKey("return") && introplayer.clip == IntroVideo)
        {
            introIsplaying = false;
                //  introplayer.enabled = false;
                panel.SetActive(false);
            
        }
        //Debug.Log("should factory be played ? " +( !introIsplaying && mpc.isEmergentBool() && !EmergentFactoryIntroBool && !introplayer.isPlaying));
        //factory start
        if ( !introIsplaying  && mpc.isEmergentBool() && !EmergentFactoryIntroBool && !introplayer.isPlaying )
        {
           
            panel.SetActive(true);
            EmergentFactoryIntroBool = true;
            //  introplayer.enabled = true;
            introplayer.clip = EmergentFactoryIntro;
            Debug.Log("playing park!!____ ");
            introplayer.time = 0;

        }
       
        //park start

        if (!introplayer.isPlaying && mpc.isEmergentBool() && EmergentParkIntroBool)
        {

            if (mpc.skip())
            {
                skip.SetActive(false);
            }
            skip.SetActive(false);
            panel.SetActive(true);
            EmergentParkIntroBool = true;
            //  introplayer.enabled = true;
            introplayer.clip = EmergentParkIntro;
            Debug.Log("playing park!!____ ");

        }
        

      

      /*  if(!videoIsplaying){
            StartCoroutine(playVideo_Donut());
        }*/
        if (mpc.Dcounter>4&& !P1Once ){

            audio.Stop();
            theme = false;
            audio.volume = 0.5f;
            audio.loop = false;
            audio.PlayOneShot(PuzzleCompleted);

            // Dougnat cutscne
            image.enabled = true;
           // StartCoroutine(playVideo_Donut());

            if (ICanPlay && mpc.isEmergentBool() && !introplayer.isPlaying) {
                Invoke("p1_finished", 3);
                HasPlayedP1 = true;
                theme = false;
            }
            P1Once = true;
        }else if(mpc.basketCollection>1 && !P2Once || mpc.WaterPumpIsPumping && !P2Once)
        {          
            audio.Stop();
            theme = false;
            audio.volume = 0.5f;
            audio.loop = false;
            audio.PlayOneShot(PuzzleCompleted);

            // park cutscene
            image.enabled = true;
           // StartCoroutine(playVideo_Park());


            if (ICanPlay && mpc.isEmergentBool()&& !introplayer.isPlaying)
            {
                Invoke("p2_finished", 3);
                HasPlayedP2 = true;
            }
            P2Once = true;
        }else if(mpc.p3com && !P3Once )
        {
            audio.Stop();
            theme = false;
            audio.volume = 0.5f;
            audio.loop = false;
            audio.PlayOneShot(PuzzleCompleted);

            if (ICanPlay && mpc.isEmergentBool()  && !introplayer.isPlaying) {      
                Invoke("p3_finished", 3);
                HasPlayedP3 = true;
                theme = false;
            }
            P3Once = true;
        }else{

            if (!audio.isPlaying)
            {
                audio.volume = 0.15f;
                audio.loop = true;
                audio.PlayOneShot(MainTheme);
                theme = true;
            }
        }
        // park off
        if (introplayer.clip == EmergentParkIntro && !introplayer.isPlaying && EmergentParkIntro && introplayer.time > 2 && mpc.isEmergentBool() || Input.GetKey("return") && mpc.isEmergentBool() && EmergentParkIntroBool && introplayer.clip == EmergentParkIntro && introplayer.time > 1) 
        {
            EmergentParkIntroBool = false;
            EmergentFactoryIntroBool = true;
            //  introplayer.enabled = false;
            panel.SetActive(false);
        }
        // factory off
        if (introplayer.clip == EmergentFactoryIntro && !introplayer.isPlaying  && introplayer.time > 2 && EmergentFactoryIntroBool && mpc.isEmergentBool() || Input.GetKey("return") && mpc.isEmergentBool() && introplayer.clip == EmergentFactoryIntro && EmergentFactoryIntroBool && introplayer.time > 1)
        {
            EmergentParkIntroBool = true;
            //  introplayer.enabled = false;
            panel.SetActive(false);
        }
        //  Debug.Log("P1Once " + P1Once + " dougnatDone " + dougnatDone);
        if (P1Once  && !dougnatDone && !mpc.isEmergentBool() && !introplayer.isPlaying)
        {
            panel.SetActive(true);
          //  introplayer.enabled = true;
            introplayer.clip = Donut_finnished;
            Debug.Log("playing donut!!____ ");

        }

        if (P2Once && !ParkDone && !mpc.isEmergentBool() && !introplayer.isPlaying)
        {
            panel.SetActive(true);
            //  introplayer.enabled = true;
            introplayer.clip = Park_finnish;
            Debug.Log("playing park!!____ ");

        }

        if (introplayer.clip == Donut_finnished && !introplayer.isPlaying && introplayer.time > 2 && !dougnatDone && P1Once && !mpc.isEmergentBool() || Input.GetKey("return") && P1Once && !mpc.isEmergentBool())
        {
            Debug.Log("stop playing donuts");
            dougnatDone = true;
          //  introplayer.enabled = false;
            panel.SetActive(false);
        }

        if (introplayer.clip == Park_finnish && !introplayer.isPlaying && introplayer.time > 2 && !ParkDone && P2Once && !mpc.isEmergentBool()  || Input.GetKey("return") && P2Once && !mpc.isEmergentBool())
        {
            Debug.Log("stop playing park");
            ParkDone = true;
            //  introplayer.enabled = false;
            panel.SetActive(false);
        }
        
        if (P1Once && !HasPlayedP1 &&  ICanPlay && mpc.isEmergentBool()  && !introplayer.isPlaying)
        {
            Invoke("p1_finished", 3);
            HasPlayedP1 = true;
        }
        if (P2Once && !HasPlayedP2 &&  ICanPlay && mpc.isEmergentBool() && !introplayer.isPlaying)
        {
            Invoke("p2_finished", 3);
            HasPlayedP2 = true;
        }
        if (P3Once && !HasPlayedP3 && ICanPlay && mpc.isEmergentBool() && !introplayer.isPlaying)
        {
            Invoke("p3_finished", 3);
            HasPlayedP3 = true;
        }
      
        if(IamPlayingTheThemeSong() && mpc.canEXIT() && ICanPlay){
            audio.Stop();
            panel.SetActive(true);
            endIsPlaying = true;
            stop = true;

            introplayer.clip = ExitClip;
        }
        if (introplayer.clip == ExitClip && !introplayer.isPlaying && introplayer.time > 2 || Input.GetKey("return") && introplayer.clip == ExitClip)
        {
           
            exitOnce = true;
            Debug.Log("stop playing park");
            ParkDone = true;
            //  introplayer.enabled = false;
            panel.SetActive(false);
            SceneManager.LoadSceneAsync(0);
            stop = false;
            
        }
        skip.SetActive(exitOnce);
    }
    bool stop=false;
   static bool exitOnce = false;
    public bool IamPlayingTheThemeSong(){
        return audio.loop;

    }
    // Donut
    void p1_finished()
    {

        if (!introplayer.isPlaying && !stop)
        {
            audio.Stop();
            audio.loop = false;
            audio.volume = 0.75f;
            audio.PlayOneShot(SolutionDonut);
        }
    }
    // Park
    void p2_finished()
    {
        if (!introplayer.isPlaying  && !stop)
        {
            audio.Stop();
            audio.loop = false;
            audio.volume = 0.75f;
            audio.PlayOneShot(SolutionPark);
        }
    }
    // Factory
    void p3_finished()
    {
        if (!introplayer.isPlaying && !stop) {
            audio.Stop();
            audio.loop = false;
            audio.volume = 0.75f;
            audio.PlayOneShot(SolutionFactory);
        }
    }
    
}
