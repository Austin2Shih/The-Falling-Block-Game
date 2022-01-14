using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float opaqueAlpha = 0.3f;
    public float solidAlpha = 1.0f;

    Rigidbody rb;
    int blockSize;

    private float timeStartFall;
    private int x, y, z;


    MapSpawner blockSpawner;
    void Start()
    {
        blockSpawner = MapSpawner.Instance;
        rb = this.GetComponent<Rigidbody>();
        blockSize = GameSettings.blockSize;
    }
    void ChangeAlpha(Material mat, float alphaVal)
    {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        mat.SetColor("_Color", newColor);
    }

    public void setOpaque()
    {
        ChangeAlpha(gameObject.GetComponent<Renderer>().material, opaqueAlpha);
    }

    public void setSolid()
    {
        ChangeAlpha(gameObject.GetComponent<Renderer>().material, solidAlpha);
    }

}
