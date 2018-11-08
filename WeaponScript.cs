using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour {

	public AudioClip chink;
	public GameObject[] bloodSplat;
	private Animator anim;
	bool CanHit;

	void Start(){
		anim = GetComponent<Animator> ();
		CanHit = true;
	}

	void OnCollisionStay(Collision col){
		if (col.collider.tag == "Enemy") {
			if (CanHit && anim.GetCurrentAnimatorStateInfo (0).IsName ("ToolStrike")) {
				if (col.collider.gameObject.GetComponent<EnemyScript> ().health != 0) {
					GetComponent<AudioSource> ().PlayOneShot (chink);
					int x = Random.Range (0, 3);
					Instantiate (bloodSplat [x], col.contacts [0].point, Quaternion.identity);
					col.collider.gameObject.GetComponent<EnemyScript> ().TakeDamage (GetComponent<WeaponData> ().damage);
					CanHit = false;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (!CanHit) {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("ToolIdle")) {
				CanHit = true;
			}
		}
	}
}
