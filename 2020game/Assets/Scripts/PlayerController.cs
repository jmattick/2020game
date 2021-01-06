using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardForce = 10000f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //get rigidbody
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        //add forces
        if (direction.magnitude >= 0.1f)
        {
            rb.AddForce(direction * forwardForce * Time.deltaTime);
        }        
    }
}
