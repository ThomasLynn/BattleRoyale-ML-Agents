using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaBulletScript : MonoBehaviour
{
    public AudioSource source;
    public float maxSpeed;
    public float timeToLive;

    private float creationTime;
    // Start is called before the first frame update
    void Start()
    {
        source.volume = transform.GetComponent<Rigidbody>().velocity.magnitude / maxSpeed;
        source.Play();
        creationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (creationTime + timeToLive < Time.time)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Bot")
        {
            if (collision.transform != transform.parent)
            {
                //print("hit enemy");
                transform.parent.GetComponent<Agent>().AddReward(0.05f);
                collision.transform.GetComponent<Agent>().AddReward(-0.05f);
            }
            else
            {
                //print("hit self");
                transform.parent.GetComponent<Agent>().AddReward(-0.05f);
            }
        }
    }
}
