# 2020game

[Play Game](https://jmattick.github.io/2020game/Builds/index.html)

### Abstract

### Platform

- Desktop / WebGL

### Development Tools / Languages

- Unity 2019.4.12f1
- C#
- JavaScript

### SDKS

### APIS

COVID-19 Data from CDC:

- [CDC COVID-19 Data Tracker](https://dev.socrata.com/foundry/data.cdc.gov/9mfq-cb36)

### Assets

Unity Assets: 

- [TextMeshPro](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html)

### Video

### Development Process

The goal of this project is to combine data visualization and game development. 
Game levels are generated from real-time daily COVID-19 data. Players can traverse a track and encounter game objects
that represent the fraction of new covid cases on each day in the dataset over the maximum new cases.
Data is sourced from the [CDC COVID-19 Data Tracker](https://dev.socrata.com/foundry/data.cdc.gov/9mfq-cb36) API. 

**Creating Game Levels from JSON data**

The first milestone was to convert the json string received from the api into a usable format for building levels. 
To do this, an API Handling C# script was created to get the data into the game. 
Unity's [JsonUtility.FromJson](https://docs.unity3d.com/ScriptReference/JsonUtility.FromJson.html) utility will 
return an object from its json representation. However, the data that comes from the api
is a json array, so a custom class representing the covid data as an object was created. Each daily datapoint can be 
represented as a DataPoint object that stores information about the date, the U.S. state, and the new cases on the date.

```csharp
// class to hold data about daily new cases
    [Serializable]
    public class DataPoint
    {
        public string submission_date;
        public string state;
        public float new_case;
    }
```

Next, the DataList object was created to store all of the data points in the dataset. For the purpose of the game, it is 
useful to sort the data points by submission date. The SortByDate() method strips any excess data from the 
submission_date string to sort the dataset in ascending order.

```csharp
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
```

In order to provide a working game in the case the api fails to provide data, a backup json datafile is included
in the project. The GetAPIData() function will try to convert the provided json string into an object and will 
default to the json file in the case of an ArgumentException.

```csharp
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
```

The DataList object created can then be used to generate game levels. 

**Accessing API data within Unity C# scripts**

While Unity has built-in C# [web request methods](https://docs.unity3d.com/Manual/webgl-networking.html), accessing
cross-domain resources in WebGL builds has restrictions. To solve this challenge, the data
was fetched from the api using Javascript. A diagram of the solution is shown below.

![unity api diagram](img/UnityAPI.png) 

To preserve data throughout the entire game, a Game Manager object was created using a Singleton design pattern. 
A single instance of this object is created during the initial scene of the game 
and is not destroyed at any point in the game. The game manager stores the json string covid data from the api.

```csharp
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
        _instance = this;
        covidData = "[]"; // set inital value for data
        RequestJS("IL"); // initially request data from Illinois
        DontDestroyOnLoad(this);
    }

    // method to set jsondata to be called from Javascript
    public void SetJsonData(string jsonString)
    {
        covidData = jsonString;
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
        RequestUnityJS(query);
#endif
    }
}

``` 

The game manager will call the RequestUnityJS() method in a javascript plugin within the unity project 
whenever new data is needed in the game. RequestUnityJS() will call the 
javascript function getCovidData() found in a script tag in index.html.

```javascript
var covidlib = {
	$dependencies:{},
	RequestUnityJS: function(query){
		getCovidData(UTF8ToString(query));
	}	
};
autoAddDeps(covidlib, '$dependencies');
mergeInto(LibraryManager.library,covidlib);
```

Finally, the getCovidData() method will fetch data from the CDC API and call the SetJsonData() C# method in the 
Game Manager with the json string as a parameter. 

```javascript
var unityInstance = UnityLoader.instantiate("unityContainer", "Build/Builds.json", {onProgress: UnityProgress});   

// method to fetch api 
var getCovidData = function(st) {
    var base_url = "https://data.cdc.gov/resource/9mfq-cb36.json?state=";
     
    fetch(base_url+st)
        .then(res => res.json())
        .then((out) => {
            // call SetJsonData in Game Manager
            unityInstance.SendMessage('Game Manager', 'SetJsonData', JSON.stringify(out));
    }).catch(err => console.error(err));
}
```






