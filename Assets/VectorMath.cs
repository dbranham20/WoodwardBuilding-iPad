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
    public GameObject realPlayer, virtualPlayer, thirdRealPlayer, thirdVirtualPlayer;
    [SerializeField] bool vuforiaTargetDetected = false;
    string lastImageTracked = "";
    


    [SerializeField]Text playerX, playerY, playerZ, virtualX, virtualY, virtualZ;

    // Use this for initialization
    void Start () {
       
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
        if(vuforiaTargetDetected)
        {
            vuforiaTargetDetected = false;
            lastImageTracked = "actualImageTarget";
        }
    }

    private void UpdatePlayerPositionAndRotation()
    {
        if(lastImageTracked != "")
        {
            RepositionThePlayer();
            RotateThePlayer();
            GetServerPlayer();
        }


    }

    private void GetServerPlayer()
    {
        //TODO: Use RealPlayer coordinates (in server world space) and make it relative to player.
        //Code from Elias
        var m = Matrix4x4.TRS(ServerImageTarget.transform.position - thirdRealPlayer.transform.position, ServerImageTarget.transform.rotation, Vector3.one);
        m = m.inverse;

        //Positioning the player
        var pos = MatrixUtils.ExtractTranslationFromMatrix(ref m);
        thirdVirtualPlayer.SetActive(true);
        thirdVirtualPlayer.transform.position = pos + PlayerImageTarget.transform.position;

    }

    private void RepositionThePlayer()
    {
        //TODO: GetThe Player Image Target which was last recognized in imageTargetInPlayerWorld
        //Code from Elias
        var m = Matrix4x4.TRS(PlayerImageTarget.transform.position - realPlayer.transform.position, PlayerImageTarget.transform.rotation, Vector3.one);
        m = m.inverse;

        //Positioning the player

        var pos = MatrixUtils.ExtractTranslationFromMatrix(ref m);
        //Setting the virtual player as a child of Image Target (for server)
        virtualPlayer.transform.SetParent(ServerImageTarget.transform, false);
        //Assigning local position to virtual player with rewspect to Image Target
        virtualPlayer.transform.localPosition = pos;
    }

    private void RotateThePlayer()
    {
        //Rotating the player
        //Code from Elias
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

    private void updateVirtualPlayer()
    {
        UpdatePlayerPositionAndRotation();
    }

   
}
