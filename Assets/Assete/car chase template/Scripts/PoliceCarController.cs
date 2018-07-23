using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PoliceCarController : MonoBehaviour {


	public NavMeshAgent agent;
	public bool stopIfGameOver = true;
	private GameObject player;
	private GameMasterLoop gameMasterLoop;
	// Use this for initialization
	void Start () {
		gameMasterLoop = GameObject.FindGameObjectWithTag("gamemasterloop").GetComponent<GameMasterLoop>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		else {
			if(gameMasterLoop.gameStarted && !gameMasterLoop.gameOver) {
				navigate();
			}
			if(gameMasterLoop.gameOver) {
				agent.Stop();
			}

		}
	}


	public void resetPlayerAI() {
		agent.Stop();
		agent.ResetPath();
		agent.Resume();
	}

	public void navigate() {
	
		agent.Resume();

		agent.SetDestination(player.transform.position);

		if (!agent.pathPending)
		{
			if (agent.remainingDistance <= agent.stoppingDistance)
			{
				if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
				{
						gameObject.transform.LookAt(player.transform.position);
						agent.Stop();
				}
			}
		}

	}
}
