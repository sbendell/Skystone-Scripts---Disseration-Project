using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour {

	[SerializeField]
	public List<GameObject> cloudPrefabs;

	private List<GameObject> clouds = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		for (int i = 0; i < 100; i++) {
			int prefabnum = Random.Range (0, 6);

			GameObject cloud = Instantiate (cloudPrefabs [prefabnum], new Vector3 (Random.Range (-500, 500), 100, Random.Range (-500, 500)), Quaternion.Euler (new Vector3 (0, Random.Range (0, 361), 0)));
			clouds.Add (cloud);
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject cloud in clouds) {
			cloud.transform.position += new Vector3 (0.01f, 0, 0);

			if (cloud.transform.position.x > 500) {
				cloud.transform.position = new Vector3 (-500, 100, cloud.transform.position.z);
			}
		}
	}
}
