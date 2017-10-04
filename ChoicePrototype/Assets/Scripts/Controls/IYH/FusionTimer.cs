using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusionTimer : MonoBehaviour {

	public IYHFusionPlayer fusion;
	Image i;
	public Color nada;
	public Color regularColor;

	// Use this for initialization
	void Start () {

		i = GetComponent<Image> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (fusion.gameObject.activeSelf == false || fusion.separationCountDown == fusion.maxSeparationCountDown) {
			i.color = nada;
		} else {
			i.color = regularColor;
		}

		i.fillAmount = fusion.separationCountDown / fusion.maxSeparationCountDown;
		
	}
}
