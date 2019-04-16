using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationUpdateManager : MonoBehaviour {

    public Camera mainCamera;
    public Text xLocation;
    public Text yLocation;
    public Text zLocation;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (mainCamera != null){
            xLocation.text = Camera.main.transform.position.x.ToString();
            yLocation.text = Camera.main.transform.position.y.ToString();
            zLocation.text = Camera.main.transform.position.z.ToString();
        }
	}
}
