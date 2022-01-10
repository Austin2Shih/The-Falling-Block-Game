using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHeightMap : MonoBehaviour
{

    public Text textElement;
    BlockSpawner blockSpawner;
    // Start is called before the first frame update
    void Start()
    {
        blockSpawner = BlockSpawner.Instance;
        textElement.text = "" + GameSettings.playerScore;
    }

    // Update is called once per frame
    void Update()
    {
        displayGrid();
    }

    public void displayGrid()
    {
        string debugMessage = "";
        for (int i = 0; i < blockSpawner.gridX; i++)
        {
            for (int j = 0; j < blockSpawner.gridZ; j++)
            {
                debugMessage += blockSpawner.heightMap[i, j] + "  ";
            }
            debugMessage += "\n";
        }
        textElement.text = debugMessage;
    }
}