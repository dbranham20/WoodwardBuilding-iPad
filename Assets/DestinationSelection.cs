using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetDestination(GameObject button){
        NodeNavigation.destination = button.name;
    }
}
