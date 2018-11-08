using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgeScript : MonoBehaviour {

	private ItemDatabase database;
	public Transform itemLoc;
	public Transform spawnLoc;
	private PlayerScript player;

	public GameObject weapon;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();
		database = GameObject.FindGameObjectWithTag ("Database").GetComponent<ItemDatabase> ();
	}

	void Update(){
		if (weapon != null) {
			if (weapon.GetComponent<UnfinishedWeaponData> () != null) {
				UnfinishedWeaponData data = weapon.GetComponent<UnfinishedWeaponData> ();

				if (data.crossguard != null && data.grip != null && data.pommel != null) {
					for (int i = 0; i < database.weaponItems.Count; i++) {
						if (database.weaponItems [i].GetComponent<WeaponData> ().itemName == data.itemName) {
							GameObject createdWeapon = GameObject.Instantiate (database.weaponItems [i], spawnLoc.position, Quaternion.identity);


							for (int x = 0; x < database.parts.Count; x++) {
								if (database.parts [x].GetComponent<ItemData> ().itemName == data.crossguard.GetComponent<ItemData> ().itemName) {
									GameObject newGuard = Instantiate (database.parts [x], createdWeapon.transform);
									Destroy (newGuard.GetComponent<Rigidbody> ());
									Destroy (newGuard.GetComponent<BoxCollider> ());
									Destroy (newGuard.GetComponent<MeshCollider> ());
									createdWeapon.GetComponent<WeaponData> ().crossguard = newGuard;
									createdWeapon.GetComponent<WeaponData> ().crossguard.transform.localPosition = weapon.GetComponent<UnfinishedWeaponData> ().crossguardPos.localPosition;
									createdWeapon.GetComponent<WeaponData> ().crossguard.transform.localRotation = Quaternion.identity;
								}
								if (database.parts [x].GetComponent<ItemData> ().itemName == data.grip.GetComponent<ItemData> ().itemName) {
									GameObject newGrip = Instantiate (database.parts [x], createdWeapon.transform);
									Destroy (newGrip.GetComponent<Rigidbody> ());
									Destroy (newGrip.GetComponent<BoxCollider> ());
									Destroy (newGrip.GetComponent<MeshCollider> ());
									createdWeapon.GetComponent<WeaponData> ().grip = newGrip;
									createdWeapon.GetComponent<WeaponData> ().grip.transform.localPosition = weapon.GetComponent<UnfinishedWeaponData> ().gripPos.localPosition;
									createdWeapon.GetComponent<WeaponData> ().grip.transform.localRotation = Quaternion.identity;
								}
								if (database.parts [x].GetComponent<ItemData> ().itemName == data.pommel.GetComponent<ItemData> ().itemName) {
									GameObject newPommel = Instantiate (database.parts [x], createdWeapon.transform);
									Destroy (newPommel.GetComponent<Rigidbody> ());
									Destroy (newPommel.GetComponent<BoxCollider> ());
									Destroy (newPommel.GetComponent<MeshCollider> ());
									createdWeapon.GetComponent<WeaponData> ().pommel = newPommel;
									createdWeapon.GetComponent<WeaponData> ().pommel.transform.localPosition = weapon.GetComponent<UnfinishedWeaponData> ().pommelPos.localPosition;
									createdWeapon.GetComponent<WeaponData> ().pommel.transform.localRotation = Quaternion.identity;
								}
							}
						}
					}
					Destroy (weapon);
				}
			}
		}
	}

	public void DoInteraction(ItemData Weapon){
		if (weapon == null) {
			if (Weapon.GetComponent<ItemData> ().itemName == "Iron Curved Blade" ||
			    Weapon.GetComponent<ItemData> ().itemName == "Iron Straight Blade" ||
			    Weapon.GetComponent<ItemData> ().itemName == "Iron Square Blade" ||
			    Weapon.GetComponent<ItemData> ().itemName == "Adamantium Curved Blade" ||
			    Weapon.GetComponent<ItemData> ().itemName == "Adamantium Straight Blade" ||
			    Weapon.GetComponent<ItemData> ().itemName == "Adamantium Square Blade") {
				for (int i = 0; i < database.parts.Count; i++) {
					if (database.parts [i].GetComponent<ItemData> ().itemName == Weapon.itemName) {
						GameObject newWeapon = Instantiate (database.parts [i], this.gameObject.transform);
						newWeapon.transform.localPosition = itemLoc.localPosition;
						newWeapon.transform.rotation = itemLoc.rotation;

						newWeapon.AddComponent<UnfinishedWeaponData> ();
						UnfinishedWeaponData data = newWeapon.GetComponent<UnfinishedWeaponData> ();
						data.itemName = Weapon.itemName;
						data.price = Weapon.price;
						data.weight = Weapon.weight;
						data.rarity = Weapon.rarity;
						data.itemType = "Weapon";
						data.stackSize = 1;
						Destroy (newWeapon.GetComponent<ItemData> ());
						Destroy (newWeapon.GetComponent<Rigidbody> ());
						weapon = newWeapon;
					}
				}
			}
		}
		if (weapon != null) {
			if (Weapon.GetComponent<ItemData> ().itemName == "H Crossguard" ||
			   Weapon.GetComponent<ItemData> ().itemName == "W Crossguard" ||
			   Weapon.GetComponent<ItemData> ().itemName == "M Crossguard") {
				for (int i = 0; i < database.parts.Count; i++) {
					if (database.parts [i].GetComponent<ItemData> ().itemName == Weapon.itemName) {
						GameObject newGuard = Instantiate (database.parts [i], weapon.transform);
						Destroy (newGuard.GetComponent<Rigidbody> ());
						Destroy (newGuard.GetComponent<BoxCollider> ());
						Destroy (newGuard.GetComponent<MeshCollider> ());
						weapon.GetComponent<UnfinishedWeaponData> ().crossguard = newGuard;
						weapon.GetComponent<UnfinishedWeaponData> ().crossguard.transform.localPosition = weapon.GetComponent<UnfinishedWeaponData> ().crossguardPos.localPosition;
						weapon.GetComponent<UnfinishedWeaponData> ().crossguard.transform.localRotation = Quaternion.identity;
					}
				}
			}

			if (Weapon.GetComponent<ItemData> ().itemName == "Ball Grip" ||
			   Weapon.GetComponent<ItemData> ().itemName == "Round Grip" ||
			   Weapon.GetComponent<ItemData> ().itemName == "Segment Grip") {
				for (int i = 0; i < database.parts.Count; i++) {
					if (database.parts [i].GetComponent<ItemData> ().itemName == Weapon.itemName) {
						GameObject newGrip = Instantiate (database.parts [i], weapon.transform);
						Destroy (newGrip.GetComponent<Rigidbody> ());
						Destroy (newGrip.GetComponent<BoxCollider> ());
						Destroy (newGrip.GetComponent<MeshCollider> ());
						weapon.GetComponent<UnfinishedWeaponData> ().grip = newGrip;
						weapon.GetComponent<UnfinishedWeaponData> ().grip.transform.localPosition = weapon.GetComponent<UnfinishedWeaponData> ().gripPos.localPosition;
						weapon.GetComponent<UnfinishedWeaponData> ().grip.transform.localRotation = Quaternion.identity;
					}
				}
			}

			if (Weapon.GetComponent<ItemData> ().itemName == "Gem Pommel" ||
			   Weapon.GetComponent<ItemData> ().itemName == "Pointed Pommel" ||
			   Weapon.GetComponent<ItemData> ().itemName == "Round Pommel") {
				for (int i = 0; i < database.parts.Count; i++) {
					if (database.parts [i].GetComponent<ItemData> ().itemName == Weapon.itemName) {
						GameObject newPommel = Instantiate (database.parts [i], weapon.transform);
						Destroy (newPommel.GetComponent<Rigidbody> ());
						Destroy (newPommel.GetComponent<BoxCollider> ());
						Destroy (newPommel.GetComponent<MeshCollider> ());
						weapon.GetComponent<UnfinishedWeaponData> ().pommel = newPommel;
						weapon.GetComponent<UnfinishedWeaponData> ().pommel.transform.localPosition = weapon.GetComponent<UnfinishedWeaponData> ().pommelPos.localPosition;
						weapon.GetComponent<UnfinishedWeaponData> ().pommel.transform.localRotation = Quaternion.identity;
					}
				}
			}
			player.RemoveWeaponFromInventory (1);
			Destroy (player.weapon);
		}
	}
}
