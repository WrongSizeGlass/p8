using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerWalkSound : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip crouchSound;
    private PlayerAniScript pas;
    private bool isWalking;
    private bool isRunning;
    private bool isCrouching;
    private bool start;
    
    [Range(0.1f, 0.5f)] public float minVol= 0.1f;
    [Range(0.2f, 0.75f)] public float maxVol= 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        pas = GetComponent<PlayerAniScript>();
    }

    // Update is called once per frame
    void Update()
    {
        isWalking = pas.currentstate == "Walk";
        isRunning = pas.currentstate == "RunAnimation";
        isCrouching = pas.currentstate == "Crouch_Walk";

        if (isWalking && !audio.isPlaying && !start) {
            start = true;
            audio.pitch = Random.Range(0.7f, 1.2f);
            audio.volume = Random.Range(minVol, maxVol);
            audio.PlayOneShot(walkSound);
            audio.Play();
        } else if (isRunning && !audio.isPlaying && !start) {
            start = true;
            audio.pitch = Random.Range(0.7f, 1.2f);
            audio.volume = Random.Range(minVol, maxVol);
            audio.PlayOneShot(runSound);
            audio.Play();
        } else if (isCrouching && !audio.isPlaying && !start){
            start = true;
            audio.pitch = Random.Range(0.7f, 1.2f);
            audio.volume = Random.Range(minVol, maxVol);
            audio.PlayOneShot(crouchSound);
            audio.Play();
        } else {
            if (!audio.isPlaying) {
                start = false;
                audio.Stop();
            }
        }
    }
}
