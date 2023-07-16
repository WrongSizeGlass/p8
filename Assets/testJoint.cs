using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testJoint : MonoBehaviour
{
    public Rigidbody playerRb;
    Rigidbody myRb;
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //myRb.MovePosition(playerRb.position);
    }
}
