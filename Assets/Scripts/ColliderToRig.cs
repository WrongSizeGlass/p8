using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderToRig : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform newPosRot;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = newPosRot.position;
        //transform.rotation = newPosRot.rotation;
    }
}
