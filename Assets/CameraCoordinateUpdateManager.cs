using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraCoordinateUpdateManager : MonoBehaviour {


    [SerializeField]Text xCoordinate, yCoordinate, zCoordinate;
    [SerializeField] Camera deviceCamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Vuforia.CameraDevice.Instance.GetCameraFields().ToString();
        xCoordinate.text = deviceCamera.transform.position.x.ToString();
        yCoordinate.text = deviceCamera.transform.position.y.ToString();
        zCoordinate.text = deviceCamera.transform.position.z.ToString();
    }
}
