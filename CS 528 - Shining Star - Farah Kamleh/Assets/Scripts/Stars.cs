using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Stars : MonoBehaviour
{
    // reference the star sprite prefab
    public GameObject starSprite;

    // refer to the renderer
    SpriteRenderer renderer;

    // refer to the player
    public GameObject player;

    // track previous position of the player
    public Vector3 lastPosition;

    // create two dictionaries to store star data
    public Dictionary<float, GameObject> theStars = new Dictionary<float, GameObject>();
    public Dictionary<float, starClass> starClass = new Dictionary<float, starClass>();

    // Start is called before the first frame update
    void Start()
    {
        // at start, last position is start position
        lastPosition = player.transform.position;

        // call the function that reads through the CSV file
        ReadCSVFile();

        // render some stars at the start
        StartCoroutine(RenderAtDistance());
    }

    // function that reads through the stars data
    void ReadCSVFile()
    {
        // load the CSV file as a text asset
        TextAsset csvText = Resources.Load<TextAsset>("athyg_31_reduced_m10_new");

        // create a list to store each line as an element
         List<string> linesList = new List<string>();

        // string variable to split and store each value as an element
        string[] values;

        // split the entire text by new lines to separate by line
        linesList = csvText.text.Split('\n').ToList();

        // loop through each element of the line list and split by comma
        for (int i = 0; i < linesList.Count; i++)
        {
            // for each line, make each value an element by splitting
            values = linesList[i].Split(',');

            // skip the first row
            if (values[0] != "")
            {
                // if the distance is less than or equal to 100 parsecs (~326 lightyears)
                //if (float.Parse(values[2]) <= 100)
                //{
                    // create the star class
                    starClass star = new starClass();

                    // store the values
                    star.hip = float.Parse(values[1]);
                    star.distance = float.Parse(values[2]);
                    star.xPos = float.Parse(values[3]);
                    star.yPos = float.Parse(values[4]);
                    star.zPos = float.Parse(values[5]);
                    star.mag = float.Parse(values[6]);
                    star.xVel = float.Parse(values[8]);
                    star.yVel = float.Parse(values[9]);
                    star.zVel = float.Parse(values[10]);
                    star.spect = values[11];

                    // add the star to the disctionary containing class information
                    starClass.Add(star.hip, star);

                    // instantiate star sprite prefab and use x, y, z for position and add to dictionary with hip #
                    theStars.Add(star.hip, Instantiate(starSprite, new Vector3(star.xPos, star.yPos, star.zPos), Quaternion.identity));

                    // refer to the renderer component
                    renderer = starSprite.GetComponent<SpriteRenderer>();

                    // make the color and size match spect (1/4th of estimated diameter)
                    if (star.spect == "O\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(6.6f / 4.0f, 6.6f / 4.0f, 6.6f / 4.0f);

                        // color based on chromaticity
                        renderer.color = new Color(146f / 255.0f, 181f / 255.0f, 255f / 255.0f);
                    }
                    if (star.spect == "B\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(1.8f / 4.0f, 1.8f / 4.0f, 1.8f / 4.0f);

                        // color based on chromaticity
                        renderer.color = new Color(162f / 255.0f, 192f / 255.0f, 255f / 255.0f);
                    }
                    if (star.spect == "A\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(1.4f / 4.0f, 1.4f / 4.0f, 1.4f / 4.0f);

                        // color based on chromaticity
                        renderer.color = new Color(213f / 255.0f, 224f / 255.0f, 255f / 255.0f);
                    }
                    if (star.spect == "F\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(1.15f / 4.0f, 1.15f / 4.0f, 1.15f / 4.0f);

                        // color based on chromaticity
                        renderer.color = new Color(249f / 255.0f, 245f / 255.0f, 255f / 255.0f);
                    }
                    if (star.spect == "G\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(0.96f / 4.0f, 0.96f / 4.0f, 0.96f / 4.0f);

                        // color based on chromaticity
                        renderer.color = new Color(255f / 255.0f, 237f / 255.0f, 227f / 255.0f);
                    }
                    if (star.spect == "K\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(0.7f / 4.0f, 0.7f / 4.0f, 0.7f / 4.0f);

                        // color based on chromaticity
                        renderer.color = new Color(255f / 255.0f, 218f / 255.0f, 181f / 255.0f);
                    }
                    if (star.spect == "M\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(0.2f / 4.0f, 0.2f / 4.0f, 0.2f / 4.0f);

                        // color based on chromaticity
                        renderer.color = new Color(255f / 255.0f, 181f / 255.0f, 108f / 255.0f);
                    }
                //}
            }
        }
    }

    // call ineumerator function for checking distance at every frame
    private void Update()
    {
        Debug.Log(Vector3.Distance(lastPosition, player.transform.position));

        // only call if player has traveled a particular distance from original position
        if (Vector3.Distance(lastPosition, player.transform.position) > 10)
        {
            // update new position
            lastPosition = player.transform.position;

            // call an ienumerator function to check distance from player
            StartCoroutine(RenderAtDistance());
        }
    }

    // check the distance of each star from the player, deactivate if too far away
    private IEnumerator RenderAtDistance()
    {
        // loop through the dictionary of stars
        foreach (KeyValuePair<float, GameObject> singleStar in theStars)
        {
            // if the star is too far from the player
            if (Vector3.Distance(player.transform.position, singleStar.Value.transform.position) > 50)
            {
                // disable the star
                singleStar.Value.SetActive(false);
            }
            // otherwise
            else
            {
                // enable the star
                singleStar.Value.SetActive(true);
            }
        }
        yield return new WaitForSeconds(1);
    }
}
