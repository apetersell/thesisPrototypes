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


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetAxis ("LeftStickX_P" + owner) == 0 && Input.GetAxis ("LeftStickY_P" + owner) == 0) {
			noInput = true;
		} else {
			noInput = false;
		}

		sp = GetComponentInParent <SoloPlayers> (); 
		fp = GetComponentInParent<FusionPlayer> ();
		sr = GetComponent<SpriteRenderer> ();

		transform.localPosition = placement;

		if (fusionIndicator == false) 
		{
			pointDirection = new Vector3 ((Input.GetAxis ("LeftStickX_P" + owner)), (Input.GetAxis ("LeftStickY_P" + owner)) * -1, 0);  
			if (pointDirection != Vector3.zero) 
			{
				float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			}
			placement = new Vector2 (Input.GetAxis ("LeftStickX_P" + owner) + offset.x, (Input.GetAxis ("LeftStickY_P" + owner) * -1) + offset.y).normalized; 

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
			if (pointDirection != Vector3.zero) 
			{
				float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			}
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
}
