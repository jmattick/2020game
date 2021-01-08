using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Net;
using System;
//using System.IO;

public class HandleAPIs : MonoBehaviour
{
    // string containing api url
    public string url_base = "https://data.cdc.gov/resource/9mfq-cb36.json?state=";
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
            items.Sort((a, b) => {
                string date_a = a.submission_date.Substring(0, 10).Replace("-","");
                string date_b = b.submission_date.Substring(0, 10).Replace("-", "");

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

        // request url
        // HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url_base + state);
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //StreamReader reader = new StreamReader(response.GetResponseStream());

        // add field to store list and read response
        //string jsonResponse = "{\"items\":" + reader.ReadToEnd() + "}";
        string jsonResponse;
        if (jsonString == "")
        {
            jsonResponse = "{\"items\":" + jsonFile.text + "}";
        } else
        {
            jsonResponse = "{\"items\":" + jsonString + "}";
        }
        // convert json response to DataList object
        DataList result = JsonUtility.FromJson<DataList>(jsonResponse);
        return result;
    }
}
