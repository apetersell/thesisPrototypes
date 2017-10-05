using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData 
{
	public GameObject speaker;
	public string dialogue;

	public DialogueData ()
	{
		speaker = null;
		dialogue = "What are they sayin?";
	}

	public DialogueData (GameObject speaker, string dialogue)
	{
		this.speaker = speaker;
		this.dialogue = dialogue;
	}

}
