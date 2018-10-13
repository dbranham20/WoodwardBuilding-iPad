using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMath : MonoBehaviour {

    public Vector3 cameraPosition, playerPosition;
    public Camera mainCamera;
    public GameObject realPlayer, virtualPlayer;
    [SerializeField] bool vuforiaTargetDetected = false;
    string lastImageTracked = "";
    Vector3 vuforiaImageTargetLocation = new Vector3(35f, 1f, 12f);
    Quaternion vuforiaImageTargetRotation = new Quaternion(0, -90, 0, 1);
    Vector3 cameraPositionWhenImageTracked;
    Quaternion cameraRotationWhenImageTracked;
    Vector3 recentPosition;
    //Quaternion vuforiaImageTargetRotation = new Quaternion();

    // Use this for initialization
    void Start () {
        recentPosition = virtualPlayer.transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        scanImageTarget();
        //move virtual player based on real player's movement
        updateVirtualPlayer();
	}

    private void scanImageTarget()
    {
        if(vuforiaTargetDetected){
            vuforiaTargetDetected = false;
            lastImageTracked = "actualImageTarget";
            //Implementation part 1
            //cameraPositionWhenImageTracked = virtualPlayer.transform.position;
            //cameraRotationWhenImageTracked = virtualPlayer.transform.rotation;
            //based on which image target has been detected the offset will be different

            //Implementation way 2
            virtualPlayer.transform.position = vuforiaImageTargetLocation;
            virtualPlayer.transform.rotation = vuforiaImageTargetRotation;

        }
    }

    private void updateVirtualPlayer()
    {
        Vector3 currentOffset = realPlayer.transform.position - recentPosition;
        recentPosition = realPlayer.transform.position;
        virtualPlayer.transform.Translate(currentOffset, Space.Self);
        //if (lastImageTracked == ""){
        //    //virtualPlayer.transform.position = realPlayer.transform.position;

        //    virtualPlayer.transform.rotation = realPlayer.transform.rotation;
        //}else{

        //    virtualPlayer.transform.position = vuforiaImageTargetLocation - cameraPositionWhenImageTracked + realPlayer.transform.position;
        //    //virtualPlayer.transform.Translate()
        //    virtualPlayer.transform.rotation = (vuforiaImageTargetRotation * Quaternion.Inverse(cameraRotationWhenImageTracked)) * realPlayer.transform.rotation;
        //}

        //virtualPlayer.transform.rotation = realPlayer.transform.rotation;
    }
}
