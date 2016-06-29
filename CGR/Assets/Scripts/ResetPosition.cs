using UnityEngine;
using System.Collections;

public class ResetPosition : MonoBehaviour {

    private Vector2 savePosition;
    private Rigidbody2D rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void CheckpointRestart()
    {
        if (savePosition == null)
        {
            Debug.LogError("Object does not have a save state");
            return;
        }
        gameObject.transform.position = savePosition;
        if (rBody)
            StartCoroutine(freezeObject(2));
            
    }

    void SetCheckpointState()
    {
        savePosition = gameObject.transform.position;
    }

    IEnumerator freezeObject(float time)
    {
        float formerGravity = rBody.gravityScale;
        rBody.gravityScale = 0;
        rBody.Sleep();
        yield return new WaitForSeconds(time);

        rBody.gravityScale = formerGravity;
    }

    //Listeners for player
    void OnEnable()
    {
        LevelManager.OnCheckpointLoad += CheckpointRestart;
        LevelManager.OnCheckpointSave += SetCheckpointState;
    }

    void OnDisable()
    {
        LevelManager.OnCheckpointLoad -= CheckpointRestart;
        LevelManager.OnCheckpointSave -= SetCheckpointState;
    }

}
