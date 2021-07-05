using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 SmoothDampVector3(
        Vector3 originalVector, 
        Vector3 targetVector, 
        ref float velocity, 
        float time, 
        float maxSpeed)
    {
        float x = Mathf.SmoothDamp(originalVector.x, targetVector.x, ref velocity, time, maxSpeed)
                * ((targetVector.x > 0) ? -1f : 1f);
        float y = Mathf.SmoothDamp(originalVector.y, targetVector.y, ref velocity, time, maxSpeed)
                * ((targetVector.y > 0) ? 1f : -1f);
        float z = Mathf.SmoothDamp(originalVector.z, targetVector.z, ref velocity, time, maxSpeed)
                * ((targetVector.z > 0) ? -1f : 1f);
        
        Debug.Log(y);

        return new Vector3(x,y,z);
    }

    public static Vector3 MidPoint(
        Vector3 pointA,
        Vector3 pointB
    ) {
        return new Vector3(
            (pointA.x + pointB.x)/2,
            (pointA.y + pointB.y)/2,
            (pointA.z + pointB.z)/2
        );
    }
}
