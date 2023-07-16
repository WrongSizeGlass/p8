using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class basicmovement : MonoBehaviour
{
    public PlayerAniScript pas;
    [HideInInspector] public Rigidbody rb;
    public GameObject InteractCanvas;
    public Image JumpBarGUI;
    public Image JumpOverlayGUI;

    [SerializeField] private GameObject GrabPos;

    [Range(0.2f, 10)] public float jumpSpeed = 0.5f;
    [Header("Speed")]
    [Tooltip("start Speed is the start value, max speed is x2 of the start speed, crouch speed is start speed/2")]
    [Range(0.5f, 5)] public float speed = 5;

    [Header("added jump force to player")]
    [Range(0.0f, 1.5f)] public float jumpForceIncrements = 0.2f;

    [Header("Jump speed incremential time mili sec")]
    [Range(0.0f, 1)] public float jumpIncrements = 0.1f;


    [Header("max increments depended on jumpincrements see tooltip")]
    [Tooltip("if jumpincrements is 0.1 and maxincrements max jump height will be achived after 1 sec")]
    public int maxIncrements = 10;

    [Header("Sound Files")]
    [Tooltip("for sound")]
    
    public AudioClip QuickJumpSound;
    public AudioClip LongJumpSound;
    public AudioClip GrabbingSound;
    public AudioClip LandingSound;
    private AudioSource audio;

    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public Rigidbody grabbedObject;

    private CapsuleCollider BodyCollider;

    private Vector3 playerPos;
    private Vector3 downDirection;
    private Vector3 grabDirection;
    private Vector3 raypos;
    bool landingBool;

    private float downDisRange;
    private float downDis = 0.1f;
    private float jump = 0;
    private float vertical = 0;
    private float horizontal = 0;
    private float startSpeed = 5;
    private float maxSpeed = 10;
    private float crouchSpeed = 1f;
    private float grabY;
    private float startWeight;
    private float weight;
    private float startJumpSpeed = 0f;
    private float JumpBarUnits=0.000f;
    private float velY = 1;

    [HideInInspector] public bool grabbing;
    [HideInInspector] public bool playJumpAnimation;
    private bool forward;
    private bool backward;
    private bool crouching;
    private bool flying;
    private bool startJumpCounter;
    private bool canJump = false;
    private bool fall = false;
    private bool jumpCountTest;
    private bool jumpIsMaxed;
    private bool movement;
    private int grabCounter = 0;

    private int increments = 0;
    private int counter;
    private int multiplyierX = 1;
    private int multiplyierZ = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        downDirection = Vector3.down;
        startSpeed = speed;
        maxSpeed = startSpeed * 2;
        BodyCollider = GetComponent<CapsuleCollider>();
        downDisRange = BodyCollider.GetComponent<Collider>().GetComponent<Collider>().bounds.extents.y + downDis;
        crouchSpeed = startSpeed / 2;
        startWeight = rb.mass;
        weight = startWeight;
        startJumpSpeed = jumpSpeed;
        horiMax = startSpeed * 1.6f;              
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        counter++;
        forward = Input.GetAxis("Vertical") > 0;
        backward = Input.GetAxis("Vertical") < 0;
        movement = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
        raypos = BodyCollider.transform.position;
        raypos.y = raypos.y + 0.32f;// 927692
        crouching = pas.crouch;
        playerPos = rb.position;

        move();
        grab();
        jumping(jumpIncrements);     
        preventFlying();
        if(!onSurface() ){
        landingBool = false;
        }

        if(onSurface() && !landingBool){
            if (!audio.isPlaying) {
                audio.PlayOneShot(LandingSound);
            }
            landingBool = true;
            //audio.volume = 1;
        }
        if(transform.position.y<0 && !onSurface()){
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
    }

    void move()
    {
        vertical = Input.GetAxis("Vertical") * runSpeed();

        horizontal = Input.GetAxis("Horizontal") * runSpeed();
        jump = Input.GetAxis("Jump");
        if (!rayHitStop())
        {
            if (!rayHit())
            {
                 rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime); 
                 rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1 * Time.fixedDeltaTime); 
        
            }
            else if (rayHit() && !forward)
            {
                 rb.MovePosition(rb.position + (transform.right * vertical) * runSpeed() * Time.fixedDeltaTime); 
                 rb.MovePosition(rb.position + (transform.forward * horizontal) * runSpeed() * -1 * Time.fixedDeltaTime);
                
            }
            else{
                if (speed>0) { speed--; }
            }

        }else if(rayHitStop() && !forward)
        {
            rb.MovePosition(rb.position + (transform.right * vertical) * 3* Time.fixedDeltaTime);

            rb.MovePosition(rb.position + (transform.forward * horizontal) * 1.1f * -1 * Time.fixedDeltaTime);
       
        }
        else { rb.velocity = Vector3.zero; Debug.LogWarning(hit.collider.name); speed--;  }
      /*  if (!audio.isPlaying && onSurface() && movement && rb.velocity.magnitude>0)
        {

            //audio.Stop();
            walkSoundTest = walkSound;
            audio.PlayOneShot(walkSoundTest);

            
        }else{
            walkSoundTest = null;
        }*/

    }
   
    void preventFlying()
    {
        if (jump>0){
            jumpCountTest = true;
        }
        if(jumpCountTest && jump==0){
            if (counter % Mathf.Round(1.5f / Time.fixedDeltaTime) == 0 && startJumpCounter && !onSurface() && grabbing)
            {
                flying = false;
                startJumpCounter = false;
                counter = -10;
                fall = true;
                jumpCountTest = false;
            }
        }        
        if (fall && onSurface())
        {
            fall = false;
        }
        if (fall && vertical != 0 || horizontal != 0 && fall)
        {
            rb.velocity = new Vector3(rb.velocity.x, Physics.gravity.y + rb.velocity.y, rb.velocity.z);
        }

    }
    void jumping(float increaseForcePrMiliSec)
    {
        float a=0;
        if (jump > 0 && onSurface())
        {
            canJump = true;
            flying = true;
        }

        if (canJump)
        {
            if (!JumpBarGUI.enabled || !JumpOverlayGUI.enabled)
            {
                JumpBarGUI.enabled = true;
                JumpOverlayGUI.enabled = true;
            }
            if ((counter/4) % Mathf.Round(increaseForcePrMiliSec / Time.fixedDeltaTime) == 0 && jump > 0 && canJump && increments < maxIncrements && !Input.GetKey(KeyCode.LeftShift))
            {
                jumpSpeed += jumpForceIncrements;
                increments++;
                JumpBarUnits = Mathf.Lerp(0, 1, increments*increaseForcePrMiliSec);
               // Debug.LogError("a= "+ JumpBarUnits);
            }
            JumpBarGUI.fillAmount = JumpBarUnits;
        }
       
        if (jump == 0 && canJump)
        {
            if( increments>=maxIncrements/2){ // if jumpbar>50%
                audio.volume = 0.5f;
                audio.PlayOneShot(LongJumpSound);
            }else{
                audio.volume = 0.5f;
                audio.PlayOneShot(QuickJumpSound);
            }

            if(grabbing){
                jumpSpeed = jumpSpeed+1f;
            }
            playJumpAnimation = true;
            rb.velocity = new Vector3(0, jumpSpeed, 0);
            jumpSpeed = startJumpSpeed;
            increments = 0;
            canJump = false;
            counter = 0;
            startJumpCounter = true;
            JumpBarGUI.enabled = false;
            JumpOverlayGUI.enabled = false;
            JumpBarGUI.fillAmount = 0;
            JumpBarUnits = 0;

        }
    }

    void grab()
    {    
        float GuiHeight;
       
        float y =  Physics.gravity.y + rb.velocity.y;
        //GUI
        if (Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f) && hit.collider.attachedRigidbody && grabbedObject == null && !prevent && onSurface() && !grabbing && !Input.GetKey(KeyCode.E) && hit.collider.tag!="car" && hit.collider.tag != this.tag)
        {
            InteractCanvas.SetActive(true);
            Vector3 newVector = new Vector3(transform.position.x, Camera.main.transform.position.y, transform.position.z);
            //GuiHeight = hit.transform.position.y;
            
            if (Vector3.Distance(hit.point, newVector) <0.36 &&  hit.normal.y>0){
                InteractCanvas.transform.position = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
                
            }
            else{ 
                InteractCanvas.transform.position = Vector3.Lerp(InteractCanvas.transform.position,newVector, Time.deltaTime * 50f);

            }
            
           
           InteractCanvas.transform.LookAt(Camera.main.transform.position);



        }else{
            InteractCanvas.SetActive(false);
        }

        if (Input.GetKey(KeyCode.E))
        {
            if ((counter) % Mathf.Round(0.1f / Time.fixedDeltaTime) == 0)
            {
                grabCounter += 1;
            }
        }
       
        bool toggleGrab = grabCounter%2==1;

        if ( Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f) && toggleGrab && hit.collider.attachedRigidbody && grabbedObject==null &&!prevent && onSurface()){

            
            if (hit.collider.gameObject.GetComponent<Rigidbody>().mass < rb.mass + 110f && hit.collider.tag != this.tag)
            {
                grabbedObject = hit.collider.gameObject.GetComponent<Rigidbody>();
                audio.volume = 0.5f;
                audio.PlayOneShot(GrabbingSound);
               
                grabbedObject.freezeRotation = true;
                grabDirection = transform.position - grabbedObject.transform.position;
                if (onSurface())
                {
                    grabY = rb.position.y;
                }
                rb.position = new Vector3(rb.position.x, grabY, rb.position.z);
                Debug.LogWarning("Grabbing");
                grabbing = true;
            }
            audio.volume=1;
        }
        
        if (vertical != 0 && grabbing || horizontal != 0 && grabbing && grabbedObject != null){
            multiplyierX = hit.normal.x > 0 ? 1 : -1;
            multiplyierZ = hit.normal.z > 0 ? 1 : -1;
            if(!flying)
                rb.velocity = new Vector3(rb.velocity.x*multiplyierX, y, rb.velocity.z*multiplyierZ);
            grabbedObject.velocity = rb.velocity;
        }
         if (grabbing && grabbedObject != null){
            multiplyierX = hit.normal.x > 0 ? 1 : -1;
            multiplyierZ = hit.normal.z > 0 ? 1 : -1;
            if (!flying)
                rb.velocity = new Vector3(rb.velocity.x * multiplyierX, y, rb.velocity.z * multiplyierZ);
          
            rb.useGravity = true;
            grabbedObject.velocity = Vector3.zero;
            grabbedObject.useGravity = false;
            grabbedObject.transform.position = GrabPos.transform.position;
        }

        if (grabbedObject != null && !toggleGrab){
            grabbedObject.useGravity = true;
            grabbedObject.freezeRotation = false;
            grabbing = false;
            grabbedObject = null;
            rb.mass = startWeight;
        }
       // Debug.Log("grabCounter is "+grabCounter);
    }  
    float horiMax;
    public bool prevent = false;
    float diffMaxSpeed;
    private float runSpeed()
    {
        diffMaxSpeed=maxSpeed;
        if (!crouching)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !grabbing)
            {
                speed++;
                diffMaxSpeed = maxSpeed;               
            } 
            else
            {
                speed--;
                diffMaxSpeed = maxSpeed;
            }
            if(grabbing ){            
                diffMaxSpeed = maxSpeed;
                speed =  startSpeed+0.25f;
            }           
        }else{         
            speed = crouchSpeed;
        }
        if(speed> startSpeed+0.25f){
            prevent = true;
        }else{
            prevent = false;
        }
        if(vertical!=0 && horizontal!=0){
            diffMaxSpeed = horiMax;
        }
        speed = Mathf.Clamp(speed, startSpeed, diffMaxSpeed);
        return speed;
    }

    public bool rayHit()
    {
        Debug.DrawRay(raypos, Camera.main.transform.forward);
        try
        {
            if (Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f) && hit.collider.attachedRigidbody)
            {
                return false;
            }
            else
            {
                return Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f);
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }
    public bool rayHitStop()
    {
        Debug.DrawRay(raypos, Camera.main.transform.forward);
        try
        {
            if (Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f) && hit.collider.attachedRigidbody)
            {
                return false;
            }
            else
            {
                return Physics.Raycast(raypos, Camera.main.transform.forward, out hit, 0.4f);
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public bool onSurface()
    {
        //                      origin point,   direction   maxDis
        return Physics.Raycast(playerPos, downDirection, downDisRange);

    }
    public bool objectInfront()
    {
        //                      origin point,   direction   maxDis
        return Physics.Raycast(playerPos, Vector3.forward, 0.1f);

    }
    
}
