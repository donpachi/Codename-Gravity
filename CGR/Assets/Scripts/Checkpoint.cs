using UnityEngine;

public class Checkpoint : MonoBehaviour 
{
    private GameObject _checkpointIndicator;

    public void SpawnPlayer(Player player)
    {
        player.RespawnAt(transform);
    }

    /// <summary>
    /// Assign the indicator obj.
    /// If old one exist, it will destroy it
    /// </summary>
    /// <param name="indicator"></param>
    public void AssignObjectToCheckpoint(GameObject indicator)
    {
        if (_checkpointIndicator != null)
            _checkpointIndicator.GetComponent<Animator>().SetTrigger("DestroySpirit");
        _checkpointIndicator = indicator;
    }
}
