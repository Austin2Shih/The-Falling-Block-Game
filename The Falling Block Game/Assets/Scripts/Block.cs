using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    Rigidbody rb;
    int blockSize;
    BlockSpawner blockSpawner;
    void Start()
    {
        blockSpawner = BlockSpawner.Instance;
        rb = this.GetComponent<Rigidbody>();
        blockSize = (int)this.transform.localScale.x;
    }

    private void FixedUpdate()
    {
        preventBounce();
    }

    private void OnCollisionEnter(Collision collision)
    {
        bool hitBlock = collision.gameObject.tag == "Building Block";
        float blockBottomY = this.transform.position.y - 1;
        float collisionY = collision.GetContact(0).point.y;
        bool landed = Mathf.Abs(collisionY - blockBottomY) < 0.2;

        if (hitBlock && landed)
        {
            int x = (int)this.transform.position.x / blockSize;
            int z = (int)this.transform.position.z / blockSize;
            blockSpawner.grid[x, z] ++;
            blockSpawner.printGrid();
        }
    }

    private void preventBounce()
    {
        if (rb.velocity.y > 0)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
