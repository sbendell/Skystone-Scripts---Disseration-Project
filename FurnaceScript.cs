using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceScript : MonoBehaviour {

	private ItemDatabase database;
	public GameObject CounterWindow;
	public GameObject Counter;
	public GameObject mould;
	public Transform spawnLoc;
	private PlayerScript player;
	public int stacksize;
	public bool IsGUIOpen;

	private List<Item> containedBars = new List<Item> ();

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();
		database = GameObject.FindGameObjectWithTag ("Database").GetComponent<ItemDatabase> ();
	}

	// Update is called once per frame
	void Update () {

		if (containedBars.Count != 0) {
			if (containedBars [0].itemName == "Iron Bar") {
				for (int i = 0; i < database.parts.Count; i++) {
					if (database.parts [i].GetComponent<ItemData> ().itemName == "Iron " + mould.GetComponent<MouldScript> ().CurrentMeshName) {
						GameObject newItem = GameObject.Instantiate (database.parts [i], spawnLoc.position, Quaternion.identity);

						if (containedBars [0].stackSize == 5) {
							newItem.GetComponent<ItemData> ().rarity = "Rare";
						} else {
							newItem.GetComponent<ItemData> ().rarity = "Common";
						}
					}
				}
			}
			if (containedBars [0].itemName == "Adamantium Bar") {
				for (int i = 0; i < database.parts.Count; i++) {
					if (database.parts [i].GetComponent<ItemData> ().itemName == "Adamantium " + mould.GetComponent<MouldScript> ().CurrentMeshName) {
						GameObject newItem = GameObject.Instantiate (database.parts [i], spawnLoc.position, Quaternion.identity);

						if (containedBars [0].stackSize == 5) {
							newItem.GetComponent<ItemData> ().rarity = "Rare";
						} else {
							newItem.GetComponent<ItemData> ().rarity = "Common";
						}
					}
				}
			}
			containedBars.Clear ();
		}

		if (IsGUIOpen)
			Counter.GetComponent<Text> ().text = stacksize.ToString ();

		if (IsGUIOpen) {
			if (Input.GetKeyDown (KeyCode.W)) {
				if (player.weapon.GetComponent<ItemData> ().stackSize >= 5) {
					stacksize = 5;
				}
			}

			if (Input.GetKeyDown (KeyCode.S)) {
				if (player.weapon.GetComponent<ItemData> ().stackSize >= 3) {
					stacksize = 3;
				}
			}

			if (Input.GetButtonDown ("Action")) {
				if (player.waitOneFrame == false) {
					ItemData data = player.weapon.GetComponent<ItemData> ();

					containedBars.Add (new Item (data.itemName, data.price, data.weight, data.rarity, data.itemType, stacksize));

					player.RemoveWeaponFromInventory (stacksize);
					Destroy (player.weapon);
					stacksize = 1;
					player.IsGUIOpen = false;
					player.waitOneFrame = true;
					CounterWindow.SetActive (false);
					IsGUIOpen = false;
				}
			}

			if (Input.GetButtonDown ("Inventory")) {
				player.IsGUIOpen = false;
				CounterWindow.SetActive (false);
				IsGUIOpen = false;
			}
		}
	}
}
