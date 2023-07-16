using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckScript : MonoBehaviour
{
    public Transform p3Pos;
    //public bool start;
    private Vector3 startPos;
   // private Vector3 self;
    // Start is called before the first frame update
    void Start()
    {
        //self = GetComponent<Transform>();
        startPos = this.transform.position;
        //Debug.LogError("(__ " + startPos + " __)" );
    }

    public void startTrucks(){
        transform.position = Vector3.Lerp(transform.position, p3Pos.position, Time.deltaTime * 0.25f);       
    }
    public void resetTrucks(){
        transform.position = startPos;
    }
}
