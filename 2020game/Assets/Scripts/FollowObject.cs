using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform followObject;
    Vector3 offset;

    private void Start()
    {
        //Calculate offset between camera and object
        offset = Camera.main.transform.position - followObject.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Set camera position as object moves
        Camera.main.transform.position = followObject.position + offset;
        Camera.main.ScreenToWorldPoint(followObject.position);
    }
}
