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

    private Vector3 lastPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
        Debug.DrawLine(transform.position, transform.position + transform.forward);
        wheel.Rotate(0, transform.InverseTransformVector(transform.position - lastPos).z * -60, 0);
        lastPos = transform.position;
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 move = actionBuffers.ContinuousActions[0] * transform.right + actionBuffers.ContinuousActions[1] * transform.forward;
        if (move.magnitude > 1)
        {
            move = move.normalized;
        }
        body.MovePosition( body.position + move * Time.fixedDeltaTime);
        Quaternion newAngles = body.rotation;
        Vector3 euler = newAngles.eulerAngles;
        euler.y += Mathf.Clamp(actionBuffers.ContinuousActions[2], -1f, 1f) * Time.fixedDeltaTime * 45;
        newAngles.eulerAngles = euler;
        body.MoveRotation(newAngles);
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
    }
}
