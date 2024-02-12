using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    // for the direction of the camera
    Vector3 cameraDir;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // the camera direction (face forward)
        cameraDir = Camera.main.transform.forward;

        // set the y direction to 0 to prevent facing vertical pos of player
        cameraDir.y = 0;

        // rotate sprite to face it
        transform.rotation = Quaternion.LookRotation(cameraDir);

    }
}
