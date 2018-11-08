using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouldScript : MonoBehaviour {

	[SerializeField]
	public List<Mesh> mouldMeshses = new List<Mesh> ();

	private List<string> meshNames = new List<string> ();

	public GameObject SequenceWindow;
	public GameObject Sequence;
	private PlayerScript player;
	public bool IsGUIOpen;
	private int selectedMesh = 0;

	private MeshFilter filter;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerScript> ();
		filter = GetComponent<MeshFilter> ();

		meshNames.Add ("Viking Axe Head");
		meshNames.Add ("Bearded Axe Head");
		meshNames.Add ("Dual Axe Head");
		meshNames.Add ("Straight Blade");
		meshNames.Add ("Curved Blade");
		meshNames.Add ("Square Blade");
	}

	public string CurrentMeshName{
		get { return meshNames [selectedMesh]; }
	}

	// Update is called once per frame
	void Update () {
		if (IsGUIOpen){
			Sequence.GetComponent<Text> ().text = meshNames [selectedMesh];
			filter.mesh = mouldMeshses[selectedMesh];
		}

		if (IsGUIOpen) {
			if (Input.GetKeyDown (KeyCode.A)) {
				if (selectedMesh < mouldMeshses.Count - 1) {
					selectedMesh++;
				} else {
					selectedMesh = 0;
				}
			}

			if (Input.GetKeyDown (KeyCode.D)) {
				if (selectedMesh > 0) {
					selectedMesh--;
				} else {
					selectedMesh = mouldMeshses.Count - 1;
				}
			}

			if (Input.GetButtonDown ("Inventory")) {
				player.IsGUIOpen = false;
				SequenceWindow.SetActive (false);
				IsGUIOpen = false;
			}
		}
	}
}
