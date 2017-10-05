using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	public GameObject fusion;
	IYHFusionPlayer fusionPlayer; 
	public GameObject p1;
	public GameObject p2;
	public RealWorldCharacter cat;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		fusionPlayer = fusion.GetComponent<IYHFusionPlayer> ();

		if (fusionPlayer.player1InControl) {
			cat.player1InControl = true;
		} else {
			cat.player1InControl = false;
		}

		if (fusionPlayer.player2InControl) {
			cat.player2InControl = true;
		} else {
			cat.player2InControl = false;
		}
	}

	public void createFusion (Vector3 pos)
	{
		p1.SetActive (false);
		p2.SetActive (false);
		if (fusion == null) {
			fusion = Instantiate (Resources.Load ("Prefabs/FusionTest/FusionPlayer")) as GameObject;
		} else 
		{
			fusion.SetActive (true);
		}
		fusion.transform.position = pos; 
		Camera.main.GetComponent<DynamicCamera> ().fused = true;
	}

	public void defuse (Vector3 p1Pos, Vector3 p2Pos)
	{
		fusion.GetComponent<IYHFusionPlayer> ().separationCountDown = fusion.GetComponent<IYHFusionPlayer> ().maxSeparationCountDown;
		fusion.SetActive (false);
//		Camera.main.GetComponent<DynamicCamera> ().fused = false;
		p1.SetActive (true);
		p1.transform.position = p1Pos;
		p2.SetActive (true);
		p2.transform.position = p2Pos;
		SoloPlayers p1SP = p1.GetComponent<SoloPlayers> ();
		SoloPlayers p2SP = p2.GetComponent<SoloPlayers> ();
		p1SP.actionable = true;
		p1SP.incapacitated = false;
		p1SP.HP = p1SP.maxHP;
		p2SP.actionable = true;
		p2SP.incapacitated = false;
		p2SP.HP = p1SP.maxHP;

	}

	public void IYHcreateFusion (Vector3 pos, string player)
	{
		p1.SetActive (false);
		p2.SetActive (false);
		if (fusion == null) {
			fusion = Instantiate (Resources.Load ("Prefabs/FusionTest/FusionPlayer")) as GameObject;
		} else 
		{
			fusion.SetActive (true);
		}
		fusion.transform.position = pos;
		Camera.main.GetComponent<DynamicCamera> ().fused = true;
		if (player == "Player_1")
		{
			fusion.GetComponent<IYHFusionPlayer> ().player1InControl = true;
			fusion.GetComponent<IYHFusionPlayer> ().player2InControl = false;
		}
		if (player == "Player_2")
		{
			fusion.GetComponent<IYHFusionPlayer> ().player1InControl = false;
			fusion.GetComponent<IYHFusionPlayer> ().player2InControl = true;
		}
		if (player == "Both")
		{
			fusion.GetComponent<IYHFusionPlayer> ().player1InControl = true;
			fusion.GetComponent<IYHFusionPlayer> ().player2InControl = true;
		}
	}
}
