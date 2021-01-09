using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float forwardForce = 500f;
    public float jumpForce = 500f;
    bool isGrounded = true;
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
        // get input information
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // calculate normalized direction of movement
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // move
        if (direction.magnitude >= 0.1f)
        {
            rb.AddForce(direction * forwardForce * Time.deltaTime);
        }      

        // jump on space if player is grounded
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce);
            isGrounded = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // check if player is grounded
        if(collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
