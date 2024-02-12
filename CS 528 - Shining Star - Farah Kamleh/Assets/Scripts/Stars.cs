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

    // Start is called before the first frame update
    void Start()
    {
        // call the function that reads through the CSV file
        ReadCSVFile();

        // refer to the renderer component
        renderer = GetComponent<SpriteRenderer>();
    }

    // function that reads through the stars data
    void ReadCSVFile()
    {
        // load the CSV file as a text asset
        TextAsset csvText = Resources.Load<TextAsset>("athyg_31_reduced_m10_transformed");

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
                // if the distance is less than or equal to 8 parsecs (~25 lightyears)
                if (float.Parse(values[2]) <= 8)
                {
                    // instantiate star sprite prefab and use x, y, z for position 
                    Instantiate(starSprite, new Vector3(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5])), Quaternion.identity);

                    // make the color and size match spect
                    if (values[11] == "O")
                    {
                        // change size based on stellar classification estimates (* 2 to diameter)
                        starSprite.transform.localScale = new Vector3(6.6f * 2f, 6.6f * 2f, 6.6f * 2f);

                        // color based on chromaticity
                        renderer.color = new Color(146, 181, 255);
                    }
                    if (values[11] == "B")
                    {
                        // change size based on stellar classification estimates (* 2 to diameter)
                        starSprite.transform.localScale = new Vector3(6.6f * 2f, 6.6f * 2f, 6.6f * 2f);

                        // color based on chromaticity
                        renderer.color = new Color(162, 192, 255);
                    }
                    if (values[11] == "A")
                    {
                        // change size based on stellar classification estimates (* 2 to diameter)
                        starSprite.transform.localScale = new Vector3(6.6f * 2f, 6.6f * 2f, 6.6f * 2f);

                        // color based on chromaticity
                        renderer.color = new Color(213, 224, 255);
                    }
                    if (values[11] == "F")
                    {
                        // change size based on stellar classification estimates (* 2 to diameter)
                        starSprite.transform.localScale = new Vector3(6.6f * 2f, 6.6f * 2f, 6.6f * 2f);

                        // color based on chromaticity
                        renderer.color = new Color(249, 245, 255);
                    }
                    if (values[11] == "G")
                    {
                        // change size based on stellar classification estimates (* 2 to diameter)
                        starSprite.transform.localScale = new Vector3(6.6f * 2f, 6.6f * 2f, 6.6f * 2f);

                        // color based on chromaticity
                        renderer.color = new Color(255, 237, 227);
                    }
                    if (values[11] == "K")
                    {
                        // change size based on stellar classification estimates (* 2 to diameter)
                        starSprite.transform.localScale = new Vector3(6.6f * 2f, 6.6f * 2f, 6.6f * 2f);

                        // color based on chromaticity
                        renderer.color = new Color(255, 218, 181);
                    }
                    if (values[11] == "M")
                    {
                        // change size based on stellar classification estimates (* 2 to diameter)
                        starSprite.transform.localScale = new Vector3(6.6f * 2f, 6.6f * 2f, 6.6f * 2f);

                        // color based on chromaticity
                        renderer.color = new Color(255, 181, 108);
                    }
                }
            }
        }
    }
}
