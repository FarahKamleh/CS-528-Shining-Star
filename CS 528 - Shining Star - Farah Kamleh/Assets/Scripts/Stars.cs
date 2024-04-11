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
    public Dictionary<float, starClass> theStarClass = new Dictionary<float, starClass>();

    // set of lists to store each constellation set
    public List<LineRenderer> modern = new List<LineRenderer>();

    // an empty prefab game object for each line
    public GameObject lineObj;

    // int for constellation choice in menu; at start, modern
    public int constellationChoice = 1;

    // text asset to be used for loading constellations
    TextAsset csvConst;

    // a list to store lines
    List<string> constLine;

    // string variable to split and store each value of a line as an element
    string[] values;

    // Start is called before the first frame update
    void Start()
    {
        // at start, last position is start position
        lastPosition = player.transform.position;

        // call the function that reads through the CSV file
        ReadCSVFile();

        // render some stars at the start
        StartCoroutine(RenderAtDistance());

        // instantiate list
        constLine = new List<string>();

        // begin with the modern constellations
        CreateConstellations(1, modern);

        // load next set of constellations

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------------------

    // call ineumerator function for checking distance at every frame
    private void Update()
    {
        // only call if player has traveled a particular distance from original position
        if (Vector3.Distance(lastPosition, player.transform.position) > 10)
        {
            // update new position
            lastPosition = player.transform.position;

            // call an ienumerator function to check distance from player
            StartCoroutine(RenderAtDistance());
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------

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

                // refer to the renderer component
                renderer = starSprite.GetComponent<SpriteRenderer>();

                // make the color and size match spect (1/4th of estimated diameter)
                if (star.spect == "O\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(6.6f / 4.0f, 6.6f / 4.0f, 6.6f / 4.0f);

                    // color based on chromaticity
                    renderer.color = new Color(146f / 255.0f, 181f / 255.0f, 255f / 255.0f);

                    // store color value
                    star.color = renderer.color;
                }
                if (star.spect == "B\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(1.8f / 4.0f, 1.8f / 4.0f, 1.8f / 4.0f);

                    // color based on chromaticity
                    renderer.color = new Color(162f / 255.0f, 192f / 255.0f, 255f / 255.0f);

                    // store color value
                    star.color = renderer.color;
                }
                if (star.spect == "A\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(1.4f / 4.0f, 1.4f / 4.0f, 1.4f / 4.0f);

                    // color based on chromaticity
                    renderer.color = new Color(213f / 255.0f, 224f / 255.0f, 255f / 255.0f);

                    // store color value
                    star.color = renderer.color;
                }
                if (star.spect == "F\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(1.15f / 4.0f, 1.15f / 4.0f, 1.15f / 4.0f);

                    // color based on chromaticity
                    renderer.color = new Color(249f / 255.0f, 245f / 255.0f, 255f / 255.0f);

                    // store color value
                        star.color = renderer.color;
                }
                if (star.spect == "G\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(0.96f / 4.0f, 0.96f / 4.0f, 0.96f / 4.0f);

                    // color based on chromaticity
                    renderer.color = new Color(255f / 255.0f, 237f / 255.0f, 227f / 255.0f);

                    // store color value
                    star.color = renderer.color;
                }
                if (star.spect == "K\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(0.7f / 4.0f, 0.7f / 4.0f, 0.7f / 4.0f);

                    // color based on chromaticity
                    renderer.color = new Color(255f / 255.0f, 218f / 255.0f, 181f / 255.0f);

                    // store color value
                    star.color = renderer.color;
                }
                if (star.spect == "M\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(0.2f / 4.0f, 0.2f / 4.0f, 0.2f / 4.0f);

                    // color based on chromaticity
                    renderer.color = new Color(255f / 255.0f, 181f / 255.0f, 108f / 255.0f);

                    // store color value
                    star.color = renderer.color;
                }

                // add the star to the disctionary containing class information
                theStarClass.Add(star.hip, star);

                // instantiate star sprite prefab and use x, y, z for position and add to dictionary with hip #
                theStars.Add(star.hip, Instantiate(starSprite, new Vector3(star.xPos, star.yPos, star.zPos), Quaternion.identity));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------

    void CreateConstellations(int constellationChoice, List<LineRenderer> constellationSet)
    {
        /* choice in dataset */

        // if the choice is "modern"
        if (constellationChoice == 1)
        {
            // load the CSV file as a text asset
            csvConst = Resources.Load<TextAsset>("modern");
        }
        if (constellationChoice == 2)
        {

        }
        if (constellationChoice == 3)
        {

        }
        if (constellationChoice == 4)
        {

        }
        if (constellationChoice == 5)
        {

        }

        /* read through dataset */

        // split the entire text by newlines to separate by line
        constLine = csvConst.text.Split('\n').ToList();

        // loop through each constellation and split by space
        for (int i = 0; i < constLine.Count; i++)
        {
            // for each line, make each value an element by splitting
            values = constLine[i].Split(' ');

            // for the position of the line prefab
            GameObject constellation = new GameObject();

            // loop through each element of the line
            for (int j = 3; j < values.Count() - 1; j = j + 2)
            {
                // instantiate line prefab with constellation position
                GameObject linePrefab = Instantiate(lineObj, constellation.transform);

                // use a line renderer
                LineRenderer lineRenderer = linePrefab.GetComponent<LineRenderer>();

                // create the line using two stars as the set positions
                lineRenderer.SetPosition(0, theStars[float.Parse(values[j].Trim())].transform.position);
                lineRenderer.SetPosition(1, theStars[float.Parse(values[j + 1].Trim())].transform.position);

                // set the colors of the line
                lineRenderer.SetColors(theStarClass[float.Parse(values[j].Trim())].color, theStarClass[float.Parse(values[j + 1].Trim())].color);

                // store the line in the list
                constellationSet.Add(lineRenderer);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

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

        // return
        yield return new WaitForSeconds(1);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    // to switch between constellations

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------
}
