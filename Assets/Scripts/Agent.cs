using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class Agent : Unity.MLAgents.Agent
{

    public Transform wheel;
    public Rigidbody body;
    public Transform bananaTip;
    public GameObject bulletPrefab;
    public ArenaScript arena;
    public List<Rigidbody> otherBots;

    public float wheelSpeedMultiplier;
    public float botSpeed;
    public float botRotationSpeed;
    public Color bananaLightColor;

    public int maxPower;
    public int powerLoss;
    public int minPower;
    public int powerIncreaseSpeed;
    public int maxCooldown;
    public int cooldownSpeed;

    private Vector3 lastPos = Vector3.zero;
    private int power = 0;
    private int cooldown = 0;
    private Vector3 startingPos;
    private Quaternion startingRot;
    private List<Vector3> previousPosOtherBots;
    private float previousTime;

    private List<GameObject> bullets = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        startingPos = body.position;
        startingRot = body.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        Debug.DrawLine(transform.position, transform.position + transform.forward);
        wheel.Rotate(0, transform.InverseTransformVector(transform.position - lastPos).z * wheelSpeedMultiplier, 0);
        lastPos = transform.position;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Apply linear movement
        Vector3 move = actionBuffers.ContinuousActions[0] * transform.right + actionBuffers.ContinuousActions[1] * transform.forward;
        if (move.magnitude > 1)
        {
            move = move.normalized;
        }
        move = move * botSpeed;
        body.MovePosition( body.position + move * Time.fixedDeltaTime);

        // Apply rotation
        Quaternion newAngles = body.rotation;
        Vector3 euler = newAngles.eulerAngles;
        euler.y += Mathf.Clamp(actionBuffers.ContinuousActions[2], -1f, 1f) * Time.fixedDeltaTime * botRotationSpeed;
        newAngles.eulerAngles = euler;
        body.MoveRotation(newAngles);

        // Shoot
        if (power < maxPower)
        {
            power+= powerIncreaseSpeed;
        }
        if (cooldown > 0)
        {
            cooldown-= cooldownSpeed;
        }
        if (power > minPower && cooldown <= 0)
        {
            if (actionBuffers.DiscreteActions[0] == 1)
            {
                GameObject bullet = Instantiate(bulletPrefab, bananaTip.position, bananaTip.rotation, transform) as GameObject;
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * power;
                bullet.GetComponent<Light>().color = bananaLightColor;
                power -= powerLoss;
                cooldown = maxCooldown;
                bullets.Add(bullet);
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float x = body.rotation.eulerAngles.x;
        float z = body.rotation.eulerAngles.z;
        if (x > 180f)
        {
            x -= 360f;
        }
        x /= 90f;
        if (z > 180f)
        {
            z -= 360f;
        }
        z /= 90f;
        //print("x " + x + " z " + z);
        sensor.AddObservation(Mathf.Atan(x));
        sensor.AddObservation(Mathf.Atan(z));

        if (previousPosOtherBots == null)
        {
            previousTime = Time.time;
            previousPosOtherBots = new List<Vector3>();
            foreach (Rigidbody rb in otherBots)
            {
                previousPosOtherBots.Add(rb.position);
                sensor.AddObservation(Vector3.zero);
            }
        }
        else
        {
            float currentTime = Time.time;
            for (int i=0;i<otherBots.Count;i++)
            {
                // rescaled relative (rotated) velocity between -1 and 1
                Vector3 v = transform.InverseTransformDirection(otherBots[i].position - previousPosOtherBots[i]) / (currentTime - previousTime) / 4f;
                previousPosOtherBots[i] = otherBots[i].position;
                //print("rel v " + v);
                sensor.AddObservation(v);
            }
            previousTime = currentTime;
        }
        for (int i = 0; i < otherBots.Count; i++)
        {
            // rescaled relative position between -1 and 1
            // 20 is the size of the arena, uses sqrt(2) because euclidean space goes brrr
            Vector3 p = transform.InverseTransformDirection(otherBots[i].position - transform.parent.position) / (20f / Mathf.Sqrt(2));
            //print("rel p " + p + " mag " + p.magnitude);
            sensor.AddObservation(p);
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

        if (Input.GetKey(KeyCode.W))
        {
            actionsOut.ContinuousActions.Array[1] = 1.0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            actionsOut.ContinuousActions.Array[1] = -1f;
        }
        else
        {
            actionsOut.ContinuousActions.Array[1] = 0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            actionsOut.ContinuousActions.Array[0] = 1.0f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut.ContinuousActions.Array[0] = -1f;
        }
        else
        {
            actionsOut.ContinuousActions.Array[0] = 0f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            actionsOut.ContinuousActions.Array[2] = 1.0f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            actionsOut.ContinuousActions.Array[2] = -1f;
        }
        else
        {
            actionsOut.ContinuousActions.Array[2] = 0f;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            actionsOut.DiscreteActions.Array[0] = 1;
        }
        else
        {
            actionsOut.DiscreteActions.Array[0] = 0;
        }
    }

    public override void OnEpisodeBegin()
    {
        foreach(GameObject gm in bullets)
        {
            Destroy(gm);
        }
        bullets.Clear();
        body.position = startingPos;
        body.rotation = startingRot;
        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        power = 0;
        cooldown = 0;
        arena.resetEnv();
    }
}
