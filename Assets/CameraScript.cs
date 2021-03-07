using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void Start()
    {
        AudioListener.volume = 0.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            if (AudioListener.volume == 0.0f)
            {
                AudioListener.volume = 1.0f;
            }
            else
            {
                AudioListener.volume = 0.0f;
            }
        }
    }
}
