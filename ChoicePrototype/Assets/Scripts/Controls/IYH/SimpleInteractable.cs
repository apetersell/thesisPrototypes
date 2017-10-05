using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInteractable : Interactable { 

	public string [] dialougeLines;
	public int currentLine;
	public bool onlyOnce;  

	// Update is called once per frame
	public override void Update () {
		base.Update ();

	}

	public override void doInteraction (string input)
	{
		if (active) 
		{
			if (currentLine <= dialougeLines.Length - 1) 
			{
				sayLine (currentLine);
				beingInteractedWith = true;
				character.interacting = true;
				character.speaking = true;
			} 
			else 
			{
				endInteraction ();
			}
		}
	}

	public virtual void sayLine (int num)
	{
		character.currentlySaying = dialougeLines [num];
		currentLine++;
	}

	public override void endInteraction ()
	{
		beingInteractedWith = false;
		character.interacting = false;
		character.speaking = false;
		character.currentlySaying = null;
		if (onlyOnce) {
			active = false;
		}
		else 
		{
			currentLine = 0;
		}
	}
}