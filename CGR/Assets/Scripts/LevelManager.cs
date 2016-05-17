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
    public LayerMask checkpointRayMask;
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
    private LevelData checkpointSaveData;

    public Checkpoint DebugSpawn;
    public int BonusCutoffSeconds;
    public int BonusSecondMultiplyer;
    public float MinionLeaderDist = 0.1f;

    public delegate void CheckpointSave();
    public static event CheckpointSave OnCheckpointSave;

    public delegate void CheckpointLoad();
    public static event CheckpointLoad OnCheckpointLoad;

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

    void triggerSave()
    {
        if (OnCheckpointSave != null)
            OnCheckpointSave();
    }

    void trigerLoad()
    {
        if (OnCheckpointLoad != null)
            OnCheckpointLoad();
    }

    /// <summary>
    /// Adds a minion to the minion list
    /// Function sets the parent of the minion to the last in this list
    /// If no minions exist, sets parent to null 
    /// </summary>
    /// <param name="minion"></param>
    public void AddMinion(GameObject minion)
    {
        Minion minionScript = minion.GetComponent<Minion>();

        if (_minionList.Count == 0)
        {
            minionScript.SetParent(Player.gameObject);
            minionScript.MinionDistance = MinionLeaderDist;
        }
        else
        {
            minionScript.SetParent(_minionList.Last());
        }
        _minionList.Add(minion);
        reorderMinions();
    }

    /// <summary>
    /// Fixes order of the minions so they render properly
    /// </summary>
    void reorderMinions()
    {
        int j = _minionList.Count - 1;
        for (int i = 0; i < _minionList.Count; i++)
        {
            _minionList[i].GetComponent<Minion>().SetRenderOrder(j);
            j--;
        }
    }

    /// <summary>
    /// Removes minions from the list. NOT from the game
    /// </summary>
    /// <param name="minion"></param>
    public void RemoveMinion(GameObject minion, bool destroy)
    {
        if(_minionList.Count == 0)
        {
            Debug.LogError("There are no objects in the list, including this one", minion);
            return;
        }
        _minionList.Remove(minion);

        if (destroy)
            Destroy(minion);
    }

    public void GotoNextLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void ReloadFromCheckpoint()
    {
        if(_currentCheckpoint == null)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
        {
            trigerLoad();
            reinitializeMinions();
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        Player.LoadPlayerState(checkpointSaveData.PlayerState);

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
        if (_minionList.Count > 0)
        {
            _minionList[0].GetComponent<Minion>().SetParent(Player.gameObject); //set the new front minion parent to player
            _minionList[0].GetComponent<Minion>().MinionDistance = MinionLeaderDist;
        }
        return minion;
    }

    /// <summary>
    /// Request a new checkpoint to be set
    /// </summary>
    /// <param name="requestedObj">The object that requested the checkpoint</param>
    public bool NewCheckpointRequest(GameObject requestedObj)
    {
        RaycastHit2D groundCheckRay = Physics2D.Raycast(Player.transform.position, Player.transform.up * -1, 1, checkpointRayMask);
        if (!groundCheckRay || groundCheckRay.collider.tag == "Dynamic")
            return false;

        if (requestedObj.GetComponent<Minion>() != null)
        {
            requestedObj.GetComponent<Minion>().DeactivateControl(StateChange.CHECKPOINT);
            positionCheckpoint(groundCheckRay);
            return true;
        }
        else if(requestedObj.GetComponent<Player>() != null && _minionList.Count != 0)
        {
            _minionList[_minionList.Count - 1].GetComponent<Minion>().DeactivateControl(StateChange.CHECKPOINT);
            RemoveMinion(_minionList[_minionList.Count - 1], false);
            positionCheckpoint(groundCheckRay);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Sets the new checkpoint location, triggers the animation for the checkpoint too
    /// </summary>
    public void setNewCheckpoint()
    {
        _currentCheckpoint.anim.SetBool("Spawn", true);
        createLevelSave();
        triggerSave();
    }

    private void positionCheckpoint(RaycastHit2D collisionPoint)
    {
        if (_currentCheckpoint == null)
        {
            GameObject checkpoint = (GameObject)Instantiate(Resources.Load("Prefabs/Checkpoint"));
            _currentCheckpoint = checkpoint.GetComponent<Checkpoint>();
        }
        _currentCheckpoint.rend.enabled = false;
        _currentCheckpoint.anim.SetBool("Spawn", false);
        _currentCheckpoint.transform.position = collisionPoint.point;
        _currentCheckpoint.transform.rotation = Player.transform.rotation;
    }

    private void createLevelSave()
    {
        if(checkpointSaveData == null)
            checkpointSaveData = new LevelData();
        checkpointSaveData.MinionCount = _minionList.Count;
        checkpointSaveData.PlayerState = Player.SavePlayerState();
    }

    private void reinitializeMinions()
    {
        if(_minionList.Count > checkpointSaveData.MinionCount)
        {
            while (_minionList.Count > checkpointSaveData.MinionCount)
            {
                RemoveMinion(_minionList[_minionList.Count - 1], true);
            }
        }
        else
        {
            while (_minionList.Count < checkpointSaveData.MinionCount)
            {
                GameObject newMinion = (GameObject)Instantiate(Resources.Load("Prefabs/Minion"));
                newMinion.transform.position = Player.transform.position;
                AddMinion(newMinion);
            }
        }
    }
}

public class LevelData
{
    public int MinionCount;
    public PlayerState PlayerState;
}