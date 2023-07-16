using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPump : MonoBehaviour
{
    private bool pumping;
    private Outline ol;
    float y;
    float starty;
    public GameObject MyPump;
    public GameObject QuestPump;
    // Start is called before the first frame update
    void Start()
    {
        MyPump.SetActive(false);
        ol = GetComponent<Outline>();
        y = transform.position.y - 0.1f;
        starty = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime, Space.Self);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "pump")
        {
            pumping = true;
            GetComponent<MeshRenderer>().enabled = false;
            QuestPump.SetActive(false);
            MyPump.SetActive(true);
            
        }
       
    }
    public bool isPumping(){
        return pumping;
    }
   /* private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "pump")
        {
            ispumping = false;


        }
    }*/
}
