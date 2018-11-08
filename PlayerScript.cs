using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerScript : MonoBehaviour {

	private Inventory inventory;
	private ItemDatabase database;
	private FirstPersonController controller;
	private Text messageBox;
	public GameObject weapon;
	public GameObject crosshair;
	public GameObject alchemyBook;
	public RectTransform staminaBar;
	//private 
	public RectTransform healthBar;
	private Vector3 localWeaponPosition;

	Console console;
	RaycastHit interactableHit;

	public bool IsGUIOpen;
	public bool waitOneFrame;
	enum InventoryState { Closed, Sections, Items }
	InventoryState invenState = InventoryState.Closed;


	public float health;
	public float maxHealth;
	public float stamina;
	public float maxStamina;

	public GameObject deathmessage;
	private bool IsCollidingWithDeath;
	private float deathtimer = 10f;

	// Use this for initialization
	void Start () {
		console = GameObject.FindGameObjectWithTag ("Console").GetComponent<Console> ();
		database = GameObject.FindGameObjectWithTag ("Database").GetComponent<ItemDatabase> ();
		inventory = GetComponent<Inventory> ();
		controller = GetComponent<FirstPersonController> ();
		messageBox = GameObject.FindGameObjectWithTag ("Message Box").GetComponent<Text> ();
		IsGUIOpen = false;
		waitOneFrame = false;
		IsCollidingWithDeath = true;

		localWeaponPosition = new Vector3 (0.5f, -0.5f, 0.6f);

		health = 100;
		maxHealth = 100;
		stamina = 100;
		maxStamina = 100;
	}

	public void StopMovement(){
		controller.AllowMove = false;
	}

	public void StartMovement(){
		controller.AllowMove = true;
	}

	public void RemoveWeaponFromInventory(int Amount){
		ItemData data = weapon.GetComponent<ItemData> ();

		if (data.itemType == "Potion") {
			for (int i = 0; i < inventory.potions.Count; i++) {
				if (inventory.potions [i].itemName == data.itemName) {
					inventory.potions [i].stackSize -= Amount;
				}
			}
		}
		if (data.itemType == "Herb") {
			for (int i = 0; i < inventory.herbs.Count; i++) {
				if (inventory.herbs [i].itemName == data.itemName) {
					inventory.herbs [i].stackSize -= Amount;
				}
			}
		}
		if (data.itemType == "Part") {
			for (int i = 0; i < inventory.parts.Count; i++) {
				if (inventory.parts [i].itemName == data.itemName) {
					inventory.parts [i].stackSize -= Amount;
				}
			}
		}
	}

	public void TakeDamage(float damage){
		health -= damage;
	}

	void OnTriggerEnter(Collider col){
		if (col.tag == "DeathCollider") {
			deathtimer = 10;
			deathmessage.SetActive (false);
			IsCollidingWithDeath = true;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.tag == "DeathCollider") {
			deathmessage.SetActive (true);
			IsCollidingWithDeath = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (IsCollidingWithDeath == false) {
			deathtimer -= Time.deltaTime;

			if (deathtimer <= 0) {
				Application.Quit ();
			}
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		if (Input.GetButtonDown ("Fire1")) {
			if (!IsGUIOpen) {
				if (weapon != null) {
					if (weapon.tag == "Weapon" || weapon.tag == "Tool") {
						weapon.GetComponent<Animator> ().SetTrigger ("Strike");
					}
				}
			}
		}
		if (Input.GetKeyDown (KeyCode.M)) {
			if (!IsGUIOpen) {
				if (weapon != null) {
					Destroy (weapon);
				}
			}
		}
		if (Input.GetKey (KeyCode.LeftShift)) {
			if (controller.CanRun) {
				stamina -= 0.1f;
			}
		} else {
			stamina += 0.1f;
		}

		if (stamina <= 0) {
			controller.CanRun = false;
		} else {
			controller.CanRun = true;
		}
		staminaBar.sizeDelta = new Vector2 (360 * (stamina / maxStamina), 24);
		healthBar.sizeDelta = new Vector2 (725 * (health / maxHealth), 24);
		UpdateInventory ();
		CheckForItems ();
		if (IsGUIOpen) {
			StopMovement ();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		} else {
			StartMovement ();
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		crosshair.SetActive (!IsGUIOpen);
	}

	void UpdateInventory(){
		switch (invenState) {
		case InventoryState.Closed:
			if (!IsGUIOpen) {
				if (Input.GetButtonDown ("Inventory")) {
					controller.AllowMove = false;
					Cursor.lockState = CursorLockMode.None;
					Cursor.visible = true;
					inventory.inventoryUI.SetActive (true);
					inventory.selectedheader = 0;
					inventory.UpdateHeaders ();
					IsGUIOpen = true;
					invenState = InventoryState.Sections;
				}
			}
			break;
		case InventoryState.Sections:
			if (inventory.inventoryItemValues.activeSelf) {
				inventory.inventoryItemValues.SetActive (false);
			}
			if (Input.GetButtonDown ("Inventory")) {
				controller.AllowMove = true;
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
				inventory.inventoryUI.SetActive (false);
				IsGUIOpen = false;
				invenState = InventoryState.Closed;
			}
			if (inventory.selectedheader > 0) {
				if (Input.GetKeyDown (KeyCode.W)) {
					inventory.selectedItem = 0;
					inventory.selectedheader--;
				}
			}
			if (inventory.selectedheader < 3) {
				if (Input.GetKeyDown (KeyCode.S)) {
					inventory.selectedItem = 0;
					inventory.selectedheader++;
				}
			}
			if (Input.GetButtonDown ("Action")) {
				if (inventory.GetItemListLength != 0) {
					invenState = InventoryState.Items;
				}
			}
			break;
		case InventoryState.Items:
			if (!inventory.inventoryItemValues.activeSelf) {
				inventory.inventoryItemValues.SetActive (true);
			}
			if (inventory.GetItemListLength == 0) {
				invenState = InventoryState.Sections;
			}
			if (Input.GetButtonDown ("Inventory")) {
				invenState = InventoryState.Sections;
			}
			if (inventory.selectedItem > 0) {
				if (Input.GetKeyDown (KeyCode.W)) {
					inventory.selectedItem--;
				}
			}
			if (inventory.selectedItem < inventory.GetItemListLength - 1) {
				if (Input.GetKeyDown (KeyCode.S)) {
					inventory.selectedItem++;
				}
			}

			if (Input.GetKeyDown (KeyCode.Q)) {
				if (inventory.selectedheader == 0) {
					if (inventory.weapons [inventory.selectedItem].stackSize > 0) {
						for (int i = 0; i < database.weaponItems.Count; i++) {
							if (database.weaponItems [i].GetComponent<ItemData> ().itemName == inventory.weapons [inventory.selectedItem].itemName) {
								GameObject spawnedItem = GameObject.Instantiate (database.weaponItems [i],  Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);

								for (int x = 0; x < database.parts.Count; x++) {
									if (database.parts [x].GetComponent<ItemData> ().itemName == ((Weapon)inventory.weapons [inventory.selectedItem]).components [0]) {
										GameObject newGuard = Instantiate (database.parts [x], spawnedItem.transform);
										Destroy (newGuard.GetComponent<Rigidbody> ());
										Destroy (newGuard.GetComponent<BoxCollider> ());
										Destroy (newGuard.GetComponent<MeshCollider> ());
										spawnedItem.GetComponent<WeaponData> ().crossguard = newGuard;
										spawnedItem.GetComponent<WeaponData> ().crossguard.transform.localPosition = spawnedItem.GetComponent<WeaponData> ().crossguardPos.localPosition;
										spawnedItem.GetComponent<WeaponData> ().crossguard.transform.localRotation = Quaternion.identity;
									}
									if (database.parts [x].GetComponent<ItemData> ().itemName == ((Weapon)inventory.weapons [inventory.selectedItem]).components [1]) {
										GameObject newGrip = Instantiate (database.parts [x], spawnedItem.transform);
										Destroy (newGrip.GetComponent<Rigidbody> ());
										Destroy (newGrip.GetComponent<BoxCollider> ());
										Destroy (newGrip.GetComponent<MeshCollider> ());
										spawnedItem.GetComponent<WeaponData> ().grip = newGrip;
										spawnedItem.GetComponent<WeaponData> ().grip.transform.localPosition = spawnedItem.GetComponent<WeaponData> ().gripPos.localPosition;
										spawnedItem.GetComponent<WeaponData> ().grip.transform.localRotation = Quaternion.identity;
									}
									if (database.parts [x].GetComponent<ItemData> ().itemName == ((Weapon)inventory.weapons [inventory.selectedItem]).components [2]) {
										GameObject newPommel = Instantiate (database.parts [x], spawnedItem.transform);
										Destroy (newPommel.GetComponent<Rigidbody> ());
										Destroy (newPommel.GetComponent<BoxCollider> ());
										Destroy (newPommel.GetComponent<MeshCollider> ());
										spawnedItem.GetComponent<WeaponData> ().pommel = newPommel;
										spawnedItem.GetComponent<WeaponData> ().pommel.transform.localPosition = spawnedItem.GetComponent<WeaponData> ().pommelPos.localPosition;
										spawnedItem.GetComponent<WeaponData> ().pommel.transform.localRotation = Quaternion.identity;
									}
								}
								spawnedItem.GetComponent<ItemData> ().rarity = inventory.weapons [inventory.selectedItem].rarity;
								inventory.weapons [inventory.selectedItem].stackSize--;
								return;
							}
						}
					}
				} else if (inventory.selectedheader == 1) {
					if (inventory.potions [inventory.selectedItem].stackSize > 0) {
						for (int i = 0; i < database.potions.Count; i++) {
							if (database.potions [i].GetComponent<ItemData> ().itemName == inventory.potions [inventory.selectedItem].itemName) {
								GameObject spawnedItem = GameObject.Instantiate (database.potions [i], Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
								spawnedItem.GetComponent<ItemData> ().rarity = inventory.potions [inventory.selectedItem].rarity;
								inventory.potions [inventory.selectedItem].stackSize--;
								return;
							}
						}
					}
				} else if (inventory.selectedheader == 2) {
					if (inventory.herbs [inventory.selectedItem].stackSize > 0) {
						for (int i = 0; i < database.herbs.Count; i++) {
							if (database.herbs [i].GetComponent<ItemData> ().itemName == inventory.herbs [inventory.selectedItem].itemName) {
								GameObject spawnedItem = GameObject.Instantiate (database.herbs [i], Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
								spawnedItem.GetComponent<ItemData> ().rarity = inventory.herbs [inventory.selectedItem].rarity;
								inventory.herbs [inventory.selectedItem].stackSize--;

								return;
							}
						}
					}
				} else if (inventory.selectedheader == 3) {
					if (inventory.parts [inventory.selectedItem].stackSize > 0) {
						for (int i = 0; i < database.parts.Count; i++) {
							if (database.parts [i].GetComponent<ItemData> ().itemName == inventory.parts [inventory.selectedItem].itemName) {
								GameObject spawnedItem = GameObject.Instantiate (database.parts [i], Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);
								spawnedItem.GetComponent<ItemData> ().rarity = inventory.parts [inventory.selectedItem].rarity;
								inventory.parts [inventory.selectedItem].stackSize--;

								return;
							}
						}
					}
				}
			}

			if (Input.GetButtonDown ("Action")) {
				if (inventory.selectedheader == 0) {
					if (inventory.weapons [inventory.selectedItem].itemType == "Weapon") {
						for (int i = 0; i < database.weapons.Count; i++) {
							if (database.weapons [i].GetComponent<WeaponData> ().itemName == inventory.weapons [inventory.selectedItem].itemName) {
								
								Destroy (weapon);
								GameObject spawnedItem = GameObject.Instantiate (database.weapons [i], Camera.main.transform);
								weapon = spawnedItem;

								for (int x = 0; x < database.parts.Count; x++) {
									if (database.parts [x].GetComponent<ItemData> ().itemName == ((Weapon)inventory.weapons [inventory.selectedItem]).components [0]) {
										GameObject newGuard = Instantiate (database.parts [x], spawnedItem.transform);
										Destroy (newGuard.GetComponent<Rigidbody> ());
										Destroy (newGuard.GetComponent<BoxCollider> ());
										Destroy (newGuard.GetComponent<MeshCollider> ());
										spawnedItem.GetComponent<WeaponData> ().crossguard = newGuard;
										spawnedItem.GetComponent<WeaponData> ().crossguard.transform.localPosition = weapon.GetComponent<WeaponData> ().crossguardPos.localPosition;
										spawnedItem.GetComponent<WeaponData> ().crossguard.transform.localRotation = Quaternion.identity;
									}
									if (database.parts [x].GetComponent<ItemData> ().itemName == ((Weapon)inventory.weapons [inventory.selectedItem]).components [1]) {
										GameObject newGrip = Instantiate (database.parts [x], spawnedItem.transform);
										Destroy (newGrip.GetComponent<Rigidbody> ());
										Destroy (newGrip.GetComponent<BoxCollider> ());
										Destroy (newGrip.GetComponent<MeshCollider> ());
										spawnedItem.GetComponent<WeaponData> ().grip = newGrip;
										spawnedItem.GetComponent<WeaponData> ().grip.transform.localPosition = weapon.GetComponent<WeaponData> ().gripPos.localPosition;
										spawnedItem.GetComponent<WeaponData> ().grip.transform.localRotation = Quaternion.identity;
									}
									if (database.parts [x].GetComponent<ItemData> ().itemName == ((Weapon)inventory.weapons [inventory.selectedItem]).components [2]) {
										GameObject newPommel = Instantiate (database.parts [x], spawnedItem.transform);
										Destroy (newPommel.GetComponent<Rigidbody> ());
										Destroy (newPommel.GetComponent<BoxCollider> ());
										Destroy (newPommel.GetComponent<MeshCollider> ());
										spawnedItem.GetComponent<WeaponData> ().pommel = newPommel;
										spawnedItem.GetComponent<WeaponData> ().pommel.transform.localPosition = weapon.GetComponent<WeaponData> ().pommelPos.localPosition;
										spawnedItem.GetComponent<WeaponData> ().pommel.transform.localRotation = Quaternion.identity;
									}
								}
								spawnedItem.transform.localPosition = localWeaponPosition;
								spawnedItem.GetComponent<ItemData> ().rarity = inventory.weapons [inventory.selectedItem].rarity;
								return;
							}
						}
					} else if (inventory.weapons [inventory.selectedItem].itemType == "Tool") {
						for (int i = 0; i < database.tools.Count; i++) {
							if (database.tools [i].GetComponent<WeaponData> ().itemName == inventory.weapons [inventory.selectedItem].itemName) {
								Destroy (weapon);
								GameObject spawnedItem = GameObject.Instantiate (database.tools [i], Camera.main.transform);
								spawnedItem.transform.localPosition = localWeaponPosition;
								spawnedItem.GetComponent<WeaponData> ().rarity = inventory.weapons [inventory.selectedItem].rarity;
								weapon = spawnedItem;
								return;
							}
						}
					}
				} else if (inventory.selectedheader == 1) {
					for (int i = 0; i < database.potions.Count; i++) {
						if (database.potions [i].GetComponent<ItemData> ().itemName == inventory.potions [inventory.selectedItem].itemName) {
							Destroy (weapon);
							GameObject spawnedItem = GameObject.Instantiate (database.potions [i], Camera.main.transform);
							spawnedItem.transform.localPosition = localWeaponPosition;
							spawnedItem.transform.transform.localRotation = Quaternion.identity;
							spawnedItem.GetComponent<ItemData> ().stackSize = inventory.potions [inventory.selectedItem].stackSize;
							spawnedItem.GetComponent<ItemData> ().rarity = inventory.potions [inventory.selectedItem].rarity;
							Destroy (spawnedItem.GetComponent<Rigidbody> ());
							weapon = spawnedItem;
							return;
						}
					}				
				} else if (inventory.selectedheader == 2) {
					for (int i = 0; i < database.herbs.Count; i++) {
						if (database.herbs [i].GetComponent<ItemData> ().itemName == inventory.herbs [inventory.selectedItem].itemName) {
							Destroy (weapon);
							GameObject spawnedItem = GameObject.Instantiate (database.herbs [i], Camera.main.transform);
							spawnedItem.transform.localPosition = localWeaponPosition;
							spawnedItem.transform.transform.localRotation = Quaternion.identity;
							spawnedItem.GetComponent<ItemData> ().stackSize = inventory.herbs [inventory.selectedItem].stackSize;
							spawnedItem.GetComponent<ItemData> ().rarity = inventory.herbs [inventory.selectedItem].rarity;
							Destroy (spawnedItem.GetComponent<Rigidbody> ());
							weapon = spawnedItem;
							return;
						}
					}
				} else if (inventory.selectedheader == 3) {
					for (int i = 0; i < database.parts.Count; i++) {
						if (database.parts [i].GetComponent<ItemData> ().itemName == inventory.parts [inventory.selectedItem].itemName) {
							Destroy (weapon);
							GameObject spawnedItem = GameObject.Instantiate (database.parts [i], Camera.main.transform);
							spawnedItem.transform.localPosition = localWeaponPosition;
							spawnedItem.transform.transform.localRotation = Quaternion.identity;
							spawnedItem.GetComponent<ItemData> ().stackSize = inventory.parts [inventory.selectedItem].stackSize;
							Destroy (spawnedItem.GetComponent<Rigidbody> ());
							weapon = spawnedItem;
							return;
						}
					}
				}
			}
			break;
		}
	}
		
	void CheckForItems(){
		waitOneFrame = false;
		messageBox.text = "";
		if (!IsGUIOpen) {
			if (!waitOneFrame) {
				if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out interactableHit, 3f)) {
					if (interactableHit.collider.gameObject.tag == "Item") {
						messageBox.text = "Press E to pickup " + interactableHit.collider.gameObject.GetComponent<ItemData> ().itemName;
						if (Input.GetButtonDown ("Action")) {
							ItemData data = interactableHit.collider.gameObject.GetComponent<ItemData> ();
							GetComponent<Inventory> ().AddItem (data);
							Color whatcol;
							if (data.rarity == "Rare") {
								whatcol = Color.cyan;
							} else {
								whatcol = Color.white;
							}
							console.OutputToConsole ("Picked up " + data.itemName + " (" + data.stackSize.ToString () + ")", whatcol);
							Destroy (interactableHit.collider.gameObject);
						}
					} else if (interactableHit.collider.gameObject.tag == "Tool") {
						messageBox.text = "Press E to pickup " + interactableHit.collider.gameObject.GetComponent<ItemData> ().itemName;
						if (Input.GetButtonDown ("Action")) {
							WeaponData data = interactableHit.collider.gameObject.GetComponent<WeaponData> ();
							GetComponent<Inventory> ().AddWeapon (data);
							Color whatcol;
							if (data.rarity == "Rare") {
								whatcol = Color.cyan;
							} else {
								whatcol = Color.white;
							}
							console.OutputToConsole ("Picked up " + data.itemName, whatcol);
							Destroy (interactableHit.collider.gameObject);
						}
					} else if (interactableHit.collider.gameObject.tag == "Weapon") {
						messageBox.text = "Press E to pickup " + interactableHit.collider.gameObject.GetComponent<ItemData> ().itemName;
						if (Input.GetButtonDown ("Action")) {
							WeaponData data = interactableHit.collider.gameObject.GetComponent<WeaponData> ();
							GetComponent<Inventory> ().AddWeapon (data);
							Color whatcol;
							if (data.rarity == "Rare") {
								whatcol = Color.cyan;
							} else {
								whatcol = Color.white;
							}
							console.OutputToConsole ("Picked up " + data.itemName, whatcol);
							Destroy (interactableHit.collider.gameObject);
						}
					} else if (interactableHit.collider.gameObject.tag == "Alchemy Bowl") {
						messageBox.text = "Press E to access Alchemy Station";
						if (weapon != null) {
							if (weapon.GetComponent<ItemData> ().itemType == "Herb") {
								if (Input.GetButtonDown ("Action")) {
									IsGUIOpen = true;
									interactableHit.collider.GetComponentInParent<AlchemyStation> ().CounterWindow.SetActive (true);
									interactableHit.collider.GetComponentInParent<AlchemyStation> ().IsGUIOpen = true;
									interactableHit.collider.GetComponentInParent<AlchemyStation> ().stacksize = 1;
									waitOneFrame = true;
								}
							}
						}
					} else if (interactableHit.collider.gameObject.tag == "Alchemy Book") {
						messageBox.text = "Press E to access Alchemy Book";
						if (Input.GetButtonDown ("Action")) {
							IsGUIOpen = true;
							alchemyBook.SetActive (true);
						}
					} else if (interactableHit.collider.gameObject.tag == "Smelter") {
						messageBox.text = "Press E to access Smelter";
						if (weapon != null) {
							if (weapon.GetComponent<ItemData> ().itemName == "Iron Ore" || weapon.GetComponent<ItemData> ().itemName == "Adamantium Ore") {
								if (interactableHit.collider.GetComponentInParent<SmelterScript> ().ContainedBarsCount == 0) {
									if (Input.GetButtonDown ("Action")) {
										IsGUIOpen = true;
										interactableHit.collider.GetComponentInParent<SmelterScript> ().CounterWindow.SetActive (true);
										interactableHit.collider.GetComponentInParent<SmelterScript> ().IsGUIOpen = true;
										interactableHit.collider.GetComponentInParent<SmelterScript> ().stacksize = 1;
										waitOneFrame = true;
									}
								}
							}
						}
					} else if (interactableHit.collider.gameObject.tag == "Furnace") {
						messageBox.text = "Press E to access Furnace";
						if (weapon != null) {
							if (Input.GetButtonDown ("Action")) {
								if (weapon.GetComponent<ItemData> ().itemName == "Iron Bar" || weapon.GetComponent<ItemData> ().itemName == "Adamantium Bar") {
									IsGUIOpen = true;
									interactableHit.collider.GetComponent<FurnaceScript> ().CounterWindow.SetActive (true);
									interactableHit.collider.GetComponent<FurnaceScript> ().IsGUIOpen = true;
									interactableHit.collider.GetComponent<FurnaceScript> ().stacksize = 3;
									waitOneFrame = true;
								}
							}
						}
					} else if (interactableHit.collider.gameObject.tag == "Mould") {
						messageBox.text = "Press E to change mould";
						if (Input.GetButtonDown ("Action")) {
							IsGUIOpen = true;
							interactableHit.collider.GetComponent<MouldScript> ().SequenceWindow.SetActive (true);
							interactableHit.collider.GetComponent<MouldScript> ().IsGUIOpen = true;
						}
					} else if (interactableHit.collider.gameObject.tag == "Anvil") {
						messageBox.text = "Press E to access Anvil";
						if (weapon != null) {
							if (weapon.GetComponent<ItemData> ().itemName == "Iron Bar") {
								if (Input.GetButtonDown ("Action")) {
									IsGUIOpen = true;
									interactableHit.collider.GetComponent<AnvilScript> ().AnvilWindow.SetActive (true);
									interactableHit.collider.GetComponent<AnvilScript> ().IsGUIOpen = true;
									interactableHit.collider.GetComponent<AnvilScript> ().counter = 1;
								}
							}
						}
					} else if (interactableHit.collider.gameObject.tag == "Tannery") {
						messageBox.text = "Press E to access Tannery";
						if (weapon != null) {
							if (weapon.GetComponent<ItemData> ().itemName == "Leather") {
								if (Input.GetButtonDown ("Action")) {
									IsGUIOpen = true;
									interactableHit.collider.GetComponent<TanneryScript> ().TanneryWindow.SetActive (true);
									interactableHit.collider.GetComponent<TanneryScript> ().IsGUIOpen = true;
									interactableHit.collider.GetComponent<TanneryScript> ().counter = 1;
								}
							}
						}
					} else if (interactableHit.collider.gameObject.tag == "Forge") {
						messageBox.text = "Press E to access forge";
						if (weapon != null) {
							if (weapon.GetComponent<ItemData> () != null) {
								if (Input.GetButtonDown ("Action")) {
									interactableHit.collider.GetComponent<ForgeScript> ().DoInteraction (weapon.GetComponent<ItemData> ());
									waitOneFrame = true;
								}
							}
						}
					}
				}
			}
		}
	}
}
