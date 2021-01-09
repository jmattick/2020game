using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    // store instance of game manager
    private static GameManager _instance;

    // string to hold covid json string from api
    private string covidData;

    // initialize Javascript method if run in WebGL build
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RequestUnityJS(string query);
#endif

    // game manager getter
    public static GameManager Instance
    {
        get
        {
            // create instance of game manager if does not exist
            if(_instance == null)
            {
                GameObject instance = new GameObject("Game Manager");
                instance.AddComponent<GameManager>();
            }

            return _instance;
        }
    }
    
    // method called on awake
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        } else
        {
            _instance = this;
            covidData = "[]"; // set inital value for data
            RequestJS("CA"); // initially request data from Illinois
            DontDestroyOnLoad(this);
        }            
    }

    // method to set jsondata to be called from Javascript
    public void SetJsonData(string jsonString)
    {
        Debug.Log(jsonString);
        covidData = jsonString;
        Debug.Log("after setjsondata");
        Debug.Log(covidData);
    }

    public string GetJsonData()
    {
        return covidData;
    }

    // unity method to call Javascript method (query is US state string)
    public void RequestJS(string query)
    {
        // if run in WebGL build call JS plugin method
#if UNITY_WEBGL && !UNITY_EDITOR
        Debug.Log("requestJS");
        Debug.Log(covidData);
        RequestUnityJS(query);
#endif
    }
}
