using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangesHeatMapColor : MonoBehaviour
{
    public int counter = 0;
    byte multiplyer = 50;
    Material mat;
    public string o = "";
    bool GreenOnce = false;
    bool BlueOnce = false;
    bool RedOnce = false;
    bool insert = true;
    //public List<string> neihboors;
    bool JustOnce = false;
   public int neighboor;
    public int dublicants;
    int nd;
    int Fixedcounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        //neihboors = new List<string>();
        mat = GetComponent<Renderer>().material;
        mat.SetColor("_BaseColor", new Color32(0, 100, 0, 75));
        // neihboors.Insert(0, gameObject.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("pos")) {
            neighboor++;
            
        }
    }

    private void FixedUpdate()
    {
        Fixedcounter++;
        if (!JustOnce && Fixedcounter % Mathf.Round(1f / Time.fixedDeltaTime) ==0) {
            ChangeColor();
           
            JustOnce = true;
        }
    }

    void ChangeColor(){
        counter = (neighboor+dublicants);
        // start green
        if( counter <10){
            mat.SetColor("_Color", new Color32(200, 200, 200, 25));
        }else
        if(counter >= 10 && counter < 20)
        {
            mat.SetColor("_Color", new Color32(225, 225, 225, 50));
        }
        else
        if (counter >= 20 && counter < 30)
        {
            mat.SetColor("_Color", new Color32(255, 255, 255, 75));
        }
        // start blue
        else if( counter >= 30 && counter < 40)
        {
            mat.SetColor("_Color", new Color32(0, 0, 150, 150));
        }
        else if (counter >= 40 && counter < 50)
        {
            mat.SetColor("_Color", new Color32(0, 0, 200, 150));
        }
        else if (counter >= 60 && counter < 70)
        {
            mat.SetColor("_Color", new Color32(0, 0, 255, 150));

        // start red
        }else if(counter >= 70 && counter < 80)
        {
            mat.SetColor("_Color", new Color32(150, 0, 0, 185));
        }
        else if (counter >= 80 && counter < 90)
        {
            mat.SetColor("_Color", new Color32(200, 0, 0, 200));
        }
        else if (counter >= 90 )
        {
            mat.SetColor("_Color", new Color32(255, 0, 0, 255));
        }else{
            mat.SetColor("_Color", new Color32(255, 0, 0, 255));
        }
    }

   /*void ColorChange(){

        counter = (byte)neighboor;
            Debug.Log("HEAT MAP counter = " + counter);

            byte newGreen = 100;
            byte newBlue = 100;
            byte newRed = 100;

            if (newGreen > 255)
            {

                newGreen = (byte)(counter + multiplyer);
                mat.SetColor("_BaseColor", new Color32(0, 255, 0, 0));

            }
            if (newGreen >= 255 && !GreenOnce)
            {
                GreenOnce = true;
                counter = 0;
            }
            if (newBlue < 255 && newGreen >= 255)
            {
                newBlue = (byte)(counter + multiplyer);
                mat.SetColor("_BaseColor", new Color32(0, 0, newBlue, 0));
            }
            if (newBlue >= 255 && !BlueOnce)
            {
                BlueOnce = true;
                counter = 0;
            }
            if (newBlue >= 255 && BlueOnce && newRed < 255)
            {
                newRed = (byte)(counter + multiplyer);
                mat.SetColor("_BaseColor", new Color32(0, 0, newRed, 0));
            }
            if (newBlue >= 255 && newGreen >= 255 && newRed >= 255)
            {
                RedOnce = true;
                mat.SetColor("_BaseColor", new Color32(255, 255, 255, 0));
            }
        
    }*/

    // Update is called once per frame

}
