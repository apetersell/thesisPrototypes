using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

	public RealWorldCharacter character;
	public GameObject buttonIcon;
	public bool active = true;
	public bool beingInteractedWith;

	// Use this for initialization
	public virtual void Start () {

		buttonIcon = transform.GetChild (0).gameObject;
		
	}
	
	// Update is called once per frame
	public virtual void Update () { 

		if (active) 
		{
			if (character != null) 
			{
				if (beingInteractedWith) 
				{
					buttonIcon.SetActive (false);
				} 
				else 
				{
					buttonIcon.SetActive (true);
				}
			} 
			else 
			{
				buttonIcon.SetActive (false);
			}
		} 
		else 
		{
			buttonIcon.SetActive (false);

		}
		
	}

	public abstract void doInteraction (string input);

	public abstract void endInteraction ();
}
