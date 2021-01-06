using System.Collections;
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
    public float offset = 10f;
    // Start is called before the first frame update
    void Start()
    {
        // list to hold markers or dates to be used on track
        List<string> markerLabels = new List<string>();
        for (int i = 1; i < 101; i++)
        {
            markerLabels.Add(i.ToString());
        }

        
        // length of track
        float gameLength = end.transform.position.z - start.transform.position.z;

        // calculate offset based on distance between start and end points
        offset = gameLength / (markerLabels.Count + 1);

        // loop through labels in list and create label on track
        for (int i = 0; i < markerLabels.Count; i++)
        {
            Vector3 newPos = start.transform.position + new Vector3(0f, 0f, offset * (i + 1));
            Transform clone;
            clone = (Transform)Instantiate(marker, newPos, Quaternion.identity, parent);
            clone.GetComponentInChildren<TextMeshPro>().text = markerLabels[i];

        }

        //newObj.GetComponent<TextMeshPro>().text  = "-----";
        //newObj.transform.position = newObj.transform.position + new Vector3(0f, 0f, 20f * i);


    }

}
