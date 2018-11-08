using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour {

	public GameObject flower;

	public List<GameObject> flowers = new List<GameObject> ();

	[SerializeField]
	public List<Transform> flowerSpawnLocations;

	// Use this for initialization
	void Start () {
		int howMany = Random.Range (1, 4);

		for (int i = 0; i < howMany; i++) {
			int loc = Random.Range (0, flowerSpawnLocations.Count - 1);
			GameObject piece = Instantiate (flower, flowerSpawnLocations [loc]);
			piece.transform.position = piece.transform.parent.transform.position;
			piece.transform.rotation = piece.transform.parent.transform.rotation;
			Destroy (piece.GetComponent<Rigidbody> ());
			Destroy (piece.GetComponent<MeshCollider> ());
			Destroy (piece.GetComponent<ItemData> ());
			flowers.Add (piece);
			flowerSpawnLocations.RemoveAt (loc);
		}

		GetComponent<ItemData> ().stackSize = howMany;

		int rarity = Random.Range (0, 100);

		if (rarity > 80) {
			GetComponent<ItemData> ().rarity = "Rare";
		} else {
			GetComponent<ItemData> ().rarity = "Common";
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
