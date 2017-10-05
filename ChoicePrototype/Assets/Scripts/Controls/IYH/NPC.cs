using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour {

	public GameObject textBox;
	public Text textBoxText;
	public bool speaking;
	public string currentlySaying;

	// Use this for initialization
	void Start () {

		textBoxText = textBox.GetComponentInChildren<Text> ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (speaking) {
			textBox.SetActive (true);
			textBoxText.text = currentlySaying;
		} else {
			textBox.SetActive (false);
		}
		
	}
}
