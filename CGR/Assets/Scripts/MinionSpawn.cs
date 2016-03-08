using UnityEngine;
using System.Collections;

public class MinionSpawn : MonoBehaviour
{

    public int minionsSpawned;
    public Animator anim;

    // Use this for initialization
    void Start()
    {
        minionsSpawned = 0;
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player" && minionsSpawned < 1 && !LevelManager.Instance.MinionLimitReached()) 
            anim.SetBool("Spawning", true);
    }

    void spawnMinion()
    {
        GameObject newMinion = (GameObject)Instantiate(Resources.Load("Prefabs/Minion"));
        newMinion.transform.position = transform.position;
        foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
            minion.SendMessage("updateList");

        foreach (GameObject minionSpawner in GameObject.FindGameObjectsWithTag("MinionSpawner"))
        {
            if (minionSpawner.name.Contains("Clone"))
                Destroy(minionSpawner);
            else
                minionSpawner.GetComponent<MinionSpawn>().minionsSpawned++;
        }
        anim.SetBool("Spawning", false);
    }

    void newMinion()
    {
        GameObject newMinion = (GameObject)Instantiate(Resources.Load("Prefabs/Minion"));
        newMinion.transform.position = transform.position;
        LevelManager.Instance.AddMinion(newMinion);
        anim.SetBool("Spawning", false);

    }
}
