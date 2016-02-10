using UnityEngine;
using System.Collections;

public class Waypoints : MonoBehaviour {
	private NavMeshAgent agent;
	public GameObject[] arrTargets;
	public int iRedirectDistance = 3;
	private int iCurrentDest = 0;
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		agent.destination = arrTargets[0].transform.position; 
	}
	void Update () {
		if(agent.remainingDistance < iRedirectDistance){
			int arrLength = arrTargets.Length;
			if(iCurrentDest >= arrLength-1){
				iCurrentDest = 0;
			}else{
				iCurrentDest++;
			}
			agent.destination = arrTargets[iCurrentDest].transform.position;
		}
	}
}
