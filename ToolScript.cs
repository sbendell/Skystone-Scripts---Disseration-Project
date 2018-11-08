using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolScript : MonoBehaviour {

	public AudioClip chink;
	public AudioClip thud;
	public GameObject sparkParticle;
	private Animator anim;

	private bool CanHarvest = false;

	void Start(){
		anim = GetComponent<Animator> ();
	}

	void OnCollisionStay(Collision col){
		if (col.gameObject.tag == "OreNode") {
			if (CanHarvest && anim.GetCurrentAnimatorStateInfo (0).IsName ("ToolStrike")) {
				if (col.gameObject.GetComponent<OreScript> ().OrePieces.Count != 0) {
					GetComponent<AudioSource> ().PlayOneShot (chink);
					col.gameObject.GetComponent<OreScript> ().CreateOre ();
					Instantiate (sparkParticle, col.contacts[0].point, Quaternion.identity);
					CanHarvest = false;
				} else {
					GetComponent<AudioSource> ().PlayOneShot (thud);
					CanHarvest = false;
				}
			}
		}
	}

	void Update(){
		if (!CanHarvest) {
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("ToolIdle")) {
				CanHarvest = true;
			}
		}
	}
}
