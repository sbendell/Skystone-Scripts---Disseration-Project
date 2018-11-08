using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmelterScript : MonoBehaviour {

	[SerializeField]
	public List<Transform> spawnLocs;

	private ItemDatabase database;
	public GameObject CounterWindow;
	public GameObject Counter;
	private PlayerScript player;
	public int stacksize;
	public bool IsGUIOpen;

	private List<Item> containedBars = new List<Item> ();

	private float timer = 0;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();
		database = GameObject.FindGameObjectWithTag ("Database").GetComponent<ItemDatabase> ();
	}

	public int ContainedBarsCount{
		get { return containedBars.Count; }
	}
	
	// Update is called once per frame
	void Update () {
		if (containedBars.Count != 0) {
			timer += Time.deltaTime;

			if (timer > 5f) {
				GetComponent<AudioSource> ().Play ();
				GetComponent<Animation> ().Play ();
				timer = 0;
			}
		}

		if (IsGUIOpen)
			Counter.GetComponent<Text> ().text = stacksize.ToString ();

		if (IsGUIOpen) {
			if (Input.GetKeyDown (KeyCode.W)) {
				if (stacksize < player.weapon.GetComponent<ItemData> ().stackSize) {
					stacksize++;
				}
			}

			if (Input.GetKeyDown (KeyCode.S)) {
				if (stacksize > 1) {
					stacksize--;
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

	public void CreateBars(){
		for (int i = 0; i < database.parts.Count; i++) {
			if (containedBars[0].itemName == "Iron Ore"){
				if (database.parts [i].GetComponent<ItemData> ().itemName == "Iron Bar") {
					int whatSpawn = 0;
					for (int x = 0; x < containedBars [0].stackSize; x++) {
						GameObject newBar = GameObject.Instantiate (database.parts [i], spawnLocs [whatSpawn].position, Quaternion.identity);
						whatSpawn++;

						if (whatSpawn > spawnLocs.Count - 1) {
							whatSpawn = 0;	
						}
					}
				}
			}

			if (containedBars[0].itemName == "Adamantium Ore"){
				if (database.parts [i].GetComponent<ItemData> ().itemName == "Adamantium Bar") {
					int whatSpawn = 0;
					for (int x = 0; x < containedBars [0].stackSize; x++) {
						GameObject newBar = GameObject.Instantiate (database.parts [i], spawnLocs [whatSpawn].position, Quaternion.identity);
						whatSpawn++;

						if (whatSpawn > spawnLocs.Count - 1) {
							whatSpawn = 0;	
						}
					}
				}
			}
		}

		containedBars.Clear ();
		timer = 0;
	}
}
