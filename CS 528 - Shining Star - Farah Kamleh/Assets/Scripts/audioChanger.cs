using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioChanger : MonoBehaviour
{
    // refer to the audio sources to toggle between
    public AudioSource LinkinPark;
    public AudioSource Skillet;

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
            Skillet.Stop();

            // turn the other toggle off
            stars.isOn = false;

            // play the correct song
            LinkinPark.Play();
        }

        // if Stars toggle is on, play audio
        if (stars.isOn == true && selected == 2)
        {
            // stop the audio source
            LinkinPark.Stop();

            // turn the other toggle off
            light.isOn = false;

            // play the correct song
            Skillet.Play();
        }

        // if all toggles are off, 
        else if ((light.isOn == false) && (stars.isOn == false))
        {
            Skillet.Stop();
            LinkinPark.Stop();
        }
    }

    // function to control volume
    public void volumeChanger()
    {
        // make the audio source's volume that of the slider divided by ten
        LinkinPark.volume = sliderVol.value / 10.0f;
        Skillet.volume = sliderVol.value / 10.0f;
    }
}