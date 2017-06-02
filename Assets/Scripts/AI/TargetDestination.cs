using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class TargetDestination : MonoBehaviour {

	private NavMeshAgent agent;
	public GameObject target;
	
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.destination = target.transform.position; 
	}
	
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			print("Mouse down");
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit)){
				agent.SetDestination(hit.point);
			}
			
		}
	}

}
