using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour {

	public List <string> surviors = new List<string> ();
	public List <string> eaten = new List <string>();
	public Text result;
	public Text health;
	public Text morals;
	public Text satisfy;
	public Text[] peopleAte;
	public Text[] peopleSaved;
	public bool casualMode;
	public string youLived;
	public string youDied;
	public string goodHealth;
	public string okayHealth;
	public string badHealth;
	public string goodMorals;
	public string okayMorals;
	public string badMorals;
	public string goodSatisfaction;
	public string okaySatisfaction;
	public string badSatisfaction;
	public Color good;
	public Color okay;
	public Color bad;
	public float maxHunger; 
	public float hunger;
	public float satisfaction;
	public float nutrition;
	public float morality;
	public GameObject endUI;
	public bool end = false;
	public float highEnd;
	public float lowEnd; 
	public Image meter;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (hunger > maxHunger) 
		{
			hunger = maxHunger; 
		}

		meter.fillAmount = hunger / maxHunger;

		if (casualMode == false && end == false) 
		{
			hunger = hunger - Time.deltaTime;
		}

		if (hunger <= 0) {
			end = true;
			GameObject player = GameObject.Find ("FusionPlayer");
			Destroy (player);
		}

		itsOver ();

		if (end) {
			endUI.SetActive (true);
			findSurviors ();
		}
			else {
			endUI.SetActive (false);
		}

	}

	public void madeChoice (float sentHunger, float sentSatisfaction, float sentNutrition, float sentMorality, string sentEaten, string saved)
	{
		hunger = hunger + sentHunger;
		satisfaction = satisfaction + sentSatisfaction;
		nutrition = nutrition + sentNutrition;
		morality = morality + sentMorality;
		eaten.Add (sentEaten);
		surviors.Add (saved);
	}

	public void itsOver()
	{
		if (hunger <= 0) {
			result.text = youDied;
			result.color = bad;
		} else {
			result.text = youLived;
			result.color = good;
		}

		//Heatlh
		if (nutrition >= highEnd) 
		{
			health.text = goodHealth;
			health.color = good;
		}
		if (nutrition < highEnd && nutrition > lowEnd) 
		{
			health.text = okayHealth;
			health.color = okay;
		}

		if (nutrition <= lowEnd) 
		{
			health.text = badHealth;
			health.color = bad;
		}

		//Satisfaction
		if (satisfaction >= highEnd) 
		{
			satisfy.text = goodSatisfaction;
			satisfy.color = good;
		}
		if (satisfaction < highEnd && satisfaction > lowEnd) 
		{
			satisfy.text = okaySatisfaction;
			satisfy.color = okay;
		}

		if (satisfaction <= lowEnd) 
		{
			satisfy.text = badSatisfaction;
			satisfy.color = bad;
		}

		//Morality
		if (morality >= highEnd) 
		{
			morals.text = goodMorals;
			morals.color = good;
		}
		if (morality < highEnd && morality > lowEnd) 
		{
			morals.text = okayMorals;
			morals.color = okay;
		}

		if (morality <= lowEnd) 
		{
			morals.text = badMorals;
			morals.color = bad;
		}

	}

	void findSurviors ()
	{
		for (int i = 0; i < eaten.Count; i++) {

			peopleAte [i].text = eaten [i];
			if (eaten [i] == null) {
				peopleAte [i].text = "";
			}
			
		}

		for (int i = 0; i < surviors.Count; i++) {

			peopleSaved [i].text = surviors [i];

		}
	}




}
