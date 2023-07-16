using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrezzY : MonoBehaviour
{
    RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }
    float y = 0;
    // Update is called once per frame
    void Update()
    {

        
        if(rect.rotation.y<-90){
            rect.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);//rot(transform.rotation.x, y, transform.rotation.z);
        }else if(rect.rotation.y > 90)
        {
            
            rect.rotation = Quaternion.Euler(transform.rotation.x, -90, transform.rotation.z);
        }
    }
}
