using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IYHSaver : MonoBehaviour {

	public GameObject player1;
	public GameObject player2;
	public GameObject fusionPlayer;
	public Vector3 player1Transform;
	public Vector3 player2Transform;
	SoloPlayers p1SP;
	SoloPlayers p2SP;


	// Use this for initialization
	void Start () 
	{

		p1SP = player1.GetComponent<SoloPlayers> ();
		p2SP = player2.GetComponent<SoloPlayers> ();
		player1Transform = player1.transform.localPosition;
		player2Transform = player2.transform.localPosition;
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void resetIYH ()
	{
		fusionPlayer.SetActive (false);
		player1.SetActive (true);
		player2.SetActive (true);
		player1.transform.localPosition = player1Transform;
		player2.transform.localPosition = player2Transform;
		p1SP.incapacitated = false;
		p2SP.incapacitated = false;
		p1SP.HP = p1SP.maxHP;
		p2SP.HP = p2SP.maxHP;
	}
}
