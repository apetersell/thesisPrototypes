using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	public GameObject fusion;
	public GameObject p1;
	public GameObject p2;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
		fusion.SetActive (false);
		fusion.GetComponent<FusionPlayer> ().separationCountDown = 60;
		Camera.main.GetComponent<DynamicCamera> ().fused = false;
		p1.SetActive (true);
		p1.transform.position = p1Pos;
		p2.SetActive (true);
		p2.transform.position = p2Pos;

	}
}
