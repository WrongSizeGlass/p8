using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointColumn : MonoBehaviour
{
    List<Rigidbody> childrenRb;
    List<float> Distance;
    //List<Vector3> DistanceOffset;
    public float spring = 0.1f;
    public float damp = 0.5f;
    public Rigidbody player;
    float DistanceOffset;
    Vector3 TargetVelocity;
    Vector3 VectorProjection;
    Vector3 connection;
    // Start is called before the first frame update
    void Start()
    {
        Distance = new List<float>();
        childrenRb = new List<Rigidbody>();
        //  DistanceOffset = new List<Vector3>();
        //  TargetVelocity = new List<Vector3>();
        //   VectorProjection = new List<Vector3>();
       
        for(int i=0; i<transform.childCount; i++){
            childrenRb.Insert(0, transform.GetChild(i).GetComponent<Rigidbody>());
           
            
        }
        Debug.Log(childrenRb.Count);
        for (int i =0; i<childrenRb.Count; i++){

            Debug.Log(childrenRb[i].name);
            if (i!=0) {
                Distance.Insert(0, Vector3.Distance(childrenRb[i].position, player.position));
            }else{
                Distance.Insert(0, Vector3.Distance(childrenRb[i].position, childrenRb[i-1].position));
            }
          
        }
        Distance.Reverse();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        for (int i=0; i< childrenRb.Count; i++) {


            if (i != 0)
            {
                connection = childrenRb[i].position - childrenRb[i-1].position;
                
            }else{

                connection = childrenRb[i].position - player.position;
              


            }
            DistanceOffset = Distance[i] - connection.magnitude;
            childrenRb[i].position -= DistanceOffset * connection.normalized;
            TargetVelocity = connection + (childrenRb[i].velocity + Physics.gravity * spring);
            VectorProjection = Vector3.Project(TargetVelocity, connection);
            childrenRb[i].velocity = (TargetVelocity - VectorProjection) / (1 + damp * Time.fixedDeltaTime);
        }


    }
}
