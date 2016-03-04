using UnityEngine;
using System.Collections;

public class WallTurret : MonoBehaviour {

	public GameObject projectile;
	private float timer;
	private Vector3 spawnLocation;

	//Publicly changable variables
	public float FireRate;
	public float InitialDelay;
	public float ProjectileForce;
	public Vector3 SpawnOffset;
	public Vector2 LaunchForce;
	public Quaternion LaunchAngle;
    public Animator anim;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
		timer = InitialDelay;
		spawnLocation = gameObject.transform.position + SpawnOffset;
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer < 0) {
            anim.SetBool("isFiring", true);
			GameObject newMissile = (GameObject) Instantiate(projectile, spawnLocation, Quaternion.identity);
			newMissile.GetComponent<Rigidbody2D>().AddForce(LaunchForce);
			timer = FireRate;
            anim.SetBool("isFiring", false);
		}
	}
}