using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAniScript : MonoBehaviour
{
    Animator ani;
    basicmovement bm;
    [SerializeField] public GameObject player;
    [SerializeField] private GameObject playerlead;

    // needs to be shared to player movement script
    public CapsuleCollider runnerbody;
    public SphereCollider walkbody;
    public float offsetY;
    //private Collider CurrentCollider;

    private AnimatorStateInfo animationInfo;
    private AnimatorClipInfo[] animationClip;

    string walk = "Walk";
    string run = "RunAnimation";
    string jump = "Jump";
    string defaultIdle = "IdleAnimation";
    string startCrouch = "Start_Crouch";
    string idleCrouch = "Idle_Crouch";
    string walkCrouch = "Crouch_Walk";
   [HideInInspector] public string currentstate;

    public bool isAnimating = false;
    public bool crouch = false;
    private bool onGround = false;
    private bool running = false;
    private bool jumping = false;
    private bool crouching = false;
    private bool walking = false;
    private bool IsWalking = false;
    private bool criticalAniDone = true;

    private Rigidbody rb;

    private float timedelay = 0.2f;
    private float AnimationTimeLength = 0;
    private const float JumpTime = 0.717f;
    private const float startCrouchTime = 0.264f;
    Vector3 newPos;
    private int CrouchCounter = 0;
    private int FixedCounter = 0;
    private int JumpCounter = 0;
    private int CrouchStartCounter = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = player.GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        bm = player.GetComponent<basicmovement>();
        runnerbody = runnerbody.GetComponent<CapsuleCollider>();
        walkbody = walkbody.GetComponent<SphereCollider>();
        currentstate = defaultIdle;

    }

    void FixedUpdate()
    {
        newPos = playerlead.transform.position;
        newPos.y = newPos.y - offsetY;
        transform.position = newPos;
        ani.SetFloat("Vertical", Input.GetAxis("Vertical"));
        ani.SetFloat("Horizontal", Input.GetAxis("Vertical"));
        FixedCounter++;
        onGround = bm.onSurface();
        criticalAniDone = currentstate == idleCrouch || currentstate == defaultIdle || currentstate == walk || currentstate == walkCrouch || currentstate == run;
        // following try catch is to ensure that animations suchs as jumping completes before the next animation
        try
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName(jump) || ani.GetCurrentAnimatorStateInfo(0).IsName(startCrouch))
            {
                animationInfo = ani.GetCurrentAnimatorStateInfo(0);
                animationClip = ani.GetCurrentAnimatorClipInfo(0);

                AnimationTimeLength = animationClip[0].clip.length * animationInfo.normalizedTime;

                if (AnimationTimeLength >= JumpTime && currentstate == jump)
                {
                    currentstate = defaultIdle;
                    AnimationTimeLength = 0;
                    JumpCounter = 0;
                    bm.playJumpAnimation = false;

                }
                else if (AnimationTimeLength >= startCrouchTime && currentstate == startCrouch)
                {

                    currentstate = idleCrouch;
                    AnimationTimeLength = 0;

                }
                else { currentstate = currentstate; }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("exception " + e);

            if (currentstate == jump)
            {
                currentstate = defaultIdle;
                JumpCounter = 0;
                bm.playJumpAnimation = false;

            }
            else if (currentstate == startCrouch)
            {
                currentstate = idleCrouch;
            }
        }

        running = Input.GetKey(KeyCode.LeftShift);
        crouching = Input.GetKey(KeyCode.LeftControl);
        jumping = Input.GetAxis("Jump") != 0;
        walking = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;

        AnimationController();

        crouch = CrouchCounter % 2 == 0 ? false : true;
        if (crouch) { Debug.LogError("player ani "+crouch); }

      //  Debug.LogError(crouch);
        walkbody.enabled = IsWalking;
        runnerbody.enabled = !IsWalking;
        //ani.PlayInFixedTime(currentstate, 0);
        /*if (rb.velocity.magnitude>2) {
            isAnimating = true;
            Debug.LogError(rb.velocity.magnitude);
        }else{
            isAnimating = false;
        }*/
        ani.Play(currentstate,0);
       // isAnimating = ani.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    public bool GetCurrentCollider()
    {
        return IsWalking ? true : false;
    }
    void AnimationController()
    {

        // following if statement changes animation states.
        if (FixedCounter % Mathf.Round(timedelay / Time.fixedDeltaTime) == 0 && Input.anyKey && criticalAniDone){
            animationState();
            FixedCounter = 0;

        }
        //Following if else if, is to prevent jump/cruch animaion locks.
        if (!Input.anyKey && crouch && criticalAniDone && onGround){
          //  timedelay = 0.1f;
            currentstate = idleCrouch;
            FixedCounter = 0;

        }else if (!Input.anyKey && !crouch && criticalAniDone&& onGround){

            timedelay = 0.1f;
            currentstate = defaultIdle;
            FixedCounter = 0;
        }
    }

    void animationState(){


        if (walking && !running && !jumping && !crouch && !crouching && onGround){
            IsWalking = true;
            //timedelay = 0.1f;
            currentstate= walk;

        }else if (walking && running && !jumping && !crouching && onGround){
            IsWalking = false;
            if (crouch)
            {
                CrouchCounter++;
            }
            //timedelay = 0.1f;
            currentstate = run;

        }else if ( bm.playJumpAnimation){
            IsWalking = false;
            
            if (crouch){
                CrouchCounter++;
               
            }
            // this might end up be redundant
            JumpCounter++;
            //timedelay = 0.2f;
            currentstate = jump;

            //Crounch animation
        }else if (crouching  && onGround){

            CrouchCounter++;
            //timedelay = 0.2f;
            if (!crouch) {
                currentstate = startCrouch;
            }

        }else if (crouch && !walking && !running && !jumping && onGround ){
            //timedelay = 0.2f;
            currentstate = idleCrouch;

        }else if (crouch && walking && !running && !jumping && onGround){
           // timedelay = 0.1f;
            currentstate = walkCrouch;
        }
    }
}
