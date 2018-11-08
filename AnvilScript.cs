using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnvilScript : MonoBehaviour {

	private ItemDatabase database;
	public GameObject AnvilWindow;
	public GameObject arrow;
	public Transform spawnLoc;
	private PlayerScript player;
	public int counter;
	public bool IsGUIOpen;
	bool waitAFrame;

	private RectTransform[] arrowLocations;
	private Text[] names;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();
		database = GameObject.FindGameObjectWithTag ("Database").GetComponent<ItemDatabase> ();
		arrowLocations = AnvilWindow.GetComponentsInChildren<RectTransform> ();
		names = AnvilWindow.GetComponentsInChildren<Text> ();
		waitAFrame = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsGUIOpen) {
			if (Input.GetKeyDown (KeyCode.S)) {
				if (counter < arrowLocations.Length - 2) {
					counter++;
				}
			}

			if (Input.GetKeyDown (KeyCode.W)) {
				if (counter > 1) {
					counter--;
				}
			}

			if (Input.GetButtonDown ("Action")) {
				if (waitAFrame == false) {
					if (player.waitOneFrame == false) {
						for (int i = 0; i < database.parts.Count; i++) {
							if (database.parts [i].GetComponent<ItemData> ().itemName == names [counter - 1].text) {
								GameObject newItem = Instantiate (database.parts [i], spawnLoc.position, Quaternion.identity);
							}
						}

						player.RemoveWeaponFromInventory (1);
						Destroy (player.weapon);
						counter = 1;
						player.IsGUIOpen = false;
						player.waitOneFrame = true;
						AnvilWindow.SetActive (false);
						IsGUIOpen = false;
					}
				}
			}

			if (Input.GetButtonDown ("Inventory")) {
				player.IsGUIOpen = false;
				AnvilWindow.SetActive (false);
				IsGUIOpen = false;
			}
		}

		if (IsGUIOpen) {
			arrow.GetComponent<RectTransform> ().anchoredPosition = arrowLocations [counter].anchoredPosition - new Vector2 (80, 0);
			waitAFrame = false;
		} else {
			waitAFrame = true;
		}
	}
}
