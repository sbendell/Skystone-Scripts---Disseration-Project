using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ItemData {

	public int damage;
	public string material;

	public GameObject crossguard;
	public GameObject grip;
	public GameObject pommel;

	public Transform crossguardPos;
	public Transform gripPos;
	public Transform pommelPos;

	public float swingTime{
		get { return 10f / weight; }
	}
}
