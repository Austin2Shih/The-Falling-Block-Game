using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float opaqueAlpha;
    public float solidAlpha;

    Rigidbody rb;
    int blockSize;

    private int x, y, z;

    MapSpawner mapSpawner;
    void Start()
    {
        mapSpawner = MapSpawner.Instance;
        rb = this.GetComponent<Rigidbody>();
        blockSize = GameSettings.blockSize;
    }

    private void Update()
    {
        if ((x + z) % 2 == 0)
        {
            setOpaque();
        }
    }

    public void spawn(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
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
