using UnityEngine;
using System.Collections.Generic;

public class PlatformPath : MonoBehaviour {
    public Transform[] Path;
    public int direction = 1;

    public IEnumerator<Transform> GetPathEnumerator()
    {
        if (Path == null || Path.Length < 1)
            yield break;
        int index = 0;
        while (true)
        {
            yield return Path[index];

            if (Path.Length == 1)
                continue;

            if (index <= 0)
                direction = 1;
            else if (index >= Path.Length - 1)
                direction = -1;
            
            index = index + direction;
        }
    }

    public float GetTotalDistance()
    {
        float totalDistance = 0;
        Transform lastPoint = Path[0];

        foreach (var point in Path)
        {
            totalDistance += (lastPoint.position - point.position).magnitude;
            lastPoint = point;
        }
        return totalDistance;
    }

    public void OnDrawGizmos()
    {
        if (Path == null || Path.Length < 2)
            return;

        for (var i = 1; i < Path.Length; i++)
        {
            if(Path[i] == null)
            {
                Debug.LogError("Array contains null elements at " + i + " For object: " + gameObject);
                break;
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Path[i - 1].position, Path[i].position);
        }
    }
}
