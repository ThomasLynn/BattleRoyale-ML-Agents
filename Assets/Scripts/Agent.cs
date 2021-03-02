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

    public float wheelSpeedMultiplier;

    public int maxPower;
    public int powerLoss;
    public int minPower;
    public float powerMultiplier;
    public int maxCooldown;

    private Vector3 lastPos = Vector3.zero;
    private int power = 0;
    private int cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
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
        body.MovePosition( body.position + move * Time.fixedDeltaTime);

        // Apply rotation
        Quaternion newAngles = body.rotation;
        Vector3 euler = newAngles.eulerAngles;
        euler.y += Mathf.Clamp(actionBuffers.ContinuousActions[2], -1f, 1f) * Time.fixedDeltaTime * 45;
        newAngles.eulerAngles = euler;
        body.MoveRotation(newAngles);

        // Shoot
        if (power < maxPower)
        {
            power++;
        }
        if (cooldown > 0)
        {
            cooldown--;
        }
        if (power > minPower && cooldown <= 0)
        {
            if (actionBuffers.DiscreteActions[0] == 1)
            {
                GameObject bullet = Instantiate(bulletPrefab, bananaTip.position, bananaTip.rotation, transform) as GameObject;
                bullet.GetComponent<Rigidbody>().velocity = transform.forward * powerMultiplier * power;
                power -= powerLoss;
                cooldown = maxCooldown;
            }
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
}
