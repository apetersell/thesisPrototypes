using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusionDoor : MonoBehaviour {

	public float timerMax;
	public float currentTimer;
	bool fusionInDDoor;
	public Image i;
	Color lerpingColor;
	public float lerpSpeed;
	public Color player1Color;
	public Color player2Color;
	public Color nada;
	public GameObject IYH; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		 
		if (fusionInDDoor) {
			currentTimer--;
		} else 
		{
			currentTimer = timerMax;
		}

		i.fillAmount = currentTimer / timerMax; 

		if (currentTimer <= 0)
		{
			currentTimer = timerMax;
			GetComponent<IYHSaver> ().resetIYH ();
			IYH.SetActive(false); 
		}
			
		lerpingColor = Color.Lerp(player1Color, player2Color, Mathf.PingPong(Time.time*lerpSpeed, 1)); 
		GetComponent<SpriteRenderer> ().color = lerpingColor;
		if (currentTimer < timerMax) {
			i.color = lerpingColor;
		} else {
			i.color = nada;
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Fusion") 
		{
			fusionInDDoor = true;
		}
	}

	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Fusion") 
		{
			fusionInDDoor = false;
		}
	}
}
