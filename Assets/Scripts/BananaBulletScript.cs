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
            Agent own = transform.parent.GetComponent<Agent>();
            Agent enemy = collision.transform.GetComponent<Agent>();
            if (collision.transform != transform.parent)
            {
                //print("hit enemy");
                if (enemy.points > 0)
                {
                    enemy.AddPoints(-1);
                    own.AddPoints(1);
                    own.AddReward(0.2f);
                    enemy.AddReward(-0.2f);
                }
            }
            else
            {
                //print("hit self");
                own.AddReward(-0.05f);
            }
        }
    }
}
