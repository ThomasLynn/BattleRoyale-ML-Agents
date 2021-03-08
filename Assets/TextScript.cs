using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{

    public Agent targetAgent;
    public string startingText;
    public Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = startingText + targetAgent.points;
    }
}
