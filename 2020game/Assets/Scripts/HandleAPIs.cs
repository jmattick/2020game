using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandleAPIs : MonoBehaviour
{
    // backup json data file
    public TextAsset jsonFile;

    // class to hold data about daily new cases
    [Serializable]
    public class DataPoint
    {
        public string submission_date;
        public string state;
        public float new_case;
    }

    // class to hold list of DataPoint objects
    [Serializable]
    public class DataList
    {
        public List<DataPoint> items;

        // method to sort items by submission date
        public void SortByDate()
        {
            // sort items
            items.Sort((a, b) => {
                // extract numerical date
                string date_a = a.submission_date.Substring(0, 10).Replace("-","");
                string date_b = b.submission_date.Substring(0, 10).Replace("-", "");

                // compare dates
                if (date_a.CompareTo(date_b)>0)
                {
                    return 1;
                } else
                {
                    return -1;
                }
            });
        }
    }

    // method to get data from API
    public DataList GetAPIData(string jsonString)
    {
        // initalize fianl json string compatible with DataList Class
        string jsonResponse;
        DataList res;
        // if no external data
        if (jsonString == "[]")
        {
            res = LoadBackupData();
        }
        else
        {
            try
            {
                // use provided json string
                jsonResponse = "{\"items\":" + jsonString + "}";
                Debug.Log("try get api data");
                Debug.Log(jsonResponse);
                // convert json response to DataList object
                res = JsonUtility.FromJson<DataList>(jsonResponse);
            }
            catch (System.ArgumentException ex)
            {
                Debug.Log(ex);
                res = LoadBackupData();
            }
        }
                
        return res;
    }

    // method loads data from backup json file
    public DataList LoadBackupData()
    {
        // load backup data from json file
        string jsonResponse = "{\"items\":" + jsonFile.text + "}";
        // convert json response to DataList object
        DataList res = JsonUtility.FromJson<DataList>(jsonResponse);
        return res;
    }
}
