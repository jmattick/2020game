using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    // state dropdown
    public TMP_Dropdown stateDropdown;

    // list of states in data
    readonly List<string> usStates = new List<string>() { "AL", "AR", "AS", "AZ", "CA", "CO", "CT", "DE", "FL", "GA", "IA", "ID", "IL", "IN", "KY", "LA", "MA", "MD", "MI", "MN", "MS", "ND", "NE", "NM", "NV", "NY", "NYC", "OH", "OK", "PA", "PR", "RMI", "SC", "TN", "TX", "VA", "WA", "WI", "WV", "WY" };


    public void Awake()
    {
        GameManager.Instance.RequestJS("IL");
    }

    public void Start()
    {
        
        // clear any input in dropdown
        stateDropdown.ClearOptions();

        // loop through states and add options
        foreach (string st in usStates)
        {
            stateDropdown.options.Add(new TMP_Dropdown.OptionData() { text = st });
        }
        stateDropdown.value = 12;

        
    }
    public void PlayGame()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            SceneManager.LoadScene(1);
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void UpdateState()
    {
        GameManager.Instance.SetUSState(usStates[stateDropdown.value]);
        GameManager.Instance.RequestJS(usStates[stateDropdown.value]);
    }
}
