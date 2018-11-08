using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CowScript : MonoBehaviour {

	NavMeshAgent agent;
	Transform player;
	Animator anim;
	EntityState state;
	enum EntityState { Idle, Wander, Fleeing, Dead };

	private int idleTimer;
	private int wanderTimer;

	public GameObject leather;

	// Use this for initialization
	void Start () {
		state = EntityState.Wander;
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform>();
		int x = Random.Range (0, 2);
		if (x == 0) {
			state = EntityState.Idle;
		}
		if (x == 1) {
			state = EntityState.Wander;
		}

		idleTimer = Random.Range (0, 300);
		wanderTimer = Random.Range (0, 600);
	}

	// Update is called once per frame
	void Update () {
		if (GetComponent<EnemyScript> ().health == 0) {
			if (state != EntityState.Dead) {
				agent.Stop ();
				Instantiate (leather, this.transform.position + new Vector3 (0, 5, 0), Quaternion.identity);
				state = EntityState.Dead;
			}
		}
		switch (state) {
		case EntityState.Idle:
			if (Vector3.Distance (player.transform.position, this.transform.position) < 3) {
				Vector3 dest = new Vector3 (Random.Range (-20f, 20f), 0f, Random.Range (-20f, 20f));
				state = EntityState.Fleeing;
			}
			idleTimer++;
			anim.SetBool ("IsWalking", false);
			if (idleTimer > 300) {
				state = EntityState.Wander;
				Vector3 dest = new Vector3 (Random.Range (-20f, 20f), 0f, Random.Range (-20f, 20f));
				agent.SetDestination (this.transform.position + dest);
				idleTimer = 0;
			}
			break;
		case EntityState.Wander:
			if (Vector3.Distance (player.transform.position, this.transform.position) < 2) {
				Vector3 dest = new Vector3 (Random.Range (-20f, 20f), 0f, Random.Range (-20f, 20f));
				state = EntityState.Fleeing;
			}
			if (Vector3.Distance (this.transform.position, agent.destination) < 1) {
				Vector3 dest = new Vector3 (Random.Range (-20f, 20f), 0f, Random.Range (-20f, 20f));
				agent.SetDestination (this.transform.position + dest);
			}
			anim.SetBool ("IsWalking", true);
			wanderTimer++;

			if (wanderTimer > 600) {
				state = EntityState.Idle;
				agent.SetDestination (this.transform.position);
				wanderTimer = 0;
			}
			break;
		case EntityState.Fleeing:
			agent.speed = 4f;
			if (Vector3.Distance (this.transform.position, agent.destination) < 1 && Vector3.Distance (player.transform.position, this.transform.position) < 2) {
				Vector3 dest = new Vector3 (Random.Range (-10f, 10f), 0f, Random.Range (-10f, 10f));
				state = EntityState.Wander;
			}
			anim.SetBool ("IsWalking", true);
			break;
		case EntityState.Dead:
			this.transform.rotation = Quaternion.Euler(new Vector3 (0, 0, 90));
			anim.SetBool ("IsWalking", false);
			break;
		}
	}
}
