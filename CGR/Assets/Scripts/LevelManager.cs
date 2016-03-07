using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Keeps track of the number of minions in a level
/// Also keeps track of where the player should spawn and where a checkpoint placed by the player might be
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set; }
    public FollowPlayer Camera { get; private set; }
    public int MinionCount;
    public int MinionLimit;
    
    private List<Checkpoint> _checkpoints;
    private int _currentCheckpointIndex;
    private Checkpoint _startPosition;
    private Checkpoint _currentCheckpoint;

    public Checkpoint DebugSpawn;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        _startPosition = FindObjectOfType<Checkpoint>();
        _checkpoints = FindObjectsOfType<Checkpoint>().ToList();
        _currentCheckpointIndex = _checkpoints.Count > 0 ? 0 : -1;

        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<FollowPlayer>();

#if UNITY_EDITOR
        if(DebugSpawn != null)
        {
            DebugSpawn.SpawnPlayer(Player);
        }
        else if(_currentCheckpointIndex != -1)
        {
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
        }

#else
        if (_currentCheckpointIndex != -1)
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
#endif

    }
    public void Update()
    {
        var isAtLastCheckPoint = _currentCheckpointIndex + 1 >= _checkpoints.Count;

    }

    public void KillPlayer()
    {

    }
    private IEnumerator KillPlayerCo()
    {
        yield break;
    }

    public void SpawnPlayer()
    {
        if (_currentCheckpoint == null)
            _startPosition.SpawnPlayer(Player);
        else
            _currentCheckpoint.SpawnPlayer(Player);
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }

    private void SetNewCheckpoint(TouchController.SwipeDirection direction)
    {
        if(direction == TouchController.SwipeDirection.DOWN)
        {
            if (_currentCheckpoint == null)
            {
                GameObject checkpoint = (GameObject)Instantiate(Resources.Load("Prefabs/Checkpoint"));
                _currentCheckpoint = checkpoint.GetComponent<Checkpoint>();
                
                _currentCheckpoint.transform.position = Player.transform.position;
            }
            else
            {
                _currentCheckpoint.transform.position = Player.transform.position;
            }
        }
    }

    //Event handling for swipe events
    void OnEnable()
    {
        TouchController.OnSwipe += SetNewCheckpoint;
    }
    void OnDisable()
    {
        TouchController.OnSwipe -= SetNewCheckpoint;
    }
}

