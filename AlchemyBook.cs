using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyBook : MonoBehaviour {

	public AlchemyStation station;
	private PlayerScript player;
	public GameObject healthPotionUI;
	public GameObject manaPotionUI;
	public GameObject staminaPotionUI;
	public GameObject damagePotionUI;
	public GameObject armorPotionUI;

	List<Combination> healthPotionCombinations = new List<Combination> ();
	List<Combination> manaPotionCombinations = new List<Combination> ();
	List<Combination> staminaPotionCombinations = new List<Combination> ();
	List<Combination> damagePotionCombinations = new List<Combination> ();
	List<Combination> armorPotionCombinations = new List<Combination> ();

	Text[] healthPotionTextList;
	Text[] manaPotionTextList;
	Text[] staminaPotionTextList;
	Text[] damagePotionTextList;
	Text[] armorPotionTextList;

	// Use this for initialization
	void Start () {
		station = GameObject.FindGameObjectWithTag ("Alchemy Station").GetComponent<AlchemyStation> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();
		healthPotionTextList = healthPotionUI.GetComponentsInChildren<Text> ();
		manaPotionTextList = manaPotionUI.GetComponentsInChildren<Text> ();
		staminaPotionTextList = staminaPotionUI.GetComponentsInChildren<Text> ();
		damagePotionTextList = damagePotionUI.GetComponentsInChildren<Text> ();
		armorPotionTextList = armorPotionUI.GetComponentsInChildren<Text> ();

		for (int i = 0; i < station.CombinationTable.Count; i++) {
			if (station.CombinationTable [i].outcome == "Health") {
				healthPotionCombinations.Add (station.CombinationTable [i]);
			}
		}

		for (int i = 0; i < station.CombinationTable.Count; i++) {
			if (station.CombinationTable [i].outcome == "Mana") {
				manaPotionCombinations.Add (station.CombinationTable [i]);
			}
		}

		for (int i = 0; i < station.CombinationTable.Count; i++) {
			if (station.CombinationTable [i].outcome == "Stamina") {
				staminaPotionCombinations.Add (station.CombinationTable [i]);
			}
		}

		for (int i = 0; i < station.CombinationTable.Count; i++) {
			if (station.CombinationTable [i].outcome == "Damage") {
				damagePotionCombinations.Add (station.CombinationTable [i]);
			}
		}

		for (int i = 0; i < station.CombinationTable.Count; i++) {
			if (station.CombinationTable [i].outcome == "Armor") {
				armorPotionCombinations.Add (station.CombinationTable [i]);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < healthPotionCombinations.Count; i++) {
			if (healthPotionCombinations [i].discovered) {
				healthPotionTextList [i + 1].text = healthPotionCombinations [i].flowerOne + " + " + healthPotionCombinations [i].flowerTwo;
			} else {
				healthPotionTextList [i + 1].text = "Undiscovered";
			}
		}
		for (int i = 0; i < manaPotionCombinations.Count; i++) {
			if (manaPotionCombinations [i].discovered) {
				manaPotionTextList [i + 1].text = manaPotionCombinations [i].flowerOne + " + " + manaPotionCombinations [i].flowerTwo;
			} else {
				manaPotionTextList [i + 1].text = "Undiscovered";
			}
		}
		for (int i = 0; i < staminaPotionCombinations.Count; i++) {
			if (staminaPotionCombinations [i].discovered) {
				staminaPotionTextList [i + 1].text = staminaPotionCombinations [i].flowerOne + " + " + staminaPotionCombinations [i].flowerTwo;
			} else {
				staminaPotionTextList [i + 1].text = "Undiscovered";
			}
		}
		for (int i = 0; i < damagePotionCombinations.Count; i++) {
			if (damagePotionCombinations [i].discovered) {
				damagePotionTextList [i + 1].text = damagePotionCombinations [i].flowerOne + " + " + damagePotionCombinations [i].flowerTwo;
			} else {
				damagePotionTextList [i + 1].text = "Undiscovered";
			}
		}
		for (int i = 0; i < armorPotionCombinations.Count; i++) {
			if (armorPotionCombinations [i].discovered) {
				armorPotionTextList [i + 1].text = armorPotionCombinations [i].flowerOne + " + " + armorPotionCombinations [i].flowerTwo;
			} else {
				armorPotionTextList [i + 1].text = "Undiscovered";
			}
		}
		foreach (var item in armorPotionTextList) {
			Debug.Log (item.name);
		}
		foreach (var item in damagePotionTextList) {
			Debug.Log (item.name);
		}

		if (Input.GetButtonDown ("Inventory")) {
			player.IsGUIOpen = false;
			this.gameObject.SetActive (false);
		}
	}
}
