using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IYHFusionPlayer : MonoBehaviour {

	SpriteRenderer sr;
	Rigidbody2D rb;

	public bool keyBoard;
	public int playerNum;
	public float HP;

	public Sprite p1Sprite;
	public Sprite p2Sprite;
	public Sprite harmonySprite;
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
	public float p1ShotDelay;
	public float p2ShotDelay;
	public bool overlapping;
	public bool p1TryingToUnfuse;
	public bool p2TryingToUnfuse;
	public float separationCountDown;
	public float maxSeparationCountDown;
	public Color player1Color;
	public Color player2Color;
	public Color fusionColor; 
	public Color p1GetOut; 
	public Color p2GetOut;
	public Color p1ToP2; 
	public float lerpSpeed;

	public bool readyToFuse; 
	public bool player1InControl;
	public bool player2InControl;

	string player1MovementX;
	string player2MovementX;
	string player1MovementY;
	string player2MovementY;
	public bool noInputP1;
	public bool noInputP2;

	float angle1;
	float angle2;
	bool sameSideOfZero;


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
		movementSmoothing ();  
		handleDualInputs (); 
		colorHandler (); 
		handleShotDelay ();

	}

	void actions ()
	{
		movement ();
		if (player1InControl) 
		{
			player1Inputs ();
		}
		if (player2InControl) 
		{
			player2Inputs ();
		}
		if (player1InControl && player2InControl) 
		{
			harmony ();
		}
	}

	void player1Inputs()
	{

		//Jumping
		if (Input.GetButtonDown ("AButton_P1")) 
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

		//Projectile
		if (Input.GetButtonDown ("BButton_P1"))  
		{
			if (p1CanShoot) {
				float modX = player1StickX;
				float modY = player1StickY * -1;
				GameObject beam = null; 
				if (modX == 0 && modY == 0) {
					modX = directionModifier;
				}
				if (modY < 0 && touchingGround == true) {
					modX = directionModifier;
					modY = 0;
				}
				Vector3 direction = new Vector3 (modX, modY, 0).normalized;
				bool harmony = false;
				if (player2InControl) {
					harmony = true;
				} else {
					harmony = false;
				}
				fireProjectile (1, modX, modY, direction, harmony);
			}
		}

		//Unfusing
		if (player2InControl == false) {
			if (Input.GetAxis ("RTrigger_P1") == 1) {
				p1TryingToUnfuse = true;
				if (Input.GetAxis ("RTrigger_P2") == 1) {
					p2TryingToUnfuse = true;
				}
			} else {
				p1TryingToUnfuse = false;
				p2TryingToUnfuse = false;
			}
		} 
		else 
		{
			if (Input.GetAxis ("RTrigger_P1") == 1) {
				p1TryingToUnfuse = true; 
			} else {
				p1TryingToUnfuse = false;
			}
		}
	}

	void player2Inputs ()
	{
		if (Input.GetButtonDown ("AButton_P2")) {
			if (touchingGround) {
				jump (jumpSpeed);
			} else if (touchingGround == false && player2airActions > 0) {
				jump (jumpSpeed * .85f); 
				player2airActions--;
			}
		}

		if (Input.GetButtonDown ("BButton_P2")) {
			if (p2CanShoot) {
				float modX = player2StickX;
				float modY = player2StickY * -1;
				if (modX == 0 && modY == 0) {
					modX = directionModifier;
				}
				if (modY < 0 && touchingGround == true) {
					modX = directionModifier;
					modY = 0;
				}
				Vector3 direction = new Vector3 (modX, modY, 0).normalized;
				bool harmony = false;
				if (player1InControl) {
					harmony = true;
				} else {
					harmony = false;
				}
				fireProjectile (2, modX, modY, direction, harmony);
			}

		}

		if (player1InControl == false) {
			if (Input.GetAxis ("RTrigger_P2") == 1) {
				p2TryingToUnfuse = true;
				if (Input.GetAxis ("RTrigger_P1") == 1) {
					p1TryingToUnfuse = true;
				}
			} else {
				p1TryingToUnfuse = false;
				p2TryingToUnfuse = false;
			}
		} 
		else 
		{
			if (Input.GetAxis ("RTrigger_P2") == 1) { 
				p2TryingToUnfuse = true; 
			} else {
				p2TryingToUnfuse = false;
			}
		}
	}

	void harmony ()
	{
		//Super Jump
		if (Input.GetButtonDown ("AButton_P1") && Input.GetButtonDown ("AButton_P2"))   
		{
			if (touchingGround) {
				jump (jumpSpeed * 1.5f);
			} 
		}

		float p1Angle = Mathf.Atan2(Input.GetAxis(player1MovementX), Input.GetAxis(player1MovementY)) * Mathf.Rad2Deg;
		float p2Angle = Mathf.Atan2(Input.GetAxis(player2MovementX), Input.GetAxis(player2MovementY)) * Mathf.Rad2Deg;
		float combindedAngle = Mathf.Abs (p1Angle) + Mathf.Abs (p2Angle); 
		angle1 = p1Angle;
		angle2 = p2Angle;
		if (p1Angle > 0 && p2Angle > 0) {
			sameSideOfZero = true;
		} else if (p1Angle < 0 && p2Angle < 0) {
			sameSideOfZero = true;
		} else if (p1Angle == 0 && p2Angle == 180) {
			sameSideOfZero = false;
		} else if (p1Angle == 180 && p2Angle == 0) {
			sameSideOfZero = false;
		} else {
			sameSideOfZero = false;
		}

		if (noInputP1 == false && noInputP2 == false) 
		{
			if (p1Angle != p2Angle) 
			{
				if (sameSideOfZero == false) 
				{
					if (combindedAngle > 130 && combindedAngle < 210) 
					{
						p1TryingToUnfuse = true;
						p2TryingToUnfuse = true;
					} 
					else 
					{
						if (Input.GetAxis ("RTrigger_P1") != 1) 
						{
							p1TryingToUnfuse = false;
						}
						if (Input.GetAxis ("RTrigger_P2") != 1) 
						{
							p2TryingToUnfuse = false;
						}
					}
				}
			}
		}
	}

	void movement ()
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
			
		//Unfusing

		if (p1TryingToUnfuse == true && p2TryingToUnfuse == true) {
			separationCountDown--;
		} else {
			separationCountDown = maxSeparationCountDown;
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
	}


	public void jump (float jumpNum) 
	{
		rb.velocity = new Vector2 (rb.velocity.x, jumpNum); 
	}

	public void fireProjectile (int owner, float modX, float modY,  Vector3 dir, bool harmony)
	{
		Color thisColor = Color.white;
		if (harmony) {
			thisColor = fusionColor;
		} else {
			if (owner == 1) {
				thisColor = player1Color;
			}
			if (owner == 2) {
				thisColor = player2Color;
			}
		}
		GameObject beam = null; 
		beam = Instantiate (Resources.Load ("Prefabs/Projectiles/SoloBeamPrefab")) as GameObject;
		beam.GetComponent<Beams> ().modPos = new Vector3 (modX, modY, 0).normalized;
		beam.transform.position = transform.position + beam.GetComponent<Beams> ().modPos;
		beam.transform.localScale = new Vector3 (
			beam.transform.localScale.x * 2,
			beam.transform.localScale.y * 2,
			beam.transform.localScale.z * 2);
		beam.GetComponent<Beams> ().owner = owner;
		beam.GetComponent<Beams> ().dir = dir;
		beam.GetComponent<SpriteRenderer> ().color = thisColor;  
		startShotDelay (owner);
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
		if (player1InControl) {
			player1StickX = Input.GetAxis (player1MovementX);
			player1StickY = Input.GetAxis (player1MovementY);
		} else {
			player1StickX = 0;
			player2StickY = 0;
		}
		if (player2InControl) {
			player2StickX = Input.GetAxis (player2MovementX); 
			player2StickY = Input.GetAxis (player2MovementY);
		} else {
			player2StickX = 0;
			player2StickY = 0;
		}
			
		moddedMoveSpeed = moveSpeed * moveSpeedModifier;


		stickPosX = (player1StickX + player2StickX) / 2; 
		stickPosY = (player1StickY + player2StickY) / 2;
		moveSpeedModifier = moveSpeedBonus * stickPosX;
		gravModifier = gravBonus * (stickPosY*2);
		if (gravModifier < 0) 
		{
			gravModifier = 0;
		}

		if (player1StickX == 0 && player1StickY == 0) {
			noInputP1 = true;
		} else {
			noInputP1 = false;
		}
		if (player2StickX == 0 && player2StickY == 0) {
			noInputP2 = true; 
		} else {
			noInputP2 = false;
		}

	}

	public void colorHandler()
	{
		if (p1TryingToUnfuse && p2TryingToUnfuse) {
			sr.color = p1ToP2;
		} else if (p1TryingToUnfuse && p2TryingToUnfuse == false) {
			sr.color = p1GetOut; 
		} else if (p1TryingToUnfuse == false && p2TryingToUnfuse) {
			sr.color = p2GetOut; 
		} else {
			if (player1InControl && player2InControl == false) {
				sr.color = player1Color;
				sr.sprite = p1Sprite;
			} else if (player2InControl && player1InControl == false) {
				sr.color = player2Color;
				sr.sprite = p2Sprite;
			} else {
				sr.color = fusionColor;
				sr.sprite = harmonySprite;
			}
		}
		p1GetOut = Color.Lerp(player1Color, fusionColor, Mathf.PingPong(Time.time*lerpSpeed, 1)); 
		p2GetOut = Color.Lerp (player2Color, fusionColor, Mathf.PingPong (Time.time * lerpSpeed, 1));  
		p1ToP2 = Color.Lerp (player1Color, player2Color, Mathf.PingPong (Time.time * lerpSpeed, 1)); 
	}


}
