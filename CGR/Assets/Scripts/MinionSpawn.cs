using UnityEngine;
using System.Collections;

public class MinionSpawn : MonoBehaviour {

    public int minionsSpawned;

	// Use this for initialization
	void Start () {
        minionsSpawned = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Player" && minionsSpawned < 1)
        {
                GameObject newMinion = (GameObject)Instantiate(Resources.Load("Prefabs/Minion"));
                newMinion.transform.position = transform.position;
                foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
                {
                    minion.SendMessage("updateList");
                }
                
            foreach (GameObject minionSpawner in GameObject.FindGameObjectsWithTag("MinionSpawner"))
            {
                minionSpawner.GetComponent<MinionSpawn>().minionsSpawned++;
            }
        }
    }
}
