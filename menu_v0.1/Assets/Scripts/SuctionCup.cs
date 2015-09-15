using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SuctionCup : MonoBehaviour {

    public float suctionTimer;

    private Vector3 zAxis =  new Vector3(0,0,1);
    private Vector3 portrait = new Vector3(150, 280, 0);
    private Vector3 landscapeRight = new Vector3(-150, 280, 0);
    private Vector3 portraitUpsideDown = new Vector3(-150, -280, 0);
    private Vector3 landscapeLeft = new Vector3(150, -280, 0);

    private float timer;
    private bool triggered;
    private GameObject player;
    private GameObject suctionText;
    private Rigidbody2D playerBody;
	private Vector2 suctionVector;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        suctionText = GameObject.Find("SuctionText");
        timer = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.deviceOrientation == DeviceOrientation.Portrait) {
            suctionText.transform.localPosition = portrait;
            suctionText.transform.rotation = Quaternion.AngleAxis(0, zAxis);
        }
        if (Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
            suctionText.transform.localPosition = landscapeRight;
            suctionText.transform.rotation = Quaternion.AngleAxis(90, zAxis);
        }
        if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown){
            suctionText.transform.localPosition = portraitUpsideDown;
            suctionText.transform.rotation = Quaternion.AngleAxis(180, zAxis);
        }
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
        {
            suctionText.transform.localPosition = landscapeLeft;
            suctionText.transform.rotation = Quaternion.AngleAxis(270, zAxis);
        }

        if (timer != 0)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
                suctionText.GetComponent<Text>().text = timer.ToString();

            else
            {
                suctionCupBootsEnd();
                timer = 0;
                suctionText.GetComponent<Text>().text = "";
                this.gameObject.SetActive(false);
            }
        }

	}

	void OnTriggerEnter2D(Collider2D collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player" && !triggered) {
			suctionVector = Physics2D.gravity;
            playerBody = collisionInfo.GetComponent<Rigidbody2D>();
			playerBody.gravityScale = 0.0f;
            playerBody.GetComponent<ConstantForce2D>().relativeForce = suctionVector * 2;
            player.GetComponent<Controls>().SuctionStatusOn();
            this.transform.Translate(0, 0, 3);

            triggered = true;
            timer = suctionTimer;
		}
	}

    private void suctionCupBootsEnd()
    {
        playerBody.GetComponent<ConstantForce2D>().relativeForce = new Vector2(0, 0);
        playerBody.gravityScale = 1.0f;
        player.GetComponent<Controls>().SuctionStatusEnd();
    }
}
