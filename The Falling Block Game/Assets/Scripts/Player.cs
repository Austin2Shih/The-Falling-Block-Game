using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 getFacingDirection()
    {
        Vector3 output;
        Vector3 eulerAngles = this.transform.eulerAngles;
        float xzDegrees = Mathf.Deg2Rad * eulerAngles.y;
        float zyDegrees = Mathf.Deg2Rad * eulerAngles.x;

        float outputY = -Mathf.Sin(zyDegrees) * (Mathf.Cos(xzDegrees)/Mathf.Cos(zyDegrees));
        output = new Vector3(Mathf.Sin(xzDegrees), outputY, Mathf.Cos(xzDegrees));
        output.Normalize();

        return output;
    }
}
