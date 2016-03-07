using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class RailDefiniton : MonoBehaviour {
    public Transform[] Points;
    private OrientationListener.Orientation[] directons;

    //next in enum will depend on gravity and point position
    public IEnumerator<Transform> GetPathEnumerator()
    {
        if (Points == null || Points.Length < 1)
            yield break;

        var index = 0;
        
        while (true)
        {
            yield return Points[index];
            

        }

    }

    void SetDirectionToNextPoint()
    {
        for (int i = 0; i < Points.Length; i++)
        {

        }
    }


    public void OnDrawGizmos()
    {
        if (Points == null || Points.Length < 2)
            return;

        for (var i = 1; i < Points.Length; i++)
        {
            Gizmos.DrawLine(Points[i - 1].position, Points[i].position);
        }
    }

}
