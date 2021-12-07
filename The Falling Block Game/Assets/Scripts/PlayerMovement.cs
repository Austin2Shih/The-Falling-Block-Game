using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rb;
    public int horizontalMoveScaler = 12;
    public int jumpPower = 10;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        checkMove();
    }

    void checkMove()
    {
        Vector3 moveDir = new Vector3(0, rb.velocity.y, 0);

        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x = -horizontalMoveScaler;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x = horizontalMoveScaler;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveDir.z = horizontalMoveScaler;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir.z = -horizontalMoveScaler;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveDir.y += jumpPower;
        }

        rb.velocity = moveDir;
    }
}
