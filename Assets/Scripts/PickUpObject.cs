using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{

    private Puzzle1Controller p1c;
    public GameObject controller;
    private Outline ol;
    private bool onlyOnce = false;
    private AudioSource audio;
    bool start = false;
    public Vector3 rotation;
    public float angle;
    private void Start()
    {
        p1c = controller.GetComponent<Puzzle1Controller>();
        ol = GetComponent<Outline>();
        audio = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider collider)
    {


        if (collider.gameObject.tag == "Player" && !onlyOnce)
        {
            audio.pitch = Random.Range(0.7f, 1.2f);
            audio.Play();
            p1c.collection = p1c.collection + 1;
            onlyOnce = true;
            start = true;
            //StopPlayAudio();
            ol.OutlineMode = (Outline.Mode)1;
        }
    }
    private void OnTriggerExit(Collider other)
    {
       // StopPlayAudio();
        if (transform.position == new Vector3(0, -100, 0)) { onlyOnce = false; }
    }
    void StopPlayAudio(){
        
    }
    void Update(){
        transform.Rotate(rotation * angle * Time.deltaTime, Space.Self);
        if (start && !audio.isPlaying)
        {
            transform.position = new Vector3(0, -100, 0);
        }
    }
    void OnTriggerStay(Collider collider)
    {
        /*Debug.Log("abc!!!" + collider.tag);
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("abc!!!" + collider.tag);
            print("Item picked up");
            transform.position = new Vector3(Random.Range(-54, 66), 0, Random.Range(22, 192));
        }*/
    }
}
