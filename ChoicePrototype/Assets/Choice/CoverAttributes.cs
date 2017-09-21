using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverAttributes : MonoBehaviour {

	public string survivor;
	public string dead;
	public float satisfaction; 
	public float hunger;
	public float nutrition;
	public float morality;
	public GameObject otherCollier;
	public ChoiceManager cm;


	// Use this for initialization
	void Start () {

		cm = GameObject.Find ("ChoiceManager").GetComponent<ChoiceManager> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void killCover ()
	{
		cm.madeChoice (hunger, satisfaction, nutrition, morality, dead, survivor);
		Destroy (this.gameObject);
		if (otherCollier != null) 
		{
			Destroy (otherCollier);
		}
	}
}
