using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPConvo : QuestionInteractable {
	
	public bool bigDecision;
	public int IYHAppears;
	public string[] NPCLines;
	public string[] NPCAltLines;
	public string[] speakerIndex;

	public override void Start ()
	{
		base.Start ();
	}

	public override void Update ()
	{
		base.Update ();
	}

	public override void doInteraction(string input)
	{
		if (active) 
		{
			if (character.deciding == false) {
				if (currentLine <= dialougeLines.Length - 1) {
					if (questionLines [questionIndicator] == currentLine) {
						if (bigDecision && currentLine == IYHAppears) 
						{
							character.deciding = true;
							character.speaking = false;
							sendQuestion ();
							character.IYH.SetActive (true);
						} 
						else {
							character.deciding = true;
							character.speaking = false;
							sendQuestion ();
						}
					} else {
						sayLine (currentLine);
						beingInteractedWith = true;
						character.interacting = true;
					}
				} else {
					endInteraction ();
				}
			} else 
			{
				int director = 0;
				if (input == "A") 
				{
					director = AAnswers [questionIndicator];
				}
				if (input == "B") 
				{
					director = BAnswers [questionIndicator];
				}
				if (input == "X") 
				{
					director = XAnswers [questionIndicator];
				}
				if (input == "Y") 
				{
					director = YAnswers [questionIndicator];
				}
				answerQuestion (director);
				character.speaking = true;

			}
		}
	}

	public override void sayLine (int num)
	{
		NPC npc = GetComponent<NPC> ();
		if (speakerIndex [num] == "Player") 
		{
			character.speaking = true; 
			npc.speaking = false;
			character.currentlySaying = dialougeLines [num];
		}

		if (speakerIndex [num] == "NPC") 
		{
			character.speaking = false; 
			npc.speaking = true;
			npc.currentlySaying = NPCLines [num];
		}
		currentLine++;
	}

	public override void answerQuestion (int num)
	{
		NPC npc = GetComponent<NPC> ();
		npc.speaking = false;
		character.currentlySaying = answers[num];
		character.AOption = null;
		character.BOption = null;
		character.XOption = null;
		character.YOption = null;
		character.deciding = false;
		if (num == BAnswers [questionIndicator]) 
		{
			NPCLines [4] = NPCAltLines [4];
			NPCLines [5] = NPCAltLines [5];
		}
		currentLine++;
	}

	public override void sendQuestion ()
	{
		character.AOption = AOptions [questionIndicator];
		character.BOption = BOptions [questionIndicator]; 
		character.XOption = XOptions [questionIndicator];
		character.YOption = YOptions [questionIndicator];
		character.deciding = true;
	}

	public override void doThing ()
	{
		
	}

	public override void endInteraction ()
	{
		NPC npc = GetComponent<NPC> ();
		npc.speaking = false;
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
