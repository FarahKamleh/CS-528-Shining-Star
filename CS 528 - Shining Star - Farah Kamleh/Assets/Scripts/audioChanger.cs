using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioChanger : MonoBehaviour
{
    // refer to both audio clips that will be switched
    public AudioClip LinkinPark;
    public AudioClip Skillet;

    // the audio source to be used
    public AudioSource smAudioSource;

    // toggles for the above
    public Toggle light;
    public Toggle stars;

    // Start is called before the first frame update
    void Start()
    {
        // make sure One More Light plays at the start
        light.isOn = true;
    }

    // FIXME: a function that will be called for toggle presses
    public void SwitchAudio()
    {
        // if Light is on, play audio
        if (light.isOn == true)
        {
            // stop the audio source
            // smAudioSource.Stop();

            // turn the other toggle off
            stars.isOn = false;

            // play the correct song
            smAudioSource.PlayOneShot(LinkinPark);
        }

        // if Stars toggle is on, play audio
        if (stars.isOn == true)
        {
            // stop the audio source
            // smAudioSource.Stop();

            // turn the other toggle off
            light.isOn = false;

            // play the correct song
            smAudioSource.PlayOneShot(Skillet);
        }

        // if all toggles are off, 
        else if ((light.isOn == false) && (stars.isOn == false))
        {
            smAudioSource.Stop();
        }
    }
}