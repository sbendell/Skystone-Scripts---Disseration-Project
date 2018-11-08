using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreScript : MonoBehaviour {

	public GameObject metalOreItem;
	public GameObject metalOrePiece;

	public List<GameObject> OrePieces = new List<GameObject> ();

	[SerializeField]
	public List<Transform> metalOrePieceSpawnLocations;

	void Start(){
		int pieces = Random.Range (3, 7);

		for (int i = 0; i < pieces; i++) {
			int loc = Random.Range (0, metalOrePieceSpawnLocations.Count - 1);
			GameObject piece = Instantiate (metalOrePiece, metalOrePieceSpawnLocations [loc]);
			piece.transform.position = piece.transform.parent.transform.position;
			piece.transform.rotation = piece.transform.parent.transform.rotation;
			OrePieces.Add (piece);
			metalOrePieceSpawnLocations.RemoveAt (loc);
		}
	}

	public void CreateOre(){
		Instantiate (metalOreItem, this.gameObject.transform.position + new Vector3 (0, 2, -1), Quaternion.identity);

		int removeWhatPiece = Random.Range (0, metalOrePieceSpawnLocations.Count - 1);
		Destroy (OrePieces[removeWhatPiece]);
		OrePieces.RemoveAt (removeWhatPiece);
	}
}
