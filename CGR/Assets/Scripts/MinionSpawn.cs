using UnityEngine;
using System.Collections;

public class MinionSpawn : MonoBehaviour
{
    public Animator anim;

    private int _bonusMinion = 1;
    private bool used = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (_bonusMinion == 0 && LevelManager.Instance.GetMinionCount() > 0)
            anim.SetBool("Stocked", false);
        else
            anim.SetBool("Stocked", true);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player" && (_bonusMinion > 0 || LevelManager.Instance.GetMinionCount() == 0))
        {
            if(_bonusMinion > 0)           
                _bonusMinion--;

            anim.SetBool("Stocked", false);
            anim.SetBool("Spawning", true);
        }            
    }

    void newMinion()
    {
        GameObject newMinion = (GameObject)Instantiate(Resources.Load("Prefabs/Minion"));
        newMinion.transform.position = transform.position;
        LevelManager.Instance.AddMinion(newMinion);
        anim.SetBool("Spawning", false);
    }

    void checkpointSave()
    {
        if(_bonusMinion != 0)
        {
            used = true;
        }
    }

    void checkpointLoad()
    {
        if (!used)
            _bonusMinion = 1;
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
