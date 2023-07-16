using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetRotPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    Transform rot;
    float x, y, z,w;
    void Start()
    {
        rot = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        x = 0;y = rot.rotation.y;z = rot.rotation.z;w = rot.rotation.w;
        rot.rotation = new Quaternion(x,y,z,w);
    }
}
