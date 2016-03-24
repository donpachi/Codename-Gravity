using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Keeps track of the minions in a level
/// Also keeps track of where the player should spawn and where a checkpoint placed by the player might be
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Player Player { get; private set; }
    public FollowPlayer Camera { get; private set; }
    public int InitialMinionCount;
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }

    public int timeBonus
    {
        get
        {
            var secondDiff = (int)(BonusCutoffSeconds - RunningTime.TotalSeconds);
            return Mathf.Max(0, secondDiff) * BonusSecondMultiplyer;
        }
    }

    private List<Checkpoint> _checkpoints;
    private int _currentCheckpointIndex;
    private Checkpoint _startPosition;
    private Checkpoint _currentCheckpoint;
    private List<GameObject> _minionList;
    private DateTime _started;
    private int checkpointMinionCount;  //The minion count when the checkpoint happened

    public Checkpoint DebugSpawn;
    public int BonusCutoffSeconds;
    public int BonusSecondMultiplyer;
    public float MinionLeaderDist = 0.1f;

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

        _started = DateTime.UtcNow;

        initializeMinions();

#if UNITY_EDITOR
        if (DebugSpawn != null)
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

    /// <summary>
    /// Adds a minion to the minion list
    /// Function sets the parent of the minion to the last in this list
    /// If no minions exist, sets parent to null 
    /// </summary>
    /// <param name="minion"></param>
    public void AddMinion(GameObject minion)
    {
        if (_minionList.Count == 0)
        {
            minion.GetComponent<Minion>().SetParent(null);
            minion.GetComponent<Minion>().MinionDistance = MinionLeaderDist;
        }
        else
        {
            minion.GetComponent<Minion>().SetParent(_minionList.Last());
        }
        _minionList.Add(minion);
    }

    /// <summary>
    /// Removes minions from the list. NOT from the game
    /// </summary>
    /// <param name="minion"></param>
    public void RemoveMinion(GameObject minion)
    {
        if(_minionList.Count == 0)
        {
            Debug.LogError("There are no objects in the list, including this one", minion);
            return;
        }
        _minionList.Remove(minion);
    }

    public void GotoNextLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void SpawnPlayer()
    {
        if (_currentCheckpoint == null)
        {
            _startPosition.SpawnPlayer(Player);
        }
        else
        {
            _currentCheckpoint.SpawnPlayer(Player);
        }
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }

    public int GetMinionCount()
    {
        return _minionList.Count;
    }

    private void initializeMinions()
    {
        _minionList = new List<GameObject>();
        for (int i = 0; i < InitialMinionCount; i++)
        {
            GameObject newMinion = (GameObject)Instantiate(Resources.Load("Prefabs/Minion"));
            newMinion.transform.position = Player.transform.position;
            AddMinion(newMinion);
        }
    }

    /// <summary>
    /// Gets the front most minion and removes it from the list
    /// </summary>
    /// <returns>Minion</returns>
    public GameObject GetMinion()
    {
        GameObject minion = _minionList[0]; //get the minion at the head of the line
        _minionList.RemoveAt(0);        //remove him from the list
        _minionList[0].GetComponent<Minion>().SetParent(null); //set the new front minion parent to null
        _minionList[0].GetComponent<Minion>().MinionDistance = MinionLeaderDist;
        return minion;
    }

    /// <summary>
    /// Request a new checkpoint to be set
    /// </summary>
    /// <param name="requestedObj">The object that requested the checkpoint</param>
    public void NewCheckpointRequest(GameObject requestedObj)
    {
        if(requestedObj.GetComponent<Minion>() != null)
        {
            setNewCheckpoint(requestedObj);
            requestedObj.GetComponent<Animator>().SetBool("Spirit", true);
        }
        else if(requestedObj.GetComponent<Player>() != null && _minionList.Count != 0)
        {
            setNewCheckpoint(_minionList[_minionList.Count - 1]);
            _minionList[_minionList.Count - 1].GetComponent<Animator>().SetBool("Spirit", true);
            RemoveMinion(_minionList[_minionList.Count - 1]);
        }
        checkpointMinionCount = _minionList.Count;
    }

    private void setNewCheckpoint(GameObject indicator)
    {
        if (_currentCheckpoint == null)
        {
            GameObject checkpoint = (GameObject)Instantiate(Resources.Load("Prefabs/Checkpoint"));
            _currentCheckpoint = checkpoint.GetComponent<Checkpoint>();
        }
        _currentCheckpoint.AssignObjectToCheckpoint(indicator);
        _currentCheckpoint.transform.position = Player.transform.position;
        indicator.GetComponent<Minion>().SetParent(_currentCheckpoint.gameObject);
        indicator.GetComponent<Minion>().isFollowing = false;
    }
}