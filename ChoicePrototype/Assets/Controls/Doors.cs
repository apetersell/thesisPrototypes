using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour {

	public int playerNum;
	public Color p1;
	public Color p2;
	public Color fusion;
	SpriteRenderer sr;

	// Use this for initialization
	void Start () {

		sr = GetComponent<SpriteRenderer> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (playerNum == 1) 
		{
			sr.color = p1;
		}

		if (playerNum == 2) 
		{
			sr.color = p2;
		}

		if (playerNum == 3) 
		{
			sr.color = fusion;
		}

		
	}

//	void OnTriggerEnter2D(Collider2D coll) 
//	{
//		if (coll.gameObject.tag == "Beam") 
//		{
//			Beams b = coll.gameObject.GetComponent<Beams> ();
//
//			if (playerNum == b.owner) 
//			{
//				Destroy (this);
//				Destroy (coll.gameObject);
//			}
//		}
//
//	}
}
