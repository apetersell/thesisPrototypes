using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloPlayers : MonoBehaviour {

	SpriteRenderer sr;
	Rigidbody2D rb;

	public int playerNum;
	public float HP;
	public float moveSpeed;
	public float airSpeedModifier;
	public float jumpSpeed;
	public float normalGrav;
	public float fastFallGrav;
	public float gravityThreshold;
	public int directionModifier;
	public int maxAirActions;
	public int airActionsRemaining;
	public bool touchingGround; 
	public bool actionable = true; 
	public bool affectedByGrav = true; 
	public bool touchingWall= false; 
	public bool stopRightMomentum = false; 
	public bool stopLeftMomentum = false; 
	public bool passThroughPlatforms; 
	public bool canShoot = true; 
	public float currentShotDelay; 
	public float shotDelay;
	public Color defaultColor;
	public Color fusionColor;
	public Color lerpingColor;
	public float lerpSpeed;

	public bool readyToFuse; 


	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		Debug.Log (Input.GetAxis ("RTrigger_P" + playerNum));

		if (actionable) 
		{
			actions ();
		}
			
		movementSmoothing (); 
		handleShotDelay (); 

		if (readyToFuse) {
			sr.color = lerpingColor;
		} else {
			sr.color = defaultColor;
		}

		lerpingColor = Color.Lerp(defaultColor, fusionColor, Mathf.PingPong(Time.time*lerpSpeed, 1));
		
	}

	void actions ()
	{
		//	Left and right movement.
		if ((Input.GetAxis ("LeftStickX_P" + playerNum) > 0) && stopRightMomentum == false) 
		{
			sr.flipX = false;
			directionModifier = 1;
			if (touchingGround) {
				rb.velocity = new Vector2 (moveSpeed, rb.velocity.y);
			} 
			else 
			{
				rb.velocity = new Vector2 (moveSpeed * airSpeedModifier, rb.velocity.y);
			}
		}

		if ((Input.GetAxis ("LeftStickX_P" + playerNum) < 0) && stopLeftMomentum == false) 
		{
			sr.flipX = true;
			directionModifier = -1;
			if (touchingGround) {
				rb.velocity = new Vector2 (moveSpeed * -1, rb.velocity.y);
			} 
			else 
			{
				rb.velocity = new Vector2 (((moveSpeed * -1) * airSpeedModifier), rb.velocity.y);
			}
		}

		if (Input.GetAxis ("LeftStickX_P" + playerNum) == 0)
		{
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}

		//Jumping
		if (Input.GetButtonDown ("AButton_P" + playerNum)) 
		{
			if (touchingGround) 
			{
				jump (jumpSpeed);
			} 
			else if (touchingGround == false && airActionsRemaining > 0) 
			{
				jump (jumpSpeed * .85f);
				airActionsRemaining--;
			}
		}

		//FastFalling/Fallling through soft platforms
		if (affectedByGrav) 
		{
			if (Input.GetAxis ("LeftStickY_P" + playerNum) > gravityThreshold) {
				rb.gravityScale = fastFallGrav;
				passThroughPlatforms = true;
			} else {
				rb.gravityScale = normalGrav;
				passThroughPlatforms = false;
			}
		}

		//Projectile Attack
		if (Input.GetButtonDown ("BButton_P" + playerNum))
		{
			if (canShoot) 
			{
				GameObject p = GameObject.Find ("Player_" + playerNum);
				float modX = (Input.GetAxis ("LeftStickX_P" + playerNum));
				float modY = (Input.GetAxis ("LeftStickY_P" + playerNum)) * -1;
				GameObject beam = null;
				if (modX == 0 && modY == 0) {
					modX = directionModifier;
				}
				if (modY < 0 && touchingGround == true) {
					modX = directionModifier;
					modY = 0;
				}
				Vector3 direction = new Vector3 (modX, modY, 0).normalized;
				beam = Instantiate(Resources.Load("Prefabs/Projectiles/SoloBeamPrefab")) as GameObject;
				beam.GetComponent<Beams> ().modPos = new Vector3 (modX, modY, 0).normalized;
				beam.transform.position = transform.position + beam.GetComponent<Beams> ().modPos;
				beam.GetComponent<Beams> ().owner = playerNum;
				beam.GetComponent<Beams> ().dir = direction;
				beam.GetComponent<SpriteRenderer> ().color = defaultColor;
				startShotDelay ();
			}

		}

		//Putting up Shield
		if (Input.GetAxis ("RTrigger_P" + playerNum) == 1) 
		{
			readyToFuse = true;	
		} 
		else 
		{
			readyToFuse = false;
		}



	}
	public void jump (float jumpNum) 
	{
		rb.velocity = new Vector2 (rb.velocity.x, jumpNum); 
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		//Reset Jumps upon touching ground
		if (coll.gameObject.tag == "Floor") 
		{
			touchingGround = true;
		}

		if (coll.gameObject.tag == "Wall") 
		{
			touchingWall = true;
		}

		if (coll.gameObject.tag == "Player") 
		{
			SoloPlayers other = coll.gameObject.GetComponent<SoloPlayers> ();
			if (readyToFuse == true && other.readyToFuse == true) 
			{
				this.gameObject.SetActive (false);
				coll.gameObject.SetActive (false);
				GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
				gm.createFusion (transform.position);
			}
		}
	}


	void OnCollisionExit2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Floor") 
		{
			touchingGround = false; 
		}

		if (coll.gameObject.tag == "Wall")  
		{
			touchingWall = false;
		} 
	}

	public void handleShotDelay ()
	{
		if (canShoot == false) 
		{
			currentShotDelay++; 
		}

		if (currentShotDelay >= shotDelay) 
		{
			canShoot = true;
		}
	}

	public void startShotDelay ()
	{
		currentShotDelay = 0;
		canShoot = false;
	}

	void movementSmoothing ()
	{
		// Resets Air Actions when touching ground
		if (touchingGround) 
		{
			airActionsRemaining = maxAirActions;
		}

		// Makes sure players don't awkwardly cling to walls
		if (touchingWall == true && touchingGround == false) {
			if (directionModifier == 1) {
				stopRightMomentum = true;
			}

			if (directionModifier == -1) {
				stopLeftMomentum = true;
			}
		} else 
		{
			stopLeftMomentum = false;
			stopRightMomentum = false;
		}

		// Makes sure players can pass through platforms at the appropriate times.

		if (rb.velocity.y > 0 && touchingGround == false) 
		{
			passThroughPlatforms = true;
		}

		if (passThroughPlatforms) {
			if (playerNum == 1) {
				Physics2D.IgnoreLayerCollision (8, 11, true);
			}
			if (playerNum == 2) {
				Physics2D.IgnoreLayerCollision (9, 11, true);
			}
		} else {
			if (playerNum == 1) {
				Physics2D.IgnoreLayerCollision (8, 11, false);
			}
			if (playerNum == 2) {
				Physics2D.IgnoreLayerCollision (9, 11, false);
			}
		}
	}

}
