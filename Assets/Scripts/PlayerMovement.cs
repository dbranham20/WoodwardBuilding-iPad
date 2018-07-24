using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    GameObject ipadPlayer;
    GameObject ARCamera;

	// Use this for initialization
	void Start () 
    {
        ipadPlayer = GameObject.FindGameObjectWithTag("Player"); // find gameobject with "player" tag
        ARCamera = GameObject.FindGameObjectWithTag("ARCamera"); // find gameobject with "ARCamera" tag
	}
	
	// Update is called once per frame
	void Update () 
    {
        ipadPlayer.transform.position = ARCamera.transform.position;
        ipadPlayer.transform.rotation = ARCamera.transform.rotation;
	}
}
