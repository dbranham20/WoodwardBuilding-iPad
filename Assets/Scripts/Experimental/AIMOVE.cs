using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMOVE : MonoBehaviour {

    [SerializeField]

    Transform destination;

    NavMeshAgent _navMeshAgent;

	// Use this for initialization
	void Start () {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if(_navMeshAgent == null){
            print("error");
        }else{
            SetDestination();
        }
    }

    private void SetDestination()
    {
        if(destination != null){
            Vector3 targetVector = destination.transform.position;
            _navMeshAgent.SetDestination(targetVector);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
