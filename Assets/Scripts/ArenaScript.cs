using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaScript : MonoBehaviour
{

    public List<Rigidbody> objectsToReset;

    private List<Vector3> startingPoses;
    private List<Quaternion> startingRots;
    private bool hasInit = false;
    // Start is called before the first frame update
    void Start()
    {
        startingPoses = new List<Vector3>();
        startingRots = new List<Quaternion>();
        for(int i = 0; i < objectsToReset.Count; i++)
        {
            startingPoses.Add(objectsToReset[i].position);
            startingRots.Add(objectsToReset[i].rotation);
        }
        hasInit = true;
    }

    public void resetEnv()
    {
        if (hasInit)
        {
            for (int i = 0; i < objectsToReset.Count; i++)
            {
                objectsToReset[i].position = startingPoses[i];
                objectsToReset[i].rotation = startingRots[i];
                objectsToReset[i].velocity = Vector3.zero;
                objectsToReset[i].angularVelocity = Vector3.zero;
            }
        }
    }
}
