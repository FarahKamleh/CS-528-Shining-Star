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

    // the volume slider
    public Slider sliderVol;

    // toggles for the above
    public Toggle light;
    public Toggle stars;

    // Start is called before the first frame update
    void Start()
    {
        // make sure One More Light plays at the start
        light.isOn = true;
    }

    // a function that will be called for toggle presses
    public void SwitchAudio(int selected)
    {
        // if Light is on, play audio
        if (light.isOn == true && selected == 1)
        {
            // stop the audio source
            smAudioSource.Stop();

            // turn the other toggle off
            stars.isOn = false;

            // play the correct song
            smAudioSource.PlayOneShot(LinkinPark);
        }

        // if Stars toggle is on, play audio
        if (stars.isOn == true && selected == 2)
        {
            // stop the audio source
            smAudioSource.Stop();

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

    // function to control volume
    public void volumeChanger()
    {
        // make the audio source's volume that of the slider divided by ten
        smAudioSource.volume = sliderVol.value / 10.0f;
    }
}