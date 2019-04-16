using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script to toggle between AR and 2D mode in iPad
public class ViewToggleManager : MonoBehaviour {

    [SerializeField]Camera topViewCamera;
    [SerializeField]Camera ARViewCamera;
    static public bool isARViewActive = false;
	// Use this for initialization
	void Start () {
        if (ARViewCamera.depth > topViewCamera.depth){
            isARViewActive = true;
        }
	}
	
    public void toggleViewMode(){
        if(isARViewActive){
            isARViewActive = false;
            topViewCamera.depth = 1;
            topViewCamera.enabled = true;
            ARViewCamera.depth = 0.8f;
        }else{
            isARViewActive = true;
            topViewCamera.depth = 0.8f;
            ARViewCamera.depth = 1;
        }
    }
}
