using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{

    public Agent targetAgent;
    public string startingText;
    public Text text;
    public RectTransform trans;
    public Vector2 offset;

    // Update is called once per frame
    void Update()
    {
        text.text = startingText + targetAgent.points;
        trans.anchoredPosition = new Vector2(-Screen.width / 2, Screen.height / 2) + offset;
    }
}
