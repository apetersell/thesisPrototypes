using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	public Vector2 placement;
	public int owner; 
	public bool fusionIndicator;
	public Vector2 offset;
	SoloPlayers sp;
	FusionPlayer fp;
	SpriteRenderer sr;
	public Color p1Color;
	public Color p2Color;
	public Color fusionColor;
	public Color alphadOut;
	bool overlapping;
	bool noInput;
	Vector3 pointDirection;

	public bool keyBoard;
	string movementX;
	string movementY;

	Vector3 initPos;

	// Use this for initialization
	void Start () {
		initPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		altControl ();

		//detect whether input sth
		if (Input.GetAxis ("LeftStickX_P" + owner) == 0 && Input.GetAxis ("LeftStickY_P" + owner) == 0) {
			noInput = true;
		} else {
			noInput = false;
		}

		sp = GetComponentInParent <SoloPlayers> (); //two smaller ones
		fp = GetComponentInParent<FusionPlayer> ();//two player together
		sr = GetComponent<SpriteRenderer> ();

		Debug.Log(initPos);
		transform.localPosition = initPos + new Vector3(placement.x,placement.y,0);

		//if it is fusionIndicator, it shows the in-between arrow of both arrows
		if (fusionIndicator == false) 
		{
			//Debug.Log(movementX);
			//direction pointing at
			pointDirection = new Vector3 ((Input.GetAxis (movementX)), (Input.GetAxis (movementY)) * -1, 0); 
			//Debug.Log(pointDirection);
			if (pointDirection != Vector3.zero) 
			{
				//calc the angle it points at
				float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			}
			placement = new Vector2 (Input.GetAxis (movementX) + offset.x, (Input.GetAxis (movementY) * -1) + offset.y).normalized; 

			//solo player is not null
			if (sp != null) {
				if (sp.touchingGround == true) {
					if (placement.y <= 0) {
						transform.localPosition = new Vector2 (placement.x, 0);
					}
					if (placement.x == 0) 
					{
						placement.x = sp.directionModifier;
						placement.y = 0;
					}
				}
			}

			//fusion player is not null
			if (fp != null) {
				if (fp.touchingGround == true) {
					if (placement.y <= 0) {
						transform.localPosition = new Vector2 (placement.x, 0);
					}
				}
			}
		} 
		else 
		{
			placement = new Vector2 (fp.stickPosX, fp.stickPosY * -1);
			pointDirection = new Vector3 (fp.stickPosX, fp.stickPosY * -1, 0);

			if (pointDirection != Vector3.zero) {
				//Debug.Log("in");
				float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
				//transform.LookAt(transform.position+pointDirection);
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			}
		}

		//Debug.Log(transform.localRotation.eulerAngles.z);
		//try to flip it but not working now
		if (transform.localRotation.eulerAngles.z <= 180 && transform.localRotation.eulerAngles.z > 90) {
			Debug.Log("flip");
			Debug.Log(transform.localRotation.eulerAngles.z);
			GetComponent<SpriteRenderer> ().flipY = true;
		} else if(transform.localRotation.eulerAngles.z >= 0 && transform.localRotation.eulerAngles.z <= 90) {
			Debug.Log("unflip");
			GetComponent<SpriteRenderer> ().flipY = false;
		}

//		if (pointDirection == Vector3.zero) {
//			sr.color = alphadOut;
//		} else {
			if (overlapping) {
				sr.color = fusionColor;
			} else {
				if (owner == 1) {
					sr.color = p1Color;
				}
				if (owner == 2) {
					sr.color = p2Color;
				}
				if (fusionIndicator) {
					sr.color = fusionColor;
				}
			}
//		}

	}

	//detect overlap
	void OnTriggerEnter2D (Collider2D coll)
	{
		if (fp != null) 
		{
			if (coll.gameObject.tag == "Arrow" && noInput == false && coll.gameObject.GetComponent<Arrow>().noInput == false) 
			{
				overlapping = true;
				fp.overlapping = true;
			}
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (fp != null) 
		{
			if (coll.gameObject.tag == "Arrow") 
			{
				overlapping = false;
				fp.overlapping = false;
			}
		}
	}

	//controls change
	void altControl()
	{
		FusionPlayer fp = GetComponentInParent<FusionPlayer> ();
		if (fp.keyBoard) {
			keyBoard = true;
		} else {
			keyBoard = false;
		}
		if (keyBoard) {
			movementX = "Horizontal_P" +owner; 
			movementY = "Vertical_P" + owner;
		} else 
		{
			movementX = "LeftStickX_P" + owner;
			movementY = "LeftStickY_P" + owner;
		}

	}
}
