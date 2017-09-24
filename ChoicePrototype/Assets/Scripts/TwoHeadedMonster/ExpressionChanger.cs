using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionChanger : MonoBehaviour {

	public Sprite[] expressions;
	public float maxExpressionTime;
	public float currentExpressionTime;
	public int playerNum;
	public bool emoting;
	SpriteRenderer sr;

	// Use this for initialization
	void Start () {

		playerNum = GetComponent<Arrow> ().owner;
		sr = GetComponent<SpriteRenderer> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		handleEmote ();

		if (Input.GetButtonDown ("AButton_P" + playerNum)) 
		{
			changeExpression (1);
		}
		if (Input.GetButtonDown ("BButton_P" + playerNum)) 
		{
			changeExpression (2);
		}
		if (Input.GetButtonDown ("XButton_P" + playerNum)) 
		{
			changeExpression (3);
		}
		if (Input.GetButtonDown ("YButton_P" + playerNum)) 
		{
			changeExpression (4);
		}
		
	}

	void changeExpression (int sent)
	{
		sr.sprite = expressions [sent];
		emoting = true;
		currentExpressionTime = maxExpressionTime;
	}

	void handleEmote ()
	{
		if (emoting) 
		{
			currentExpressionTime--;
		}

		if (currentExpressionTime <= 0) 
		{
			emoting = false;
			sr.sprite = expressions [0];
		}
	}
}
