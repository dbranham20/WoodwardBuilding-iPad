using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLocationManager : MonoBehaviour {

    // Next update in second
    Dictionary<int, GameObject> localPlayers = new Dictionary<int, GameObject>();
    [SerializeField] public GameObject ServerImageTarget;
    private int nextUpdate = 1;
    void Update()
    {

        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            UpdateEverySecond();
        }

    }

    // Update is called once per second
    void UpdateEverySecond()
    {

        if (CustomNetworkManager.clientsRawData.Length > 0 && PlayerMovement.lastImageTracked != "")
        {
            int id = 0;
            //One by one, calculate each clients location and update the gameobject
            foreach (GameObject player in CustomNetworkManager.clientsRawData)
            {
                if(player != null){
                    updatePlayerTransform(player, id);
                    print("ID is incremented to " + id);
                    id++;
                }

            }
        }

    }


    private void updatePlayerTransform(GameObject player, int id)
    {
        //if player for that id exist, then send it to update.
        print("right before both");
        if(localPlayers.ContainsKey(id)){
            print("Inside IF");
            GameObject localPlayerObject = localPlayers[id];
            localPlayerObject.SetActive(true);
            RepositionServerObject(player, localPlayerObject);
        }else{
            print("Inside ELSE");
            GameObject localPlayer = GameObject.CreatePrimitive(PrimitiveType.Cube);
            localPlayer.SetActive(false);
            localPlayers[id] = localPlayer;
        }
        //else initialize a new gamepbject with default lcoationa dn rotation.

        //RepositionServerObject
    }

    private void RotateServerObjects(GameObject playerFromServer, GameObject localPlayer)
    {
        //Rotating the player
        //Code from Elias
        var targetFlatForward = -ServerImageTarget.transform.up;

        targetFlatForward.y = 0;
        targetFlatForward.Normalize();
        var targQuat = Quaternion.LookRotation(targetFlatForward, Vector3.up);

        var cameraFlatForward = playerFromServer.transform.forward;
        cameraFlatForward.y = 0;
        cameraFlatForward.Normalize();
        var camQuat = Quaternion.LookRotation(cameraFlatForward, Vector3.up);

        var flatAngle = Quaternion.Inverse(targQuat) * camQuat;
        localPlayer.transform.localRotation = flatAngle;
    }

    private void RepositionServerObject(GameObject playerFromServer, GameObject localPlayer)
    {
        //Code from Elias
        var m = Matrix4x4.TRS(ServerImageTarget.transform.position - playerFromServer.transform.position, ServerImageTarget.transform.rotation, Vector3.one);
        m = m.inverse;

        //Positioning the player
        var pos = MatrixUtils.ExtractTranslationFromMatrix(ref m);
        //localPlayer.SetActive(true);
        localPlayer.transform.position = pos + PlayerMovement.PlayerImageTarget.transform.position;
    }
}
