using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Goblin : MonoBehaviour {

	NavMeshAgent agent;
	public Animator anim;
	Transform player;
	EntityState state;
	enum EntityState { Idle, Searching, InCombat, Dead };
	Vector3 initialPos;

	int IdleLookTimer = 0;
	int attackTimer = 500;
	float goblinDamage = 5;

	// Use this for initialization
	void Start () {
		state = EntityState.Idle;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
		initialPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<EnemyScript> ().health == 0) {
			if (state != EntityState.Dead) {
				agent.Stop ();
				state = EntityState.Dead;
			}
		}

		switch (state) {
		case EntityState.Idle:
			IdleLookTimer++;

			if (IdleLookTimer > 600) {
				anim.SetTrigger ("IdleLook");
				IdleLookTimer = 0;
			}

			if (Vector3.Distance (this.transform.position, agent.destination) < 2) {
				anim.SetBool ("InCombat", false);
			}

			if (Vector3.Distance (this.transform.position, player.transform.position) < 13) {
				agent.SetDestination (player.transform.position);
				anim.SetBool ("InCombat", true);
				state = EntityState.Searching;
			}
			break;
		case EntityState.Searching:
			if (Vector3.Distance (this.transform.position, player.transform.position) < 7) {
				state = EntityState.InCombat;
			}
			if (Vector3.Distance (this.transform.position, agent.destination) < 2) {
				agent.SetDestination (initialPos);
				state = EntityState.Idle;
			}
			break;
		case EntityState.InCombat:
			anim.SetBool ("InRange", false);
			if (Vector3.Distance (this.transform.position, player.position) > 13) {
				anim.SetBool ("InCombat", true);
				state = EntityState.Searching;
			} else if (Vector3.Distance (this.transform.position, player.position) > 3) {
				agent.SetDestination (player.position);
				agent.Resume ();
			}

			if (Vector3.Distance (this.transform.position, player.position) < 3) {
				anim.SetBool ("InRange", true);
				agent.Stop ();
				attackTimer++;
				if (attackTimer > 300) {
					anim.SetTrigger ("Attack" + Random.Range (1, 3).ToString ());
					player.gameObject.GetComponent<PlayerScript> ().TakeDamage (goblinDamage);
					attackTimer = 0;
				}
			}
			break;
		}
		this.transform.position = new Vector3 (this.transform.position.x, 5.468f, this.transform.position.z);
	}
}
