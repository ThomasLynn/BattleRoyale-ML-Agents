using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaBulletScript : MonoBehaviour
{
    public AudioSource source;
    public float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        source.volume = transform.GetComponent<Rigidbody>().velocity.magnitude / maxSpeed;
        source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
