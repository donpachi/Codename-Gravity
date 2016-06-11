using UnityEngine;
using System.Collections;

public class BreakableWall : MonoBehaviour {

	public Animator anima;
    public GameObject BrokenBlock;
	public float breakThreshold;

    Collider2D colliderBox;
    SpriteRenderer rend;
    bool broken = false;

	// Use this for initialization
	void Start () {
        colliderBox = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D(Collision2D collisionInfo) {
        if ((collisionInfo.gameObject.tag == "Pushable" || collisionInfo.gameObject.tag == "Boulder" || collisionInfo.gameObject.tag == "Hazard") && collisionInfo.relativeVelocity.magnitude > breakThreshold) {
            broken = true;
            colliderBox.enabled = false;
            rend.enabled = false;

            if (anima != null)
                anima.SetTrigger("destroy");
            else if (BrokenBlock != null)
            {
                GameObject blockDebris = Instantiate(BrokenBlock);
                blockDebris.transform.position = transform.position;
                spreadDebris(blockDebris, collisionInfo.gameObject.transform.position);
            }
		}
	}

	public void eraseWall()
	{
		this.gameObject.SetActive(false);
	}

    void spreadDebris(GameObject blockDebris, Vector2 origin)
    {
        Rigidbody2D[] rBodies = blockDebris.GetComponentsInChildren<Rigidbody2D>();
        foreach (var debris in rBodies)
        {
            Vector2 direction = (Vector2)debris.transform.position - origin;
            debris.AddForce(direction * Random.Range(0.8f,2f), ForceMode2D.Impulse);
        }
    }

    void SaveState()
    {

    }

    void LoadState()
    {

    }

    void OnEnable()
    {
        LevelManager.OnCheckpointLoad += LoadState;
        LevelManager.OnCheckpointSave += SaveState;
    }

    void OnDisable()
    {
        LevelManager.OnCheckpointLoad -= LoadState;
        LevelManager.OnCheckpointSave -= SaveState;
    }

}
