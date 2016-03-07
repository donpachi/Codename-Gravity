using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
    public void Start()
    {

    }

    private void PlayerHitCheckpoint()
    {

    }

    private IEnumerator PlayerHitCheckpointCo()
    {
        yield break;
    }

    public void PlayerLeftCheckpoint()
    {
    }

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);
    }

    public void AssignObjectToCheckpoint()
    {

    }
}
