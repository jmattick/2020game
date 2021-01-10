using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOver : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        // if player z position is greater than end point position
        if (player.transform.position.z > transform.position.z)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
