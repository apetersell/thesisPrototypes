using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	Image i;
	public SoloPlayers sp;
	public Color regColor;
	public Color Nada;


	// Use this for initialization
	void Start () {

		i = GetComponent<Image> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		i.fillAmount = sp.HP / sp.maxHP;
		if (sp.HP >= sp.maxHP) 
		{
			i.color = Nada;
		} else 
		{
			i.color = regColor;
		}
		
	}
}
