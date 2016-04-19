using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
    public Animator anim { get; private set; }
    private GameObject _checkpointIndicator;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);
    }
}
