using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintMyLocalPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        print("x :  " + transform.localPosition.x + ", y : " + transform.localPosition.y + ", z is : " + transform.localPosition.z);
	}
}
