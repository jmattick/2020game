using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script will add markers on the game track at equal intervals
public class MarkDays : MonoBehaviour
{
    // track marker prefab
    public Transform marker;
    // parent object of markers
    public Transform parent;
    // starting point object
    public Transform start;
    // ending point object
    public Transform end;
    // enemy prefab
    public Transform enemy;
    // maximum number of objects per data point
    public float maxObjects = 10f;
    // string to hold api json data
    public string jsonData = "";
    // z offset betwen data points
    private float offset;
    // object to hold daily covid cases
    private HandleAPIs.DataList data;
    // list of datapoint labels
    private List<string> markerLabels;
    // max number of cases to calculate fraction of cases per day
    private float max_cases = 0f;
    // total number of days in dataset
    private int num_days;

    // Start is called before the first frame update
    void Start()
    {
        // get COVID data from API 
        data = GetComponent<HandleAPIs>().GetAPIData(jsonData);
        // sort data by submission date
        data.SortByDate();
        // count number of days in dataset
        num_days = data.items.Count;

        // initialize list to hold markers or dates to be used on track
        markerLabels = new List<string>();
 
        // loop through each item in dataset to format marker labels and find max value
        for (int i = 1; i < num_days; i++)
        {
            // set max_cases if new_cases is larger than currrent max
            if (data.items[i].new_case > max_cases)
            {
                max_cases = data.items[i].new_case;
            }
            // add formatted label to markerLabels
            markerLabels.Add(data.items[i].submission_date.Substring(0,10));
        }

        // loop through each item in to set new_cases equal to the fraction of new cases over max cases
        //for (int i = 1; i < num_days; i++)
        //{
        //    data.items[i].new_case = data.items[i].new_case / max_cases;
        //}

        // length of track
        float gameLength = end.transform.position.z - start.transform.position.z;

        // calculate offset based on distance between start and end points
        offset = gameLength / (markerLabels.Count + 1);

        // loop through labels in list and create label on track
        for (int i = 0; i < markerLabels.Count; i++)
        {
            // newPos holds position of next marker label
            Vector3 newPos = start.transform.position + new Vector3(0f, 0f, offset * (i + 1));

            // enemyPosition holds position of next enemy 
            Vector3 enemyPos = newPos + new Vector3(0f, 10f, 4f);

            // multiple the fraction of new cases over max cases and multipy by max objects allowed 
            float maxNumItems = data.items[i].new_case / max_cases * maxObjects;
            
            // initalize marker clone
            Transform clone;

            // instatiate marker at next position
            clone = (Transform)Instantiate(marker, newPos, Quaternion.identity, parent);

            // update marker text
            clone.GetComponentInChildren<TextMeshPro>().text = markerLabels[i];

            // intantiate enemys at next enemy position
            for (int j = 0; j < maxNumItems; j++)
            {
                enemyPos += new Vector3(0f, 1f, 0f);
                Instantiate(enemy, enemyPos, Quaternion.identity);
            }
        }
    }

    // method to set jsondata to be called from Javascript
    public void SetJsonData(string jsonString)
    {
        jsonData = jsonString;
    }

}
