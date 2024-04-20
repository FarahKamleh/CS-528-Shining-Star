using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

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

    // the toggles of the constellations
    public Toggle Modern;
    public Toggle Sufi;
    public Toggle Peninsula;
    public Toggle Indigenous;
    public Toggle LunarMansions;
    public Toggle Egyptian;
    public Toggle None;

    // toggles for time
    public Toggle startT;
    public Toggle reverseT;
    public Toggle stopT;

    // create two dictionaries to store star data
    public Dictionary<float, GameObject> theStars = new Dictionary<float, GameObject>();
    public Dictionary<float, starClass> theStarClass = new Dictionary<float, starClass>();

    // list of constellation line renderers
    public List<LineRenderer> modernList = new List<LineRenderer>();
    public List<LineRenderer> sufiList = new List<LineRenderer>();
    public List<LineRenderer> peninsulaList = new List<LineRenderer>();
    public List<LineRenderer> indigenousList = new List<LineRenderer>();
    public List<LineRenderer> lunarMansionsList = new List<LineRenderer>();
    public List<LineRenderer> egyptianList = new List<LineRenderer>();

    // an empty prefab game object for each line
    public GameObject lineObj;

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

        // ensure stop time is toggled on
        stopT.isOn = true;
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

        // if start time is on
        if (startT.isOn == true)
        {
            // move the stars forward
            StartCoroutine(timeDelay(1));
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

    // a function to determine which constellations to toggle on and off
    public void CreateConstellations(int constellationChoice)
    {
        /* choice in dataset */

        // if the choice is "none"
        if (constellationChoice == 0)
        {
            // toggle off all others
            Modern.isOn = false;
            Sufi.isOn = false;
            Peninsula.isOn = false;
            Indigenous.isOn = false;
            LunarMansions.isOn = false;
            Egyptian.isOn = false;
        }
        // if the choice is "modern"
        else if (constellationChoice == 1)
        {
            // if toggled on
            if (Modern.isOn == true)
            {
                // load the CSV file as a text asset
                csvConst = Resources.Load<TextAsset>("modern");

                // call the generation function
                generateConstellations(constellationChoice, csvConst, modernList);
            }
            // if toggled off
            else if (Modern.isOn == false)
            {
                // destroy the list
                for (int i = 0; i < modernList.Count; i++)
                {
                    // rid of a single line renderer 
                    Destroy(modernList[i]);
                }

                // exit
                return;
            }
        }
        // if the choice is "arabic al-sufi"
        else if (constellationChoice == 2)
        {
            // if toggled on
            if (Sufi.isOn == true)
            {
                // load the CSV file as a text asset
                csvConst = Resources.Load<TextAsset>("al-sufi");

                // call the generation function
                generateConstellations(constellationChoice, csvConst, sufiList);
            }
            // if toggled off
            else if (Sufi.isOn == false)
            {
                // destroy the list
                for (int i = 0; i < sufiList.Count; i++)
                {
                    // rid of a single line renderer 
                    Destroy(sufiList[i]);
                }

                // exit
                return;
            }
        }
        // if the choice is "arabian peninsula"
        else if (constellationChoice == 3)
        {
            // if toggled on
            if (Peninsula.isOn == true)
            {
                // load the CSV file as a text asset
                csvConst = Resources.Load<TextAsset>("arabian peninsula");

                // call the generation function
                generateConstellations(constellationChoice, csvConst, peninsulaList);
            }
            // if toggled off
            else if (Peninsula.isOn == false)
            {
                // destroy the list
                for (int i = 0; i < peninsulaList.Count; i++)
                {
                    // rid of a single line renderer 
                    Destroy(peninsulaList[i]);
                }

                // exit
                return;
            }
        }
        // if the choice is "arabic indigenous"
        else if (constellationChoice == 4)
        {
            // if toggled on
            if (Indigenous.isOn == true)
            {
                // load the CSV file as a text asset
                csvConst = Resources.Load<TextAsset>("arabic indigenous");

                // call the generation function
                generateConstellations(constellationChoice, csvConst, indigenousList);
            }
            // if toggled off
            else if (Indigenous.isOn == false)
            {
                // destroy the list
                for (int i = 0; i < indigenousList.Count; i++)
                {
                    // rid of a single line renderer 
                    Destroy(indigenousList[i]);
                }

                // exit
                return;
            }
        }
        // if the constellation choice is "arabic lunar mansions"
        else if (constellationChoice == 5)
        {
            // if toggled on
            if (LunarMansions.isOn == true)
            {
                // load the CSV file as a text asset
                csvConst = Resources.Load<TextAsset>("arabic lunar mansions");

                // call the generation function
                generateConstellations(constellationChoice, csvConst, lunarMansionsList);
            }
            // if toggled off
            else if (LunarMansions.isOn == false)
            {
                // destroy the list
                for (int i = 0; i < lunarMansionsList.Count; i++)
                {
                    // rid of a single line renderer 
                    Destroy(lunarMansionsList[i]);
                }

                // exit
                return;
            }
        }
        // if the constellation choice is "egyptian"
        else if (constellationChoice == 6)
        {
            // if toggled on
            if (Egyptian.isOn == true)
            {
                // load the CSV file as a text asset
                csvConst = Resources.Load<TextAsset>("egyptian");

                // call the generation function
                generateConstellations(constellationChoice, csvConst, egyptianList);
            }
            // if toggled off
            else if (Egyptian.isOn == false)
            {
                // destroy the list
                for (int i = 0; i < egyptianList.Count; i++)
                {
                    // rid of a single line renderer 
                    Destroy(egyptianList[i]);
                }

                // exit
                return;
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    // a separate function to actually load the constellations
    public void generateConstellations(int constellationChoice, TextAsset csvConst, List<LineRenderer> givenList)
    {
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

                // for determining which constellations to eliminate
                Debug.Log(values[j].Trim());
                Debug.Log(values[j + 1].Trim());

                // create the line using two stars as the set positions
                lineRenderer.SetPosition(0, theStars[float.Parse(values[j].Trim())].transform.position);
                lineRenderer.SetPosition(1, theStars[float.Parse(values[j + 1].Trim())].transform.position);

                // set the colors of the line
                lineRenderer.SetColors(theStarClass[float.Parse(values[j].Trim())].color, theStarClass[float.Parse(values[j + 1].Trim())].color);

                // store the line in the list
                givenList.Add(lineRenderer);
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    // this function allows the stars and constellations to move over time
    public void time(int button)
    {
        // if start time
        if (startT.isOn == true && button == 1)
        {
            // turn off others
            reverseT.isOn = false;
            stopT.isOn = false;
        }
        // if reverse time
        if (reverseT.isOn == true && button == 2)
        {
            // turn off others
            stopT.isOn = false;
            startT.isOn = false;
        }
        // if stop time
        else if (stopT.isOn == true && button == 3)
        {
            // turn off others
            reverseT.isOn = false;
            startT.isOn = false;
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    // function to add delays to time updates
    private IEnumerator timeDelay(int button)
    {
        // if brought by start time
        if (button == 1)
        {
            // loop through the stars
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // transform the position based on velocity x 0.00102269 which is parsecs traveled per 1000 years 
                singleStar.Value.transform.position = new Vector3(singleStar.Value.transform.position.x + (theStarClass[singleStar.Key].xVel * 0.00102269f), singleStar.Value.transform.position.y + (theStarClass[singleStar.Key].yVel * 0.00102269f), singleStar.Value.transform.position.z + (theStarClass[singleStar.Key].zVel * 0.00102269f));
            }
        }
        // if brought by reverse time
        else if (button == 2)
        {


        }

        // delay
        yield return new WaitForSeconds(0);
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
}
