using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class VOIP : MonoBehaviour
{
 
    //Audio source with serialized field for networking
    [SerializeField]
    private AudioSource source;
 
    //Key to use for push to talk functionality
    public KeyCode pushToTalkKey;
 
    //So we can know when the player is talking
    private bool isPlaying;
 
 
    void Start ()
    {
        //Set isn't playing at start
        this.isPlaying = false;
 
        //Get the audio source
        source = GetComponent<AudioSource> ();
 
    }
 
    void FixedUpdate ()
    {
        //If the push to talk key is pressed down, it usese the mic and returns
        if (Input.GetKeyDown (pushToTalkKey)) {
            this.UseMic (true, RadioDistance.CLOSE);
            return;
        }
 
        //If the key is pushed up, it stops recording voice and returns
        if (Input.GetKeyUp (pushToTalkKey)) {
            this.UseMic (false, RadioDistance.CLOSE);
            return;
        }
    }
 
    //Method to use the microphone and play the sound
    public void UseMic (bool useMic, RadioDistance qual)
    {
 
        //TODO: Set the sampling rate based on distace
        int samplingRate = 48000;
 
        if (qual == RadioDistance.VERYFAR) {
            samplingRate = 2000;
        } else if (qual == RadioDistance.FAR) {
            samplingRate = 4000;
        } else if (qual == RadioDistance.CLOSE) {
            samplingRate = 20000;
        } else if (qual == RadioDistance.VERYCLOSE) {
            samplingRate = 48000;
        }
 
        //Play if we're using our mic
        if (useMic) {
            //Tell the script we're using the mic
            this.isPlaying = true;
            //Get the clip from the mic from the default device and continue to 'loop' it while it's played
            source.clip = Microphone.Start (null, true, 1, samplingRate);
            //Make sure we're looping the source
            source.loop = true;
 
            //While the microphone is active, play the audioclip
            while (!(Microphone.GetPosition (null) > 0)) {
                source.Play ();
            }
 
 
 
        } else {
            //If we're not using our mic make sure we're not playing
            this.isPlaying = false;
 
            //Make sure that the audio clip is stopped
            source.Stop ();
 
            //Make sure the clip is null again
            source.clip = null;
        }
    }
}

public enum RadioDistance
{
    VERYFAR,
    FAR,
    CLOSE,
    VERYCLOSE
 
}
