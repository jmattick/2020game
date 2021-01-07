﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This script will add markers on the game track at equal intervals
public class MarkDays : MonoBehaviour
{
    public Transform marker;
    public Transform parent;
    public Transform start;
    public Transform end;
    public Transform enemy;
    public float offset = 10f;
    public float maxObjects = 10f;
    public string usState = "IL";


    // Start is called before the first frame update
    void Start()
    {
        // get COVID data from API 
        HandleAPIs.DataList data = GetComponent<HandleAPIs>().GetAPIData(usState);
        // sort data by submission date
        data.SortByDate();

        Debug.Log(data.items[0].submission_date);
        // Intantiate Objects
        // list to hold markers or dates to be used on track
        List<string> markerLabels = new List<string>();

        // initiate variable to find max number cases
        float max_cases = 0f;

        // loop through each item in to format marker labels and find max value
        for (int i = 1; i < data.items.Count; i++)
        {
            if (data.items[i].new_case > max_cases)
            {
                max_cases = data.items[i].new_case;
            }
            markerLabels.Add(data.items[i].submission_date.Substring(0,10));
        }

        // loop through each item in to find the fraction of cases over max cases
        for (int i = 1; i < data.items.Count; i++)
        {
            data.items[i].new_case = data.items[i].new_case / max_cases;
        }

        // length of track
        float gameLength = end.transform.position.z - start.transform.position.z;

        // calculate offset based on distance between start and end points
        offset = gameLength / (markerLabels.Count + 1);

        // loop through labels in list and create label on track
        for (int i = 0; i < markerLabels.Count; i++)
        {
            Vector3 newPos = start.transform.position + new Vector3(0f, 0f, offset * (i + 1));
            Vector3 enemyPos = newPos + new Vector3(0f, 10f, 0f);
            Transform clone;
            clone = (Transform)Instantiate(marker, newPos, Quaternion.identity, parent);
            clone.GetComponentInChildren<TextMeshPro>().text = markerLabels[i];

            // Intantiate fraction of enemys at marker position
            for (int j = 0; j < data.items[i].new_case * maxObjects; j++)
            {
                enemyPos = enemyPos + new Vector3(0f, 1f, 0f);
                Instantiate(enemy, enemyPos, Quaternion.identity);
            }
            
;
        }

    }

}
