using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VectorMath : MonoBehaviour {

    [SerializeField] GameObject ServerImageTarget;
    [SerializeField] GameObject PlayerImageTarget;
    public Vector3 cameraPosition, playerPosition;
    public Camera mainCamera;
    public GameObject realPlayer, virtualPlayer;
    [SerializeField] bool vuforiaTargetDetected = false;
    string lastImageTracked = "";
    Vector3 vuforiaImageTargetLocation = new Vector3(35f, 1f, 12f);
    Vector3 imageTargetInPlayerWorld = new Vector3(0f, 1f, 3f);
    Quaternion vuforiaImageTargetRotation = new Quaternion(0, -90, 0, 1);
    Vector3 cameraPositionWhenImageTracked;
    Quaternion cameraRotationWhenImageTracked;
    Vector3 recentPosition;
    Quaternion recentRotation;

    [SerializeField]Text playerX, playerY, playerZ, virtualX, virtualY, virtualZ;
    
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
        updateText();
	}

    private void updateText()
    {
        playerX.text = realPlayer.transform.localPosition.x.ToString();
        playerY.text = realPlayer.transform.localPosition.y.ToString();
        playerZ.text = realPlayer.transform.localPosition.z.ToString();


        virtualX.text = virtualPlayer.transform.localPosition.x.ToString();
        virtualY.text = virtualPlayer.transform.localPosition.y.ToString();
        virtualZ.text = virtualPlayer.transform.localPosition.z.ToString();
    }

    private void scanImageTarget()
    {
        if(vuforiaTargetDetected){
            vuforiaTargetDetected = false;
            lastImageTracked = "actualImageTarget";

            //TODO : Make a 4x4 Matrix. Initialize with Image targewt's location and rotation plus some offset to position the player.
            var m = Matrix4x4.TRS(imageTargetInPlayerWorld - realPlayer.transform.position , PlayerImageTarget.transform.rotation, Vector3.one);
            print(m);
            m = m.inverse;

            var pos = MatrixUtils.ExtractTranslationFromMatrix(ref m);

            virtualPlayer.transform.SetParent(ServerImageTarget.transform, false);
            virtualPlayer.transform.localPosition = pos;
            //Implementation part 1
            //cameraPositionWhenImageTracked = virtualPlayer.transform.position;
            //cameraRotationWhenImageTracked = virtualPlayer.transform.rotation;
            //based on which image target has been detected the offset will be different

            //Implementation way 2
            //virtualPlayer.transform.position = vuforiaImageTargetLocation;





            //NEW CODE 

            var targetFlatForward = -PlayerImageTarget.transform.up;
            targetFlatForward.y = 0;
            targetFlatForward.Normalize();
            var targQuat = Quaternion.LookRotation(targetFlatForward, Vector3.up);

            var cameraFlatForward = realPlayer.transform.forward;
            cameraFlatForward.y = 0;
            cameraFlatForward.Normalize();
            var camQuat = Quaternion.LookRotation(cameraFlatForward, Vector3.up);



            var flatAngle = Quaternion.Inverse(targQuat) * camQuat;
            virtualPlayer.transform.localRotation = flatAngle;
        }
    }

    private void updateVirtualPlayer()
    {
        //Vector3 currentPositionOffset = realPlayer.transform.position - recentPosition;

        //Calculating delta offsets/ tiny movements on each update
        //TODO: When we rotate a player 45 degree and then move only along its z axis, its only z axis value should be offsetted. Though we are using transform.local it is still relevant to world space coordinates
        //Vector3 deltaMovement = realPlayer.transform.localPosition - recentPosition;
        //Vector3 offset2 = realPlayer.transform.form
        //print(realPlayer.transform.forward.ToString());
        Quaternion currentRotationOffset2 = realPlayer.transform.localRotation * Quaternion.Inverse(recentRotation);
        //print("Moved this "+ offsetFfromSelfSpace.ToString() +" this time");
        //Quaternion currentRotationOffset = realPlayer.transform.rotation * Quaternion.Inverse(recentRotation);
        //recentPosition = realPlayer.transform.localPosition;
        recentRotation = realPlayer.transform.localRotation;
        //if (lastImageTracked == ""){
        //virtualPlayer.transform.Translate(deltaMovement, Space.Self);
        //}else{

        //Vector3 localWorldOffset = realPlayer.transform.forward + deltaMovement;
        //virtualPlayer.transform.Translate(movementMagnitude, Space.Self);
        //print("Magnitude of offset is "+ deltaMovement.magnitude);
        //}
        //if(offsetFfromSelfSpace.magnitude > 0){
        //    var something = Vector3.Scale(virtualPlayer.transform.forward, offsetFfromSelfSpace);
        //    print("that something is " + something);
        //}
        //print("Virtual player is been moved at "+ )
        virtualPlayer.transform.Rotate(currentRotationOffset2.eulerAngles, Space.Self);

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
