using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
    public Animator anim { get; private set; }
    public LevelData data { get; private set; }

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SpawnPlayer(Player player)
    {
        player.CheckpointRespawn(transform);
    }

    public void SaveLevelState(LevelData newData)
    {
        data = newData;
    }
}

