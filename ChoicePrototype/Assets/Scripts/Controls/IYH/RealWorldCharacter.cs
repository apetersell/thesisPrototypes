using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealWorldCharacter : MonoBehaviour {

	SpriteRenderer sr;
	Rigidbody2D rb;

	public bool keyBoard;

	public float stickPlacement;
	public float player1StickX;
	public float player1StickY; 
	public float player2StickX;
	public float player2StickY;
	public float stickPosX;
	public float stickPosY;

	public float moveSpeed;
	public float moveSpeedBonus; 
	public float moveSpeedModifier;
	public float moddedMoveSpeed;
	public int directionModifier;
	public bool touchingGround; 
	public bool actionable = true; 

	public Color player1Color;
	public Color player2Color;
	public Color fusionColor; 
	public float lerpSpeed;

	public bool player1InControl;
	public bool player2InControl;

	string player1MovementX;
	string player2MovementX;
	string player1MovementY;
	string player2MovementY;
	public bool noInputP1;
	public bool noInputP2;

	public Interactable seeing;
	public string currentlySaying;
	public bool interacting;
	public bool speaking;  
	public bool deciding;  
	public string AOption;
	public string BOption;
	public string XOption;
	public string YOption;
	public GameObject myCanvas; 
	public GameObject textBox;
	public GameObject A;
	public GameObject B; 
	public GameObject X;
	public GameObject Y; 
	public Text textBoxText;
	Text AText;
	Text BText;
	Text XText;
	Text YText;
	 

	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer> ();
		rb = GetComponent<Rigidbody2D> ();
		textBoxText = textBox.GetComponentInChildren<Text> ();
		AText = A.GetComponentInChildren<Text> ();
		BText = B.GetComponentInChildren<Text> ();
		XText = X.GetComponentInChildren<Text> ();
		YText = Y.GetComponentInChildren<Text> ();
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
		handleDualInputs ();  
		colorHandler (); 
		interactionHandler ();
	}

	void actions ()
	{
		if (interacting == false) 
		{
			movement (); 
		}
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

	void movement ()
	{
		//	Left and right movement.
		if (stickPosX != 0) 
		{
			if (touchingGround) {
				rb.velocity = new Vector2 (moddedMoveSpeed, rb.velocity.y); 
			} 
//			else 
//			{
//				rb.velocity = new Vector2 (moddedMoveSpeed * airSpeedModifier, rb.velocity.y);
//			}

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
	}

	void player1Inputs ()
	{
		if (Input.GetButtonDown ("AButton_P1")) 
		{
			if (seeing != null) 
			{
				seeing.doInteraction ("A");
			}
		}
		if (Input.GetButtonDown ("BButton_P1")) 
		{
			if (seeing != null) 
			{
				seeing.doInteraction ("B");
			}
		}
		if (Input.GetButtonDown ("XButton_P1")) 
		{
			if (seeing != null) 
			{
				seeing.doInteraction ("X");
			}
		}
		if (Input.GetButtonDown ("YButton_P1")) 
		{
			if (seeing != null) 
			{
				seeing.doInteraction ("Y");
			}
		}
	}

	void player2Inputs ()
	{
		if (Input.GetButtonDown ("AButton_P2")) 
		{
			if (seeing != null) 
			{
				seeing.doInteraction ("A");
			}
		}
		if (Input.GetButtonDown ("BButton_P2")) 
		{
			if (seeing != null) 
			{
				seeing.doInteraction ("B");
			}
		}
		if (Input.GetButtonDown ("XButton_P2")) 
		{
			if (seeing != null) 
			{
				seeing.doInteraction ("X");
			}
		}
		if (Input.GetButtonDown ("YButton_P2")) 
		{
			if (seeing != null) 
			{
				seeing.doInteraction ("Y");
			}
		}
	}

	void harmony ()
	{

	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		Interactable i = coll.gameObject.GetComponent<Interactable> ();
		if (i != null) 
		{
			if (seeing == null) 
			{
				seeing = i;
				i.character = this;
			}
		}

	}

	void OnTriggerExit2D (Collider2D coll)
	{
		Interactable i = coll.gameObject.GetComponent<Interactable> ();
		if (i != null) 
		{
			if (seeing == i) 
			{
				seeing = null;
				i.character = null;
			}
		}

	}


	void handleDualInputs ()
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

	void colorHandler ()
	{
		Color p1Lerp = Color.Lerp(Color.white, player1Color, Mathf.PingPong(Time.time*lerpSpeed, 1));
		Color p2Lerp = Color.Lerp (Color.white, player2Color, Mathf.PingPong (Time.time * lerpSpeed, 1));
		Color fusionLerp = Color.Lerp (Color.white, fusionColor, Mathf.PingPong (Time.time * lerpSpeed, 1));
		if (player1InControl && player2InControl == false) 
		{
			sr.color = p1Lerp;
		}
		if (player1InControl == false && player2InControl) 
		{
			sr.color = p2Lerp;
		}
		if (player1InControl && player2InControl) 
		{
			sr.color = fusionLerp;
		}
	}

	void interactionHandler ()
	{
		if (interacting)
		{
			myCanvas.SetActive (true);

		} 
		else 
		{
			myCanvas.SetActive (false);
		}
		if (speaking) {
			textBox.SetActive (true);
			textBoxText.text = currentlySaying;
		} else {
			textBox.SetActive (false);
		}
		if (deciding) {
			if (AOption != "") {
				A.SetActive (true);
				AText.text = AOption;
			} else {
				A.SetActive (false);
			}
			if (BOption != "") {
				B.SetActive (true);
				BText.text = BOption;
			} else {
				B.SetActive (false);
			}
			if (XOption != "") {
				X.SetActive (true);
				XText.text = XOption;
			} else {
				X.SetActive (false);
			}
			if (YOption != "") {
				Y.SetActive (true);
				YText.text = YOption;
			}
			else{
				Y.SetActive(false);
			}
		} 
		else 
		{
			A.SetActive (false);
			B.SetActive (false);
			X.SetActive (false);
			Y.SetActive (false);
		}
	}
}
