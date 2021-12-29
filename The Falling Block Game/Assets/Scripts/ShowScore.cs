using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{

    public Text textElement;
    // Start is called before the first frame update
    void Start()
    {
        textElement.text = "" + GameSettings.playerScore;
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = "" + GameSettings.playerScore;
    }
}
