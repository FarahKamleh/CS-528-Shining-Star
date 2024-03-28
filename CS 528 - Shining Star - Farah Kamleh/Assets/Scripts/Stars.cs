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

    // a list of gameobjects for the star sprites
    List<GameObject> stars = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // call the function that reads through the CSV file
        ReadCSVFile();
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
                if (float.Parse(values[2]) <= 100)
                {
                    // instantiate star sprite prefab and use x, y, z for position and add to array
                    stars.Add(Instantiate(starSprite, new Vector3(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5])), Quaternion.identity));

                    // refer to the renderer component
                    renderer = starSprite.GetComponent<SpriteRenderer>();

                    // make the color and size match spect (1/4th of estimated diameter)
                    if (values[11] == "O\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(6.6f / 2.0f, 6.6f / 2.0f, 6.6f / 2.0f);

                        // color based on chromaticity
                        renderer.color = new Color(146f / 255.0f, 181f / 255.0f, 255f / 255.0f);
                    }
                    if (values[11] == "B\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(1.8f / 2.0f, 1.8f / 2.0f, 1.8f / 2.0f);

                        // color based on chromaticity
                        renderer.color = new Color(162f / 255.0f, 192f / 255.0f, 255f / 255.0f);
                    }
                    if (values[11] == "A\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(1.4f / 2.0f, 1.4f / 2.0f, 1.4f / 2.0f);

                        // color based on chromaticity
                        renderer.color = new Color(213f / 255.0f, 224f / 255.0f, 255f / 255.0f);
                    }
                    if (values[11] == "F\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(1.15f / 2.0f, 1.15f / 2.0f, 1.15f / 2.0f);

                        // color based on chromaticity
                        renderer.color = new Color(249f / 255.0f, 245f / 255.0f, 255f / 255.0f);
                    }
                    if (values[11] == "G\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(0.96f / 2.0f, 0.96f / 2.0f, 0.96f / 2.0f);

                        // color based on chromaticity
                        renderer.color = new Color(255f / 255.0f, 237f / 255.0f, 227f / 255.0f);
                    }
                    if (values[11] == "K\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(0.7f / 2.0f, 0.7f / 2.0f, 0.7f / 2.0f);

                        // color based on chromaticity
                        renderer.color = new Color(255f / 255.0f, 218f / 255.0f, 181f / 255.0f);
                    }
                    if (values[11] == "M\r")
                    {
                        // change size based on stellar classification estimates
                        starSprite.transform.localScale = new Vector3(0.2f / 2.0f, 0.2f / 2.0f, 0.2f / 2.0f);

                        // color based on chromaticity
                        renderer.color = new Color(255f / 255.0f, 181f / 255.0f, 108f / 255.0f);
                    }
                }
            }
        }
    }

    // call ineumerator function for checking distance at every frame
    private void Update()
    {
        // call an ienumerator function to check distance from player
        StartCoroutine(RenderAtDistance());
    }

    // check the distance of each star from the player, deactivate if too far away
    private IEnumerator RenderAtDistance()
    {
        // loop through the list of stars
        for (int i = 0; i < stars.Count; i++)
        {
            // if the star is too far from the player
            if (Vector3.Distance(player.transform.position, stars[i].transform.position) > 50)
            {
                // disable the star
                stars[i].SetActive(false);
            }
            // otherwise
            else
            {
                // enable the star
                stars[i].SetActive(true);
            }
        }
        yield return new WaitForSeconds(1);
    }
}
