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

    // scale mapping slider
    public Slider scaleSlider;

    // scale text
    public GameObject scaleText;

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

    // game objects for color scheme texts
    public GameObject stellarText;
    public GameObject exoText;

    // toggles for colors
    public Toggle Stellar;
    public Toggle Exo;

    // for returning to original positions
    public Vector3 originalPos;
    public Quaternion originalRot;

    // create two dictionaries to store star data
    public Dictionary<float, GameObject> theStars = new Dictionary<float, GameObject>();
    public Dictionary<float, starClass> theStarClass = new Dictionary<float, starClass>();

    // dictionary for exoplanet data wit hip keys and num planets values
    public Dictionary<float, int> exoStars = new Dictionary<float, int>();

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

    // game objects with texts to be altered
    public GameObject distance;
    public GameObject timeElapsed;

    // variable to track time elapsed
    int timePassing = 0;

    // Start is called before the first frame update
    void Start()
    {
        // variables to know the original position and rotation of the CAVE2
        originalPos = player.transform.position;
        originalRot = player.transform.rotation;

        // at start, last position is start position
        lastPosition = player.transform.position;

        // function to create exoplanet dictionary
        exoplanetData();

        // make sure stellar is on
        Stellar.isOn = true;

        // generate the stars from the csv
        ReadCSVFile();

        // render some stars at the start
        StartCoroutine(RenderAtDistance());

        // instantiate list
        constLine = new List<string>();

        // ensure stop time is toggled on
        stopT.isOn = true;

        // make sure modern is on
        Modern.isOn = true;

        // show stellar text at start
        stellarText.SetActive(true);
        exoText.SetActive(false);
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
        // if reverse time is on
        else if (reverseT.isOn == true)
        {
            // move the stars backwards
            StartCoroutine(timeDelay(2));
        }

        // display the correct distance traveled

        if (scaleSlider.value == 1)
        {
            distance.GetComponent<Text>().text = "Distance Traveled: " + (Mathf.Round(Vector3.Distance(originalPos, player.transform.position)) * 0.25f) + " Parsecs";
        }
        else if (scaleSlider.value == 2)
        {
            distance.GetComponent<Text>().text = "Distance Traveled: " + (Mathf.Round(Vector3.Distance(originalPos, player.transform.position)) * 0.5f) + " Parsecs";
        }
        else if (scaleSlider.value == 3)
        {
            distance.GetComponent<Text>().text = "Distance Traveled: " + Mathf.Round(Vector3.Distance(originalPos, player.transform.position)) + " Parsecs";
        }
        else if (scaleSlider.value == 4)
        {
            distance.GetComponent<Text>().text = "Distance Traveled: " + (Mathf.Round(Vector3.Distance(originalPos, player.transform.position)) * 2) + " Parsecs";
        }
        else if (scaleSlider.value == 5)
        {
            distance.GetComponent<Text>().text = "Distance Traveled: " + (Mathf.Round(Vector3.Distance(originalPos, player.transform.position)) * 3) + " Parsecs";
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------

    // a function to fill up the exoplanet dictionary
    void exoplanetData()
    {
        // load csv as text asset
        TextAsset exoText = Resources.Load<TextAsset>("exoplanet_filtered");

        // create a list to store each line as an element
        List<string> exoList = new List<string>();

        // split the entire text by new lines to separate by line
        exoList = exoText.text.Split('\n').ToList();

        // loop through the list and store the values within the dictionary
        for (int i = 0; i < exoList.Count; i++)
        {
            // split the line
            string[] hipAndNum = exoList[i].Split(',');

            // split the second value so only the hip number is used
            string[] theHIP = hipAndNum[1].Split(' ');

            // FIXME: currently the # with a "
            // Debug.Log(hipAndNum[2].Remove(hipAndNum[2].Length - 1));
            // Debug.Log(theHIP[1]);

            // store the key, value pair per star
            exoStars.Add(float.Parse(theHIP[1]), int.Parse(hipAndNum[2].Remove(1)));
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

                /* spect colors */

                // make the color and size match spect (1/4th of estimated diameter)
                if (star.spect == "O\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(6.6f / 4.0f, 6.6f / 4.0f, 6.6f / 4.0f);

                    // store color value
                    star.color = new Color(1f / 255.0f, 45f / 255.0f, 145f / 255.0f);
                }
                else if (star.spect == "B\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(1.8f / 4.0f, 1.8f / 4.0f, 1.8f / 4.0f);

                    // store color value
                    star.color = new Color(55f / 255.0f, 95f / 255.0f, 185f / 255.0f);
                }
                else if (star.spect == "A\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(1.4f / 4.0f, 1.4f / 4.0f, 1.4f / 4.0f);

                    // store color value
                    star.color = new Color(85f / 255.0f, 125f / 255.0f, 215f / 255.0f);
                }
                else if (star.spect == "F\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(1.15f / 4.0f, 1.15f / 4.0f, 1.15f / 4.0f);

                    // store color value
                    star.color = new Color(200f / 255.0f, 215f / 255.0f, 255f / 255.0f);
                }
                else if (star.spect == "G\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(0.96f / 4.0f, 0.96f / 4.0f, 0.96f / 4.0f);

                    // store color value
                    star.color = new Color(255f / 255.0f, 250f / 255.0f, 200f / 255.0f);
                }
                else if (star.spect == "K\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(0.7f / 4.0f, 0.7f / 4.0f, 0.7f / 4.0f);

                    // store color value
                    star.color = new Color(215f / 255.0f, 170f / 255.0f, 85f / 255.0f);
                }
                else if (star.spect == "M\r")
                {
                    // change size based on stellar classification estimates
                    starSprite.transform.localScale = new Vector3(0.2f / 4.0f, 0.2f / 4.0f, 0.2f / 4.0f);

                    // store color value
                    star.color = new Color(185f / 255.0f, 120f / 255.0f, 0f / 255.0f);
                }

                /* store exoplanet colors */

                // check if key is in dictionary first
                if (exoStars.ContainsKey(star.hip))
                {
                    // color based on number of planets
                    if (exoStars[star.hip] == 1)
                    {
                        // store the exo color (dark coral)
                        star.exoColor = new Color(208f / 255.0f, 79f / 255.0f, 69f / 255.0f);
                    }
                    else if (exoStars[star.hip] == 2)
                    {
                        // store the exo color (salmon)
                        star.exoColor = new Color(247f / 255.0f, 114f / 255.0f, 105f / 255.0f);
                    }
                    else if (exoStars[star.hip] == 3)
                    {
                        // store the exo color (golden glow)
                        star.exoColor = new Color(247f / 255.0f, 206f / 255.0f, 114f / 255.0f);
                    }
                    else if (exoStars[star.hip] == 4)
                    {
                        // store the exo color (lemon chiffon)
                        star.exoColor = new Color(255f / 255.0f, 245f / 255.0f, 208f / 255.0f);
                    }
                    else if (exoStars[star.hip] == 5)
                    {
                        // store the exo color (astral)
                        star.exoColor = new Color(51f / 255.0f, 110f / 255.0f, 31f / 255.0f);
                    }
                    else if (exoStars[star.hip] == 6)
                    {
                        // store the exo color (gulf blue)
                        star.exoColor = new Color(58f / 255.0f, 65f / 255.0f, 98f / 255.0f);
                    }
                }
                // if not in dictionary, make gray
                else
                {
                    star.exoColor = new Color(50f / 255.0f, 50f / 255.0f, 50f / 255.0f);
                }

                // color the stars
                renderer.color = star.color;

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
                // Debug.Log(values[j].Trim());
                // Debug.Log(values[j + 1].Trim());

                // create the line using two stars as the set positions
                lineRenderer.SetPosition(0, theStars[float.Parse(values[j].Trim())].transform.position);
                lineRenderer.SetPosition(1, theStars[float.Parse(values[j + 1].Trim())].transform.position);

                // set the colors of the line depending on color scheme
                if (Stellar.isOn == true)
                {
                    // if part of Thuraya, recolor
                    if (values[0] == "2000")
                    {
                        // FIXME: add glowy material
                        lineRenderer.SetColors(new Color(185f / 255.0f, 55f / 255.0f, 55f / 255.0f), new Color(185f / 255.0f, 55f / 255.0f, 55f / 255.0f));
                    }
                    else
                    {
                        // stellar colors
                        lineRenderer.SetColors(theStarClass[float.Parse(values[j].Trim())].color, theStarClass[float.Parse(values[j + 1].Trim())].color);
                    }
                    
                }
                else if (Exo.isOn == true)
                {
                    // exoplanet colors
                    lineRenderer.SetColors(theStarClass[float.Parse(values[j].Trim())].exoColor, theStarClass[float.Parse(values[j + 1].Trim())].exoColor);
                }

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

            // update the time elapsed
            timeElapsed.GetComponent<Text>().text = "Time Elapsed: " + (timePassing += 1000) + " Years";
        }
        // if brought by reverse time
        else if (button == 2)
        {
            // loop through the stars
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // transform the position based on velocity x 0.00102269 which is parsecs traveled per 1000 years 
                singleStar.Value.transform.position = new Vector3(singleStar.Value.transform.position.x - (theStarClass[singleStar.Key].xVel * 0.00102269f), singleStar.Value.transform.position.y - (theStarClass[singleStar.Key].yVel * 0.00102269f), singleStar.Value.transform.position.z - (theStarClass[singleStar.Key].zVel * 0.00102269f));
            }

            // update the time elapsed
            timeElapsed.GetComponent<Text>().text = "Time Elapsed: " + (timePassing += (-1000)) + " Years";
        }

        // delay
        yield return new WaitForSeconds(0);
    }

    // ----------------------------------------------------------------------------------------------------------------------------------------------------------

    // take care of color toggles
    public void colorScheme(int button)
    {
        // if stellar
        if (Stellar.isOn == true && button == 1)
        {
            // turn off exo
            Exo.isOn = false;
            exoText.SetActive(false);
            stellarText.SetActive(true);

            // color
            StartCoroutine(colorStars(button));
        }
        // if exo
        if (Exo.isOn == true && button == 2)
        {
            // turn off stellar
            Stellar.isOn = false;
            stellarText.SetActive(false);
            exoText.SetActive(true);

            // color
            StartCoroutine(colorStars(button));
        }
    }

    //------------------------------------------------------------------------------------------------------------------------------------------------------------

    // color based on scheme
    private IEnumerator colorStars(int button)
    {
        // if stellar
        if (button == 1)
        {
            // loop through stars and recolor
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // stellar recolor
                singleStar.Value.GetComponent<SpriteRenderer>().color = theStarClass[singleStar.Key].color;
            }
        }

        // if exo
        else if (button == 2)
        {
            // loop through stars and recolor
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // exoplanet recolor
                singleStar.Value.GetComponent<SpriteRenderer>().color = theStarClass[singleStar.Key].exoColor;
            }
        }

        /* recolor constellations */

        if (Modern.isOn == true)
        {
            Modern.isOn = false;
            Modern.isOn = true;
        }
        if (Sufi.isOn == true)
        {
            Sufi.isOn = false;
            Sufi.isOn = true;
        }
        if (Peninsula.isOn == true)
        {
            Peninsula.isOn = false;
            Peninsula.isOn = true;
        }
        if (Indigenous.isOn == true)
        {
            Indigenous.isOn = false;
            Indigenous.isOn = true;
        }
        if (LunarMansions.isOn == true)
        {
            LunarMansions.isOn = false;
            LunarMansions.isOn = true;
        }
        if (Egyptian.isOn == true)
        {
            Egyptian.isOn = false;
            Egyptian.isOn = true;
        }

        // return/delay
        yield return new WaitForSeconds(0);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    // resetting
    public void resetPos()
    {
        // reset the player position
        player.transform.position = originalPos;
    }

    public void resetRot()
    {
        // reset the player rotation
        player.transform.rotation = originalRot;
    }

    public void resetStars()
    {
        // loop through the list and transform position
        foreach (KeyValuePair<float, GameObject> singleStar in theStars)
        {
            // reset the position
            singleStar.Value.transform.position = new Vector3(theStarClass[singleStar.Key].xPos, theStarClass[singleStar.Key].yPos, theStarClass[singleStar.Key].zPos);
        }
    }

    public void resetAll()
    {
        // call the other functions
        resetPos();
        resetRot();
        resetStars();
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    // take to thuraya and display info
    public void thuraya()
    {
        // turn on Indigenous constellations
        if (Indigenous.isOn == false)
        {
            Indigenous.isOn = true;
        }

        // set position and rotation of player
        player.transform.position = new Vector3(6.11f, 4.25f, 1f);
        player.transform.rotation = Quaternion.Euler(-41.59f, 58.462f, 0f);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------------------------

    // scale the stars out
    public void scaleMapping()
    {
        // 0.25x
        if (scaleSlider.value == 1)
        {
            // loop through stars and place 0.25x
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // 0.25x
                singleStar.Value.transform.position = new Vector3(theStarClass[singleStar.Key].xPos * 0.25f, theStarClass[singleStar.Key].yPos * 0.25f, theStarClass[singleStar.Key].zPos * 0.25f);
            }

            // update text
            scaleText.GetComponent<Text>().text = "Scale Mapping 0.25x";
        }
        // 0.5x
        else if (scaleSlider.value == 2)
        {
            // loop through stars and place in 0.5x positions
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // set 0.5x position
                singleStar.Value.transform.position = new Vector3(theStarClass[singleStar.Key].xPos * 0.5f, theStarClass[singleStar.Key].yPos * 0.5f, theStarClass[singleStar.Key].zPos * 0.5f);
            }

            // update text
            scaleText.GetComponent<Text>().text = "Scale Mapping 0.50x";
        }
        // 1x
        else if (scaleSlider.value == 3)
        {
            // loop through stars and place in 1x position
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // set 1x position
                singleStar.Value.transform.position = new Vector3(theStarClass[singleStar.Key].xPos, theStarClass[singleStar.Key].yPos, theStarClass[singleStar.Key].zPos);
            }

            // update text
            scaleText.GetComponent<Text>().text = "Scale Mapping 1.00x";
        }
        // 2x
        else if (scaleSlider.value == 4)
        {
            // loop through stars and place in 2x position
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // set 2x position
                singleStar.Value.transform.position = new Vector3(theStarClass[singleStar.Key].xPos * 2, theStarClass[singleStar.Key].yPos * 2, theStarClass[singleStar.Key].zPos * 2);
            }

            // update text
            scaleText.GetComponent<Text>().text = "Scale Mapping 2.00x";
        }
        // 3x
        else if (scaleSlider.value == 5)
        {
            // loop through stars and place in 3x position
            foreach (KeyValuePair<float, GameObject> singleStar in theStars)
            {
                // set 3x position
                singleStar.Value.transform.position = new Vector3(theStarClass[singleStar.Key].xPos * 3, theStarClass[singleStar.Key].yPos * 3, theStarClass[singleStar.Key].zPos * 3);
            }

            // update text
            scaleText.GetComponent<Text>().text = "Scale Mapping 3.00x";
        }

        /* redraw present constellations */

        if (Modern.isOn == true)
        {
            Modern.isOn = false;
            Modern.isOn = true;
        }
        if (Sufi.isOn == true)
        {
            Sufi.isOn = false;
            Sufi.isOn = true;
        }
        if (Peninsula.isOn == true)
        {
            Peninsula.isOn = false;
            Peninsula.isOn = true;
        }
        if (Indigenous.isOn == true)
        {
            Indigenous.isOn = false;
            Indigenous.isOn = true;
        }
        if (LunarMansions.isOn == true)
        {
            LunarMansions.isOn = false;
            LunarMansions.isOn = true;
        }
        if (Egyptian.isOn == true)
        {
            Egyptian.isOn = false;
            Egyptian.isOn = true;
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
}
