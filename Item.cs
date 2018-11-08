using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item {

	public string itemName;
	public int price;
	public float weight;
	public string rarity;
	public string itemType;
	public int stackSize = 1;

	public Item(string ItemName, int Price, float Weight, string Rarity, string ItemType, int StackSize){
		itemName = ItemName;
		price = Price;
		weight = Weight;
		rarity = Rarity;
		itemType = ItemType;
		stackSize = StackSize;
	}
}
