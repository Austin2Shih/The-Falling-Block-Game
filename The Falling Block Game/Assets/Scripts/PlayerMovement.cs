using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = new Vector3(0, 0, 0);
        

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDir.x = -1;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDir.x = 1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDir.z = -1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDir.z = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveDir.y += 50;
        }

        Debug.Log(moveDir.ToString());

        rb.AddForce(moveDir * 10);
    }
}
