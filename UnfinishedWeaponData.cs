using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnfinishedWeaponData : ItemData {

	public string material;

	public GameObject crossguard;
	public GameObject grip;
	public GameObject pommel;

	public Transform gripPos;
	public Transform pommelPos;
	public Transform crossguardPos;

	void Start(){
		gripPos = transform.FindChild ("GripPos");
		pommelPos = transform.FindChild ("PommelPos");
		crossguardPos = transform.FindChild ("CrossguardPos");
	}
}
