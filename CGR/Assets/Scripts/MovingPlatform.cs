using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour {

    [Tooltip("The number of time the platform moves along the path until it stops. 0 = continuous")]
    public int NumberOfTraversals = 0;
    [Tooltip("Time in seconds for the platform to reach the last point")]
    public float TotalTravelTime = 1;
    public bool IsActive;
    public PlatformPath path;
    public Vector2 MovementVector { get; private set; }

    private int finishedPath = 0;   //number of times platform traveresed the entire path
    private float platformSpeed = 0;
    private IEnumerator<Transform> _currentPoint;
    private int currentDirection;
    private bool movementDone = false;
    private PlatformSaveState saveState;

	// Use this for initialization
	void Start () {
        //XDistRemain = XDistance;
        //YDistRemain = YDistance;
        
        if(path == null)
        {
            Debug.LogError("Path cannot be null", gameObject);
            return;
        }
        _currentPoint = path.GetPathEnumerator();
        _currentPoint.MoveNext();

        if (_currentPoint.Current == null)
            return;
        currentDirection = path.direction;
        transform.position = _currentPoint.Current.position;
        platformSpeed = path.GetTotalDistance() / TotalTravelTime;
        _currentPoint.MoveNext();
        MovementVector = Vector2.zero;
    }

    void Update()
    {
        if (_currentPoint == null || _currentPoint.Current == null)
            return;

        if (IsActive == true && !movementDone)
        {
            float distanceToTravel = platformSpeed * Time.deltaTime;
            float distanceToPointSquared = (transform.position - _currentPoint.Current.position).sqrMagnitude;

            Vector3 lastPosition = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current.position, distanceToTravel);

            if (distanceToPointSquared < distanceToTravel * distanceToTravel)
            {
                float remainingDistance = distanceToTravel - Mathf.Sqrt(distanceToPointSquared);
                _currentPoint.MoveNext();
                transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current.position, remainingDistance);
            }
            else if (distanceToPointSquared == distanceToTravel)
                _currentPoint.MoveNext();

            MovementVector = transform.position - lastPosition;
            if (currentDirection != path.direction)
            {
                currentDirection = path.direction;
                if (NumberOfTraversals > 0)
                {
                    finishedPath++;
                    if (finishedPath >= NumberOfTraversals)
                        movementDone = true;
                }
            }
        }
        else
            MovementVector = Vector2.zero;
    }
	
    void checkpointSave()
    {
        if (saveState == null)
            saveState = new PlatformSaveState();
        saveState.IsActive = IsActive;
        saveState.finishedPath = finishedPath;
    }

    void checkpointLoad()
    {
        if(saveState == null)
        {
            Debug.LogError("Checkpoint load was called before save state created on: " + gameObject);
            return;
        }
        path.CheckpointLoad();
        transform.position = path.GetCheckpointStartPosition().position;
        IsActive = saveState.IsActive;
        finishedPath = saveState.finishedPath;
        _currentPoint.MoveNext();
    }

    void plateDepressed()
    {
        if(NumberOfTraversals == 0)
            IsActive = !IsActive;
        else
        {
            movementDone = false;
            finishedPath = 0;
        }
    }

    void plateReleased()
    {

    }

    void OnEnable()
    {
        LevelManager.OnCheckpointLoad += checkpointLoad;
        LevelManager.OnCheckpointSave += checkpointSave;
    }
    void OnDisable()
    {
        LevelManager.OnCheckpointLoad -= checkpointLoad;
        LevelManager.OnCheckpointSave -= checkpointSave;
    }
}

class PlatformSaveState
{
    public bool IsActive;
    public int finishedPath;
}
