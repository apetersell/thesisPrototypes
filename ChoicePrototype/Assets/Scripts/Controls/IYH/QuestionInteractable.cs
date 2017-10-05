using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionInteractable : SimpleInteractable {

	public string [] AOptions;
	public int[] AAnswers;
	public string[] BOptions;
	public int [] BAnswers;
	public string [] XOptions;
	public int [] XAnswers;
	public string [] YOptions;
	public int [] YAnswers;
	public string[] answers;
	public int [] questionLines;
	public int questionIndicator = 0;

	public override void doInteraction(string input)
	{
		if (active) 
		{
			if (character.deciding == false) {
				if (currentLine <= dialougeLines.Length - 1) {
					if (questionLines [questionIndicator] == currentLine) {
						character.deciding = true;
						character.speaking = false;
						sendQuestion ();
					} else {
						sayLine (currentLine);
						beingInteractedWith = true;
						character.interacting = true;
						character.speaking = true;
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

	void answerQuestion (int num)
	{
		character.currentlySaying = answers[num];
		character.AOption = null;
		character.BOption = null;
		character.XOption = null;
		character.YOption = null;
		character.deciding = false;
		currentLine++;
	}

	void sendQuestion ()
	{
		character.AOption = AOptions [questionIndicator];
		character.BOption = BOptions [questionIndicator]; 
		character.XOption = XOptions [questionIndicator];
		character.YOption = YOptions [questionIndicator];
		character.deciding = true;
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
		Destroy (this.gameObject);
	}
	
}
