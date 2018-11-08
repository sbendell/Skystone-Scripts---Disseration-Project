using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour{
	
	[SerializeField]
	public List<Weapon> weapons = new List<Weapon> ();
	[SerializeField]
	public List<Item> potions = new List<Item> ();
	[SerializeField]
	public List<Item> herbs = new List<Item> ();
	[SerializeField]
	public List<Item> parts = new List<Item> ();

	[SerializeField]
	List<GameObject> headerNames = new List<GameObject> ();
	[SerializeField]
	List<GameObject> itemNames = new List<GameObject> ();

	public GameObject inventoryUI;
	public GameObject inventoryItemText;
	public GameObject inventoryItemSection;
	public GameObject inventoryItemValues;
	public Text itemNameText;
	public Text itemQuantityText;
	public Text itemRarityText;
	public Text itemWeightText;
	public Text itemPriceText;

	public Color baseColor;
	public Color highlightColor;
	public int selectedheader = 0;
	int lastselectedheader = 0;
	public int selectedItem = 0;
	int timer;

	void Start(){
	}

	public int GetItemListLength{
		get { return itemNames.Count; }
	}

	public void RemoveItemFromItemList(int index){
		Destroy(itemNames[index]);
		itemNames.RemoveAt (index);
	}

	public void AddItem(ItemData newItem){
		//Check what item type it is
		if (newItem.itemType == "Potion") {
			for (int i = 0; i < potions.Count; i++) {
				//If object is in inventory then increase stack size and stop method
				if (potions [i].itemName == newItem.itemName && potions[i].rarity == newItem.rarity) {
					potions [i].stackSize += newItem.stackSize;
					return;
				}
			}
			//After invetory has been checked and item is not present, then add the new item
			potions.Add (new Item(newItem.itemName, newItem.price, newItem.weight, newItem.rarity, newItem.itemType, newItem.stackSize));
		}else if (newItem.itemType == "Herb") {
			for (int i = 0; i < herbs.Count; i++) {
				//If object is in inventory then increase stack size and stop method
				if (herbs [i].itemName == newItem.itemName && herbs [i].rarity == newItem.rarity) {
					herbs [i].stackSize += newItem.stackSize;
					return;
				}
			}
			//After invetory has been checked and item is not present, then add the new item
			herbs.Add (new Item(newItem.itemName, newItem.price, newItem.weight, newItem.rarity, newItem.itemType, newItem.stackSize));
		}else if (newItem.itemType == "Part") {
			for (int i = 0; i < parts.Count; i++) {
				//If object is in inventory then increase stack size and stop method
				if (parts [i].itemName == newItem.itemName && parts [i].rarity == newItem.rarity) {
					parts [i].stackSize += newItem.stackSize;
					return;
				}
			}
			//After invetory has been checked and item is not present, then add the new item
			parts.Add (new Item(newItem.itemName, newItem.price, newItem.weight, newItem.rarity, newItem.itemType, newItem.stackSize));
		}
	}



	public void AddWeapon(WeaponData newWeapon)
	{
		if (newWeapon.itemType == "Weapon") {
			List<string> components = new List<string> ();
			components.Add (newWeapon.crossguard.GetComponent<ItemData> ().itemName);
			components.Add (newWeapon.grip.GetComponent<ItemData> ().itemName);
			components.Add (newWeapon.pommel.GetComponent<ItemData> ().itemName);

			weapons.Add (new Weapon (newWeapon.itemName, newWeapon.price, newWeapon.weight, newWeapon.rarity, newWeapon.itemType, newWeapon.stackSize,
				newWeapon.damage, newWeapon.material, components));
		} else if (newWeapon.itemType == "Tool") {
			List<string> components = new List<string> ();	
			weapons.Add(new Weapon(newWeapon.itemName, newWeapon.price, newWeapon.weight, newWeapon.rarity, newWeapon.itemType, newWeapon.stackSize,
				newWeapon.damage, newWeapon.material, components));
		}
	}

	public void UpdateHeaders(){
		foreach (GameObject t in itemNames) {
			Destroy (t);
		}
		itemNames.Clear ();
		if (selectedheader == 0) {
			for (int i = 0; i < weapons.Count; i++) {
				GameObject textItem = Instantiate (inventoryItemText, inventoryItemSection.transform);
				itemNames.Add (textItem);
			}
		}else if (selectedheader == 1) {
			for (int i = 0; i < potions.Count; i++) {
				GameObject textItem = Instantiate (inventoryItemText, inventoryItemSection.transform);
				itemNames.Add (textItem);
			}
		}
		else if (selectedheader == 2) {
			for (int i = 0; i < herbs.Count; i++) {
				GameObject textItem = Instantiate (inventoryItemText, inventoryItemSection.transform);
				itemNames.Add (textItem);
			}
		}
		else if (selectedheader == 3) {
			for (int i = 0; i < parts.Count; i++) {
				GameObject textItem = Instantiate (inventoryItemText, inventoryItemSection.transform);
				itemNames.Add (textItem);
			}
		}


	}

	public void UpdateItemValues(){
		//TODO: Make it so weapons dont display stacksize, as they dont stack.
		if (selectedheader == 0) {
			if (weapons.Count != 0) {
				itemNameText.text = weapons [selectedItem].itemName.ToString ();
				itemQuantityText.text = "Quantity: " + weapons [selectedItem].stackSize.ToString ();
				itemRarityText.text = "Quality: " + weapons [selectedItem].rarity.ToString ();
				itemWeightText.text = "Weight: " + (weapons [selectedItem].weight * weapons [selectedItem].stackSize).ToString ();
				itemPriceText.text = "Price: " + (weapons [selectedItem].price * weapons [selectedItem].stackSize).ToString ();
			}
		}else if (selectedheader == 1) {
			if (potions.Count != 0) {
				itemNameText.text = potions [selectedItem].itemName.ToString ();
				itemQuantityText.text = "Quantity: " + potions [selectedItem].stackSize.ToString ();
				itemRarityText.text = "Quality: " + potions [selectedItem].rarity.ToString ();
				itemWeightText.text = "Weight: " + (potions [selectedItem].weight * potions [selectedItem].stackSize).ToString ();
				itemPriceText.text = "Price: " + (potions [selectedItem].price * potions [selectedItem].stackSize).ToString ();
			}
		}
		else if (selectedheader == 2) {
			if (herbs.Count != 0) {
				itemNameText.text = herbs [selectedItem].itemName.ToString ();
				itemQuantityText.text = "Quantity: " + herbs [selectedItem].stackSize.ToString ();
				itemRarityText.text = "Quality: " + herbs [selectedItem].rarity.ToString ();
				itemWeightText.text = "Weight: " + (herbs [selectedItem].weight * herbs [selectedItem].stackSize).ToString ();
				itemPriceText.text = "Price: " + (herbs [selectedItem].price * herbs [selectedItem].stackSize).ToString ();
			}
		}
		else if (selectedheader == 3) {
			if (parts.Count != 0) {
				itemNameText.text = parts [selectedItem].itemName.ToString ();
				itemQuantityText.text = "Quantity: " + parts [selectedItem].stackSize.ToString ();
				itemRarityText.text = "Quality: " + parts [selectedItem].rarity.ToString ();
				itemWeightText.text = "Weight: " + (parts [selectedItem].weight * parts [selectedItem].stackSize).ToString ();
				itemPriceText.text = "Price: " + (parts [selectedItem].price * parts [selectedItem].stackSize).ToString ();
			}
		}
	}

	private void UpdateSectionList(){
		for (int i = 0; i < itemNames.Count; i++) {
			itemNames [i].GetComponent<RectTransform> ().anchoredPosition = new Vector3 (0, (i * -18) + (selectedItem * 18), -1);

			if (selectedheader == 0) {
				itemNames [i].GetComponent<Text> ().text = weapons [i].itemName;
			} else if (selectedheader == 1) {
				itemNames [i].GetComponent<Text> ().text = potions [i].itemName;
			}else if (selectedheader == 2) {
				itemNames [i].GetComponent<Text> ().text = herbs [i].itemName;
			}else if (selectedheader == 3) {
				itemNames [i].GetComponent<Text> ().text = parts [i].itemName;
			}

			if (i == selectedItem) {
				itemNames [i].GetComponent<Text> ().color = highlightColor;
			} else {
				itemNames [i].GetComponent<Text> ().color = baseColor;
			}
		}
	}

	void Update()
	{
		if (selectedheader != lastselectedheader) {
			UpdateHeaders ();
		}

		for (int i = 0; i < headerNames.Count; i++) {
			if (i == selectedheader) {
				headerNames [i].GetComponent<Text> ().color = highlightColor;
			} else {
				headerNames [i].GetComponent<Text> ().color = baseColor;
			}
		}

		UpdateSectionList ();
		UpdateItemValues ();

		for (int i = 0; i < weapons.Count; i++) {
			if (weapons [i].stackSize == 0) {
				weapons.RemoveAt (i);
				RemoveItemFromItemList (i);
				if (selectedItem != 0) {
					selectedItem--;
				}
			}
		}
		for (int i = 0; i < potions.Count; i++) {
			if (potions [i].stackSize == 0) {
				potions.RemoveAt (i);
				RemoveItemFromItemList (i);
				if (selectedItem != 0) {
					selectedItem--;
				}
			}
		}
		for (int i = 0; i < herbs.Count; i++) {
			if (herbs [i].stackSize == 0) {
				herbs.RemoveAt (i);
				RemoveItemFromItemList (i);
				if (selectedItem != 0) {
					selectedItem--;
				}
			}
		}
		for (int i = 0; i < parts.Count; i++) {
			if (parts [i].stackSize == 0) {
				parts.RemoveAt (i);
				RemoveItemFromItemList (i);
				if (selectedItem != 0) {
					selectedItem--;
				}
			}
		}

		lastselectedheader = selectedheader;
	}
}
