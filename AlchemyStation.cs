using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyStation : MonoBehaviour {
	
	public List<Combination> CombinationTable = new List<Combination> ();
	public List<Item> containedHerbs = new List<Item> ();
	PlayerScript player;
	ItemDatabase database;
	public GameObject CounterWindow;
	public GameObject Counter;
	public int stacksize;
	private float timer;
	public bool IsGUIOpen = false;

	// Use this for initialization
	void Start () {
		CombinationTable.Add (new Combination ("Amaryllis", "Bell Flower", "Health"));
		CombinationTable.Add (new Combination ("Amaryllis", "Daffodil", "Mana"));
		CombinationTable.Add (new Combination ("Amaryllis", "Foxglove", "Stamina"));
		CombinationTable.Add (new Combination ("Amaryllis", "Goldenrod", "Damage"));
		CombinationTable.Add (new Combination ("Amaryllis", "Hyacinth", "Armor"));
		CombinationTable.Add (new Combination ("Amaryllis", "Lavender", "Health"));
		CombinationTable.Add (new Combination ("Amaryllis", "Lily", "Mana"));
		CombinationTable.Add (new Combination ("Amaryllis", "Throatwort", "Stamina"));
		CombinationTable.Add (new Combination ("Amaryllis", "Tulip", "Damage"));

		CombinationTable.Add (new Combination ("Bell Flower", "Daffodil", "Armor"));
		CombinationTable.Add (new Combination ("Bell Flower", "Foxglove", "Health"));
		CombinationTable.Add (new Combination ("Bell Flower", "Goldenrod", "Mana"));
		CombinationTable.Add (new Combination ("Bell Flower", "Hyacinth", "Stamina"));
		CombinationTable.Add (new Combination ("Bell Flower", "Lavender", "Damage"));
		CombinationTable.Add (new Combination ("Bell Flower", "Lily", "Armor"));
		CombinationTable.Add (new Combination ("Bell Flower", "Throatwort", "Health"));
		CombinationTable.Add (new Combination ("Bell Flower", "Tulip", "Mana"));

		CombinationTable.Add (new Combination ("Daffodil", "Foxglove", "Stamina"));
		CombinationTable.Add (new Combination ("Daffodil", "Goldenrod", "Damage"));
		CombinationTable.Add (new Combination ("Daffodil", "Hyacinth", "Armor"));
		CombinationTable.Add (new Combination ("Daffodil", "Lavender", "Health"));
		CombinationTable.Add (new Combination ("Daffodil", "Lily", "Mana"));
		CombinationTable.Add (new Combination ("Daffodil", "Throatwort", "Stamina"));
		CombinationTable.Add (new Combination ("Daffodil", "Tulip", "Damage"));

		CombinationTable.Add (new Combination ("Foxglove", "Goldenrod", "Armor"));
		CombinationTable.Add (new Combination ("Foxglove", "Hyacinth", "Health"));
		CombinationTable.Add (new Combination ("Foxglove", "Lavender", "Mana"));
		CombinationTable.Add (new Combination ("Foxglove", "Lily", "Stamina"));
		CombinationTable.Add (new Combination ("Foxglove", "Throatwort", "Damage"));
		CombinationTable.Add (new Combination ("Foxglove", "Tulip", "Armor"));

		CombinationTable.Add (new Combination ("Goldenrod", "Hyacinth", "Health"));
		CombinationTable.Add (new Combination ("Goldenrod", "Lavender", "Mana"));
		CombinationTable.Add (new Combination ("Goldenrod", "Lily", "Stamina"));
		CombinationTable.Add (new Combination ("Goldenrod", "Throatwort", "Damage"));
		CombinationTable.Add (new Combination ("Goldenrod", "Tulip", "Armor"));

		CombinationTable.Add (new Combination ("Hyacinth", "Lavender", "Health"));
		CombinationTable.Add (new Combination ("Hyacinth", "Lily", "Mana"));
		CombinationTable.Add (new Combination ("Hyacinth", "Throatwort", "Stamina"));
		CombinationTable.Add (new Combination ("Hyacinth", "Tulip", "Damage"));

		CombinationTable.Add (new Combination ("Lavender", "Lily", "Armor"));
		CombinationTable.Add (new Combination ("Lavender", "Throatwort", "Health"));
		CombinationTable.Add (new Combination ("Lavender", "Tulip", "Mana"));

		CombinationTable.Add (new Combination ("Lily", "Throatwort", "Stamina"));
		CombinationTable.Add (new Combination ("Lily", "Tulip", "Damage"));

		CombinationTable.Add (new Combination ("Throatwort", "Tulip", "Armor"));

		database = GameObject.FindGameObjectWithTag ("Database").GetComponent<ItemDatabase> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (IsGUIOpen)
			Counter.GetComponent<Text> ().text = stacksize.ToString ();

		if (containedHerbs.Count == 2) {
			if (timer == 0) {
				GetComponent<AudioSource> ().Play ();
			}

			timer += Time.deltaTime;

			if (timer > 2.5f){
				for (int i = 0; i < CombinationTable.Count; i++) {
					if (CombinationTable [i].flowerOne == containedHerbs [0].itemName && CombinationTable [i].flowerTwo == containedHerbs [1].itemName) {
						for (int x = 0; x < database.potions.Count; x++) {
							if (database.potions [x].GetComponent<ItemData> ().itemName == (CombinationTable [i].outcome + " Potion")) {
								GameObject newPotion = GameObject.Instantiate (database.potions [x], this.transform.position + (this.transform.right * 2), Quaternion.identity);

								int potionstacksize = 1;
								if (containedHerbs [0].stackSize < containedHerbs [1].stackSize) {
									potionstacksize = containedHerbs [0].stackSize;
								}
								if (containedHerbs [1].stackSize < containedHerbs [0].stackSize) {
									potionstacksize = containedHerbs [1].stackSize;
								}
								if (containedHerbs [1].stackSize == containedHerbs [0].stackSize) {
									potionstacksize = containedHerbs [1].stackSize;
								}
								newPotion.GetComponent<ItemData> ().stackSize = potionstacksize;

								if (containedHerbs [0].rarity == "Common" && containedHerbs [1].rarity == "Common") {
									newPotion.GetComponent<ItemData> ().rarity = "Common";
								} else if (containedHerbs [0].rarity == "Rare" && containedHerbs [1].rarity == "Rare") {
									newPotion.GetComponent<ItemData> ().rarity = "Rare";
								} else {
									int rarity = Random.Range (0, 100);
									if (rarity > 50) {
										newPotion.GetComponent<ItemData> ().rarity = "Rare";
									} else {
										newPotion.GetComponent<ItemData> ().rarity = "Common";
									}
								}

								if (!CombinationTable [i].discovered) {
									CombinationTable [i].discovered = true;
								}
								containedHerbs.Clear ();
								timer = 0;
								return;
							}
						}
					}
					if (CombinationTable [i].flowerOne == containedHerbs [1].itemName && CombinationTable [i].flowerTwo == containedHerbs [0].itemName) {
						for (int x = 0; x < database.potions.Count; x++) {
							if (database.potions [x].GetComponent<ItemData> ().itemName == (CombinationTable [i].outcome + " Potion")) {
								GameObject newPotion = GameObject.Instantiate (database.potions [x], this.transform.position + (this.transform.right * 2), Quaternion.identity);

								int potionstacksize = 1;
								if (containedHerbs [0].stackSize < containedHerbs [1].stackSize) {
									potionstacksize = containedHerbs [0].stackSize;
								}
								if (containedHerbs [1].stackSize < containedHerbs [0].stackSize) {
									potionstacksize = containedHerbs [1].stackSize;
								}
								if (containedHerbs [1].stackSize == containedHerbs [0].stackSize) {
									potionstacksize = containedHerbs [1].stackSize;
								}
								newPotion.GetComponent<ItemData> ().stackSize = potionstacksize;

								if (containedHerbs [0].rarity == "Common" && containedHerbs [1].rarity == "Common") {
									newPotion.GetComponent<ItemData> ().rarity = "Common";
								} else if (containedHerbs [0].rarity == "Rare" && containedHerbs [1].rarity == "Rare") {
									newPotion.GetComponent<ItemData> ().rarity = "Rare";
								} else {
									int rarity = Random.Range (0, 100);
									if (rarity > 50) {
										newPotion.GetComponent<ItemData> ().rarity = "Rare";
									} else {
										newPotion.GetComponent<ItemData> ().rarity = "Common";
									}
								}

								if (!CombinationTable [i].discovered) {
									CombinationTable [i].discovered = true;
								}
								containedHerbs.Clear ();
								timer = 0;
								return;
							}
						}
					}
				}
			}
		}

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

					if (containedHerbs.Count == 1) {
						if (containedHerbs [0].itemName != data.itemName) {
							containedHerbs.Add (new Item (data.itemName, data.price, data.weight, data.rarity, data.itemType, stacksize));
						} else {
							containedHerbs [0].stackSize++;
						}
					} else {
						containedHerbs.Add (new Item (data.itemName, data.price, data.weight, data.rarity, data.itemType, stacksize));
					}
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

public class Combination
{
	public string flowerOne;
	public string flowerTwo;
	public string outcome;
	public bool discovered;

	public Combination(string FlowerOne, string FlowerTwo, string Outcome){
		flowerOne = FlowerOne;
		flowerTwo = FlowerTwo;
		outcome = Outcome;
		discovered = false;
	}
}

/* redundant code for old way the worked
HerbTable.Add (new AlchemyHerb ("Amaryllis", "Health", "Damage", "Armor"));
HerbTable.Add (new AlchemyHerb ("Bell Flower", "Damage", "Armor", "Stamina"));
HerbTable.Add (new AlchemyHerb ("Daffodil", "Stamina", "Armor", "Mana"));
HerbTable.Add (new AlchemyHerb ("Foxglove", "Health", "Stamina", "Armor"));
HerbTable.Add (new AlchemyHerb ("Goldenrod", "Stamina", "Mana", "Damage"));
HerbTable.Add (new AlchemyHerb ("Hyacinth", "Armor", "Mana", "Damage"));
HerbTable.Add (new AlchemyHerb ("Lavender", "Mana", "Damage", "Health"));
HerbTable.Add (new AlchemyHerb ("Lily", "Armor", "Health", "Mana"));
HerbTable.Add (new AlchemyHerb ("Throatwort", "Damage", "Stamina", "Health"));
HerbTable.Add (new AlchemyHerb ("Tulip", "Mana", "Health", "Stamina"));

if (containedHerbs.Count == 2) {
			for (int i = 0; i < HerbTable.Count; i++) {
				for (int x = 0; x < HerbTable.Count; x++) {
					if (containedHerbs [0].itemName == HerbTable [i].name) {
						if (containedHerbs [1].itemName == HerbTable [x].name) {
							for (int z = 0; z < database.potions.Count; z++) {
								//TODO: Make this work with varying amounts of herbs and to calculate rarity
								for (int c = 0; c < 3; c++) {
									for (int v = 0; v < 3; v++) {
										if (HerbTable [i].properties [c] == HerbTable [x].properties[v]) {
											if (database.potions [z].GetComponent<ItemData> ().itemName == (HerbTable [i].properties[c] + " Potion")) {
												GameObject newPotion = GameObject.Instantiate (database.potions [z], this.transform.position + (this.transform.right * 2), Quaternion.identity);
												containedHerbs.Clear ();
												return;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		*/