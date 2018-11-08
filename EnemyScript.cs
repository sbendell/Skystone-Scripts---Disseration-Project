using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

	public float health;
	public float starthealth;
	public GameObject hpBar;

	// Use this for initialization
	void Start () {
		
	}

	public void TakeDamage(float damage){
		health -= damage;

		if (GetComponent<Goblin> () != null) {
			GetComponent<Goblin> ().anim.SetTrigger ("Hit" + Random.Range (1, 4).ToString ());
		}

		if (health <= 0) {
			health = 0;
			if (GetComponent<Goblin> () != null) {
				GetComponent<Goblin> ().anim.SetBool("IsDead", true);
			}
		}
	}

	void Update(){
		if (health < starthealth) {
			hpBar.SetActive (true);
			hpBar.transform.localScale = new Vector3(0.3f * (health / starthealth), 0.4f, 1);
		}
	}
}
