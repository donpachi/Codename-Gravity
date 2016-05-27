using UnityEngine;
using System.Collections.Generic;

public class PlatformPath : MonoBehaviour {
    public Transform[] Path;
    public int direction = 1;

    private int index = 0;
    private PathSaveState saveState;

    public IEnumerator<Transform> GetPathEnumerator()
    {
        if (Path == null || Path.Length < 1)
            yield break;
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

    public Transform GetStartPoisition()
    {
        return Path[index];
    }

    public void OnDrawGizmos()
    {
        if (Path == null || Path.Length < 2)
            return;

        for (var i = 1; i < Path.Length; i++)
        {
            if (Path[i] == null)
            {
                Debug.LogError("Array contains null elements at " + i + " For object: " + gameObject);
                break;
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Path[i - 1].position, Path[i].position);
        }
    }
    
    public Transform GetCheckpointStartPosition()
    {
        if(saveState == null)
        {
            Debug.LogError("Load checkpoint was called before save state was made");
            return null;
        }
        else
        {
            return saveState.startPosition;
        }
    }

    public void CheckpointLoad()
    {
        if (saveState == null)
        {
            Debug.LogError("Checkpoint Load called before save state was made");
            return;
        }
        index = saveState.index;
        direction = saveState.direction;
    }

    void checkpointSave()
    {
        if (saveState == null)
            saveState = new PathSaveState();
        
        if (Path.Length == 1)
            return;

        saveState.index = index - direction;
        if (saveState.index == 0)
        {
            saveState.direction = -1;
        }
        else if (saveState.index == Path.Length - 1)
        {
            saveState.direction = 1;
        }
        else
            saveState.direction = direction;
        saveState.startPosition = Path[saveState.index];
    }

    void OnEnable()
    {
        LevelManager.OnCheckpointSave += checkpointSave;
    }
    void OnDisable()
    {
        LevelManager.OnCheckpointSave -= checkpointSave;
    }
}

class PathSaveState
{
    public int index;
    public int direction;
    public Transform startPosition;
}
