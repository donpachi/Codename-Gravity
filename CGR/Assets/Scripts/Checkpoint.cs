using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
    public Animator anim { get; private set; }
    public SpriteRenderer rend { get; private set; }
    public LevelData data { get; private set; }
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    public void SpawnPlayer(Player player)
    {
        player.CheckpointRespawn(transform);
    }
}

