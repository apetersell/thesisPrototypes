using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionPlayer : MonoBehaviour {

	SpriteRenderer sr;
	Rigidbody2D rb;

	public bool keyBoard;
	public int playerNum;
	public float HP;


	public float stickPlacement;
	public float player1StickX;
	public float player1StickY; 
	public float player2StickX;
	public float player2StickY;
	public float stickPosX;
	public float stickPosY;

	public float moveSpeedBonus; 
	public float moveSpeedModifier;
	public float moddedMoveSpeed;


	public float moveSpeed;
	public float airSpeedModifier;
	public float jumpSpeed;
	public float normalGrav;
	public float gravBonus;
	public float gravModifier; 
	public float fastFallGrav;
	public float gravityThreshold;
	public int directionModifier;
	public int player1maxAirActions;
	public int player2maxAirActions;
	public int player1airActions;
	public int player2airActions;
	public bool touchingGround; 
	public bool actionable = true; 
	public bool affectedByGrav = true; 
	public bool touchingWall= false; 
	public bool stopRightMomentum = false; 
	public bool stopLeftMomentum = false; 
	public bool passThroughPlatforms; 
	public bool p1CanShoot = true; 
	public bool p2CanShoot = true;
	public bool fusionCanShoot = true; 
	public float p1CurrentShotDelay; 
	public float p2CurrentShotDelay;
	public float fusionCurrenShotDelay; 
	public float p1ShotDelay;
	public float p2ShotDelay;
	public float fusionShotDelay;
	public bool overlapping;
	public bool p1TryingToUnfuse;
	public bool p2TryingToUnfuse;
	public float separationCountDown;
	public Color player1Color;
	public Color player2Color;
	public Color defaultColor; 
	public Color blueLerp;
	public Color redLerp;
	public Color blueToRed;
	public float lerpSpeed;

	public bool readyToFuse; 

	string player1MovementX;
	string player2MovementX;
	string player1MovementY;
	string player2MovementY;


	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();

	}

	// Update is called once per frame
	void Update () {

		if (keyBoard) {
			player1MovementX = "Horizontal_P1";
			player1MovementY = "Vertical_P1";
			player2MovementX = "Horizontal_P2";
			player2MovementY = "Vertical_P2";
		} else 
		{
			player1MovementX = "LeftStickX_P1";
			player1MovementY = "LeftStickY_P1";
			player2MovementX = "LeftStickX_P2"; 
			player2MovementY = "LeftStickY_P2";
		}

		if (actionable) 
		{
			actions ();
		}
//
		movementSmoothing (); 
		handleShotDelay (); 
		handleDualInputs ();
		colorHandler ();

		if (readyToFuse) {
			sr.color = blueToRed;
		} else {
			sr.color = defaultColor;
		}

		blueLerp = Color.Lerp(player1Color, defaultColor, Mathf.PingPong(Time.time*lerpSpeed, 1));
		redLerp = Color.Lerp (player2Color, defaultColor, Mathf.PingPong (Time.time * lerpSpeed, 1));
		blueToRed = Color.Lerp (player1Color, player2Color, Mathf.PingPong (Time.time * lerpSpeed, 1));

	}

	void actions ()
	{
		//	Left and right movement.
		if (stickPosX != 0 && stopRightMomentum == false) 
		{
			if (touchingGround) {
				rb.velocity = new Vector2 (moddedMoveSpeed, rb.velocity.y);
			} 
			else 
			{
				rb.velocity = new Vector2 (moddedMoveSpeed * airSpeedModifier, rb.velocity.y);
			}

			if (moddedMoveSpeed > 0) 
			{
				sr.flipX = false;
				directionModifier = 1;
			}
			if (moddedMoveSpeed < 0) 
			{
				sr.flipX = true;
				directionModifier = -1;
			}
		}

		if (stickPosX == 0)
		{
			rb.velocity = new Vector2 (0, rb.velocity.y);
		}

		//Jumping
		if (Input.GetAxis ("LeftStickY_P1") == -1) 
		{
			if (touchingGround) 
			{
				jump (jumpSpeed);
			} 
			else if (touchingGround == false && player1airActions > 0) 
			{
				jump (jumpSpeed * .85f);
				player1airActions--;
			}
		}

		if (Input.GetAxis ("LeftStickY_P2") == -1) 
		{
			if (touchingGround) 
			{
				jump (jumpSpeed);
			} 
			else if (touchingGround == false && player2airActions > 0) 
			{
				jump (jumpSpeed * .85f);
				player2airActions--;
			}
		}

		if (Input.GetAxis ("LeftStickY_P1") == -1 && Input.GetAxis ("LeftStickY_P2") == -1)   
		{
			if (touchingGround) {
				jump (jumpSpeed * 1.5f);
			} 
		}

			
//
		//FastFalling/Fallling through soft platforms
		if (affectedByGrav) 
		{
			if ((player1StickY > gravityThreshold) && (player2StickY > gravityThreshold)) 
			{
				rb.gravityScale = normalGrav * gravModifier;
				passThroughPlatforms = true;
			} else {
				rb.gravityScale = normalGrav;
				passThroughPlatforms = false;
			}
		}

		//Projectile Attack
		if (overlapping == false) {
			if (Input.GetAxis ("RTrigger_P1") == 1)  {
				if (p1CanShoot) {
					float modX = player1StickX;
					float modY = player1StickY * -1;
					GameObject beam = null; 
					if (modX == 0 && modY == 0) {
						modX = directionModifier;
					}
//					if (modY < 0 && touchingGround == true) {
//						modX = directionModifier;
//						modY = 0;
//					}
					Vector3 direction = new Vector3 (modX, modY, 0).normalized;
					beam = Instantiate (Resources.Load ("Prefabs/Projectiles/SoloBeamPrefab")) as GameObject;
					beam.GetComponent<Beams> ().modPos = new Vector3 (modX, modY, 0).normalized;
					beam.transform.position = transform.position + beam.GetComponent<Beams> ().modPos;
					beam.GetComponent<Beams> ().owner = 1;
					beam.GetComponent<Beams> ().dir = direction;
					beam.GetComponent<SpriteRenderer> ().color = player1Color;
					startShotDelay (1);
				}

			}

			if (Input.GetAxis ("RTrigger_P2") == 1)  {
				if (p2CanShoot) {
					float modX = player2StickX;
					float modY = player2StickY * -1;
					GameObject beam = null; 
					if (modX == 0 && modY == 0) {
						modX = directionModifier;
					}
//					if (modY < 0 && touchingGround == true) {
//						modX = directionModifier;
//						modY = 0;
//					}
					Vector3 direction = new Vector3 (modX, modY, 0).normalized;
					beam = Instantiate (Resources.Load ("Prefabs/Projectiles/SoloBeamPrefab")) as GameObject;
					beam.GetComponent<Beams> ().modPos = new Vector3 (modX, modY, 0).normalized;
					beam.transform.position = transform.position + beam.GetComponent<Beams> ().modPos;
					beam.GetComponent<Beams> ().owner = 2;
					beam.GetComponent<Beams> ().dir = direction;
					beam.GetComponent<SpriteRenderer> ().color = player2Color;
					startShotDelay (2);
				}

			}
		} else {
			if (Input.GetAxis ("RTrigger_P1") == 1 || (Input.GetAxis ("RTrigger_P2") == 1)) 
			{
				if (fusionCanShoot) 
				{
					float modX = stickPosX;
					float modY = stickPosY * -1;
					GameObject beam = null; 
					if (modX == 0 && modY == 0) {
						modX = directionModifier;
					}
//					if (modY < 0 && touchingGround == true) {
//						modX = directionModifier;
//						modY = 0;
//					}
					Vector3 direction = new Vector3 (modX, modY, 0).normalized;
					beam = Instantiate (Resources.Load ("Prefabs/Projectiles/SoloBeamPrefab")) as GameObject;
					beam.GetComponent<Beams> ().modPos = new Vector3 (modX, modY, 0).normalized;
					beam.transform.position = transform.position + beam.GetComponent<Beams> ().modPos;
					beam.GetComponent<Beams> ().owner = 3;
					beam.GetComponent<Beams> ().dir = direction;
					beam.GetComponent<SpriteRenderer> ().color = defaultColor;
					beam.transform.localScale = beam.transform.localScale * 2;
					startShotDelay (3);
				}
			}
		}

		//Unfusing
		if (Input.GetAxis ("RTrigger_P1") == 1) 
		{
			p1TryingToUnfuse = true;	
		} 
		else 
		{
			p1TryingToUnfuse = false;
		}

		if (Input.GetAxis ("RTrigger_P2") == 1) 
		{
			p2TryingToUnfuse = true;	
		} 
		else 
		{
			p2TryingToUnfuse = false;
		}

		if (p1TryingToUnfuse == true && p2TryingToUnfuse == true) {
			separationCountDown--;
		} else {
			separationCountDown = 60f;
		}

		if (separationCountDown <= 0) 
		{
			GameManager gm = GameObject.Find ("GameManager").GetComponent<GameManager> ();
			if (player1StickX > player2StickX)
			{
				Vector3 p1Pos = new Vector3 (transform.position.x + 1.75f, transform.position.y, transform.position.z);
				Vector3 p2Pos = new Vector3 (transform.position.x - 1.75f, transform.position.y, transform.position.z);
				gm.defuse (p1Pos, p2Pos);
			}
			else
			{
				Vector3 p1Pos = new Vector3 (transform.position.x - 1.75f, transform.position.y, transform.position.z);
				Vector3 p2Pos = new Vector3 (transform.position.x + 1.75f, transform.position.y, transform.position.z);
				gm.defuse (p1Pos, p2Pos);
			}
		}


//
//		//Putting up Shield
//		if (Input.GetAxis ("RTrigger_P" + playerNum) == 1) 
//		{
//			readyToFuse = true;	
//		} 
//		else 
//		{
//			readyToFuse = false;
//		}
//
//
//
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

		if (coll.gameObject.tag == "Cover") 
		{
			coll.gameObject.GetComponent<CoverAttributes> ().killCover ();
		}

		if (coll.gameObject.name == "EndyBoxy") 
		{
			GameObject.Find ("ChoiceManager").GetComponent<ChoiceManager> ().end = true;
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
		if (p1CanShoot == false) 
		{
			p1CurrentShotDelay++; 
		}

		if (p1CurrentShotDelay >= p1ShotDelay) 
		{
			p1CanShoot = true;
		}

		if (p2CanShoot == false) 
		{
			p2CurrentShotDelay++; 
		}

		if (p2CurrentShotDelay >= p2ShotDelay) 
		{
			p2CanShoot = true;
		}

		if (fusionCanShoot == false) 
		{
			fusionCurrenShotDelay++;
		}

		if (fusionCurrenShotDelay >= fusionShotDelay)
		{
			fusionCanShoot = true;
		}
	}
		

	public void startShotDelay (int sent)
	{
		if (sent == 1) 
		{
			p1CurrentShotDelay = 0;
			p1CanShoot = false;
		}
		if (sent == 2) 
		{
			p2CurrentShotDelay = 0;
			p2CanShoot = false;
		}

		if (sent == 3) 
		{
			fusionCurrenShotDelay = 0;
			fusionCanShoot = false; 
		}
	}

	void movementSmoothing ()
	{
		// Resets Air Actions when touching ground
		if (touchingGround) 
		{
			player1airActions = player1maxAirActions;
			player2airActions = player2maxAirActions;
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

//		if (rb.velocity.y > 0 && touchingGround == false) 
//		{
//			passThroughPlatforms = true;
//		}

		if (passThroughPlatforms) 
		{
			Physics2D.IgnoreLayerCollision (11, 13, true);
		}
		else 
		{
			Physics2D.IgnoreLayerCollision (11, 13, false);
		}
	}

	void handleDualInputs()
	{

		player1StickX = Input.GetAxis (player1MovementX);
		player1StickY = Input.GetAxis (player1MovementY);
		player2StickX = Input.GetAxis (player2MovementX); 
		player2StickY = Input.GetAxis (player2MovementY);

		moddedMoveSpeed = moveSpeed * moveSpeedModifier;


		stickPosX = (player1StickX + player2StickX) / 2; 
		stickPosY = (player1StickY + player2StickY) / 2;
		moveSpeedModifier = moveSpeedBonus * stickPosX;
		gravModifier = gravBonus * (stickPosY*2);
		if (gravModifier < 0) 
		{
			gravModifier = 0;
		}

	}

	public void colorHandler()
	{
		if (p1TryingToUnfuse == true && p2TryingToUnfuse == false) {
			sr.color = blueLerp;
		} 

	}

}

