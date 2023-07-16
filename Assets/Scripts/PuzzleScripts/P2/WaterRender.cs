using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRender : MonoBehaviour
{
    public int waterCounter;
    public GameObject middleWater;
    public GameObject topWater;
    public GameObject waterpump;
    public GameObject puzzle2Controller;
    private Puzzle2Controller p2c;
    MeshRenderer bottomRender;
    MeshRenderer middleRender;
    MeshRenderer topRender;
    Outline BR;
    Outline MR;
    Outline TR;
    AudioSource AS;
    WaterPump wp;
    private bool Pumping;
    // Start is called before the first frame update
    void Start()
    {
        p2c = puzzle2Controller.GetComponent<Puzzle2Controller>();
        bottomRender = GetComponent<MeshRenderer>();
        BR = GetComponent<Outline>();
        AS = GetComponent<AudioSource>();
        AS.enabled = false;
        middleRender = middleWater.GetComponent<MeshRenderer>();
        MR = middleWater.GetComponent<Outline>();
        topRender = topWater.GetComponent<MeshRenderer>();
        TR = topWater.GetComponent<Outline>();
        wp = waterpump.GetComponent<WaterPump>();
       // p2c = puzzle2Controller.GetComponent<Puzzle2Controller>();
        
    }
    public bool isPumping(){
        return wp.isPumping();
    }
    // Update is called once per frame
    void Update()
    {
        Pumping = isPumping();
        if (waterCounter > 0)
        {
            bottomRender.enabled = true;
            BR.enabled = true;
        }
        if (waterCounter > 1)
        {
            middleRender.enabled = true;
            topRender.enabled = true;
            MR.enabled = true;
            TR.enabled = true;
        }
      
        if (FountainComplete()){
            AS.enabled = true;
            bottomRender.enabled = true;
            middleRender.enabled = true;
            topRender.enabled = true;
            BR.enabled = true;
            MR.enabled = true;
            TR.enabled = true;
        }
        
    }

    public void RenderWater (bool turnOn){
        if (!turnOn) {
            bottomRender.enabled = turnOn;
            middleRender.enabled = turnOn;
            topRender.enabled = turnOn;
            BR.enabled = turnOn;
            MR.enabled = turnOn;
            TR.enabled = turnOn;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "basket")
        {
            if (other.gameObject.GetComponent<BascketScript>().IHaveWater()) {
                waterCounter +=2;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "basket")
        {
            waterCounter--;
        }
    }

    public bool FountainComplete()
    {
        if (p2c.skipLvl) { waterCounter = 2; }
        if (waterCounter > 1) { return true; }
        if (isPumping()) { return true; }
        return false;       
    }
}
