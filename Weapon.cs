using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {

	public int damage;
	public string material;

	public List<string> components = new List<string> ();

	public Weapon (string ItemName, int Price, float Weight, string Rarity, string ItemType, int StackSize, int Damage, string Material, List<string> Components)
		: base (ItemName, Price, Weight, Rarity, ItemType, StackSize)
	{
		damage = Damage;
		material = Material;
		foreach (string str in Components) {
			components.Add (str);
		}
	}
}
