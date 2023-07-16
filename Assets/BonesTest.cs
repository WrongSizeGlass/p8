using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonesTest : MonoBehaviour
{
    public Transform JointGroup;
    Transform myPos;
    Transform copyTarget;
    // Start is called before the first frame update
    void Start()
    {
        myPos = GetComponent<Transform>();
        for (int i =0; i<JointGroup.childCount; i++){

            if(JointGroup.GetChild(i).position == myPos.position)
            {
                copyTarget = JointGroup.GetChild(i);
            }

        }
        if(copyTarget==null){
            Debug.Log("NO COPY TARGET " + this.name);
        }//else{ copyTarget.SetParent(this.transform); }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (copyTarget!=null) {
            
            myPos.position = copyTarget.position;
            //myPos.rotation = copyTarget.rotation;
        }
    }
}
