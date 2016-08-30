using UnityEngine;
using System.Collections;

public class TargetDestination : MonoBehaviour {

	private NavMeshAgent agent;
	public GameObject target;
	
	void Start () {
		agent = GetComponent<NavMeshAgent>();
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
