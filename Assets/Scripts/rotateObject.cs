using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class rotateObject : MonoBehaviour
{
    MeshRenderer mr;
    MeshCollider mc;
    public int ECC = 0;
    private bool isTrigged;
    public GameObject MainPuzzleController;
    MainPuzzleController mpc;
    AudioSource audio;
    public float vol = 0.5f;
    public int myID = 0;
    public bool canPlay;

    List<float> motherCalculator;
    public List<Transform> mothers;
    float lowestValue;
    int lowestIndex;
    Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        audio = GetComponent<AudioSource>();
        mpc = MainPuzzleController.GetComponent<MainPuzzleController>();
        mr = GetComponent<MeshRenderer>();
        mc = GetComponent<MeshCollider>();
        motherCalculator = new List<float>();
        for (int i=0; i<mothers.Count; i++){
            //Debug.Log(mothers[i].transform.position);
            motherCalculator.Insert(i, Vector3.Distance(transform.position, mothers[i].transform.position));
        }
        lowestValue = motherCalculator.Min();
        for (int i =0; i<motherCalculator.Count; i++){
            if(motherCalculator[i]==lowestValue){
                lowestIndex = i;
            }
        }
        transform.SetParent(mothers[lowestIndex]);
      //  Debug.Log("My mother is " + mothers[lowestIndex].name);
      
        
        audio.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(mpc.isEmergentBool())
        {
            if (!isTrigged) {
                Appear();
                transform.Rotate(Vector3.up * 60 * Time.deltaTime, Space.Self);
               // Debug.Log("I am at start pos? " +(startPos == transform.position) + " name " + this.name);
                if(startPos != transform.position){
                    for (int i = 0; i < motherCalculator.Count; i++)
                    {
                        //Debug.Log(mothers[i].transform.position);
                        motherCalculator[i]= Vector3.Distance(transform.position, mothers[i].transform.position);
                    }
                    lowestValue = motherCalculator.Min();
                    for (int i = 0; i < motherCalculator.Count; i++)
                    {
                        if (motherCalculator[i] == lowestValue)
                        {
                          //  Debug.Log("new mother number = " + i );
                            lowestIndex = i;
                        }
                    }
                    transform.SetParent(mothers[lowestIndex]);
                   // Debug.Log(this.name + " my new mother is " + mothers[lowestIndex].name);
                    if (transform.parent.name == mothers[lowestIndex].name) {
                        startPos = transform.position;
                    }
                }

            }
        }else{
            Disapear();
        }
        
        
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player"){
            if (canPlay) {
                isTrigged = true;
                ECC = 1;
                Disapear();
                audio.volume = vol;
                audio.enabled = true;
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(this.name +" " + other.name);
    }
    void Disapear(){
        mr.enabled = false;
        mc.enabled = false;
    }
    void Appear(){
        mr.enabled = true;
        mc.enabled = true;
    }
    public bool IamTriggered(){
        return isTrigged;
    }
}
