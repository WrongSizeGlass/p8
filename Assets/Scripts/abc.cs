using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class abc : MonoBehaviour
{

    [HideInInspector] public Rigidbody rb;


    [SerializeField] private GameObject GrabPos;

    [Range(0.2f, 10)] public float jumpSpeed = 0.5f;
    [Header("Speed")]
    [Tooltip("start Speed is the start value, max speed is x2 of the start speed, crouch speed is start speed/2")]
    [Range(0.5f, 10)] public float speed = 5;

    [Header("added jump force to player")]
    [Range(0.0f, 1.5f)] public float jumpForceIncrements = 0.2f;

    [Header("Jump speed incremential time mili sec")]
    [Range(0.0f, 1)] public float jumpIncrements = 0.1f;


    [Header("max increments depended on jumpincrements see tooltip")]
    [Tooltip("if jumpincrements is 0.1 and maxincrements max jump height will be achived after 1 sec")]
    public int maxIncrements = 10;

    [Header("Sound Files")]
    [Tooltip("for sound")]


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
    private float JumpBarUnits = 0.000f;
    private float velY = 1;

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

        downDirection = Vector3.down;
        startSpeed = speed;
        maxSpeed = startSpeed * 2;

        crouchSpeed = startSpeed / 2;
        startWeight = rb.mass;
        weight = startWeight;
        startJumpSpeed = jumpSpeed;
  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter++;
        forward = Input.GetAxis("Vertical") > 0;
        backward = Input.GetAxis("Vertical") < 0;
        movement = Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0;
      
        playerPos = rb.position;

        move();
      
      
    }
    void move()
    {
        vertical = Input.GetAxis("Vertical");

        horizontal = Input.GetAxis("Horizontal") ;
        jump = Input.GetAxis("Jump");
        if (vertical!=0||horizontal!=0) {
            rb.MovePosition(rb.position + (transform.forward * vertical) * speed * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + (transform.right * horizontal) * speed * Time.fixedDeltaTime);
        }
        else{
            rb.velocity = Vector3.zero;
        }
        /*  if (!audio.isPlaying && onSurface() && movement && rb.velocity.magnitude>0)
          {

              //audio.Stop();
              walkSoundTest = walkSound;
              audio.PlayOneShot(walkSoundTest);


          }else{
              walkSoundTest = null;
          }*/

    }
  
}
