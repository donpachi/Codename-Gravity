using UnityEngine;
using System.Collections;

public class MinionSpawn : MonoBehaviour {

    public int maxMinions = 1;
    int spawnedMinions = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (spawnedMinions < maxMinions && collider.name == "Player")
        {
            spawnedMinions++;
            GameObject newMinion = (GameObject)Instantiate(Resources.Load("Prefabs/Minion"));
            newMinion.transform.position = transform.position;
            foreach (GameObject minion in GameObject.FindGameObjectsWithTag("Minion"))
            {
                minion.SendMessage("updateList");
            }
        }
    }
}
