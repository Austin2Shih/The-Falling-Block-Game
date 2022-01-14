using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        bool hitBlock = other.gameObject.tag == "Building Block";
        Vector3 topOfPlayer = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        bool hitTopOfPlayer = Vector3.Distance(other.ClosestPoint(topOfPlayer), topOfPlayer) < 0.2;

        if (hitBlock && hitTopOfPlayer)
        {
            //Debug.Log("You Died");
        }
    }
}
