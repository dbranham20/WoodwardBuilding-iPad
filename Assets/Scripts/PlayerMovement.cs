using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    GameObject ipadPlayer;
    //GameObject ARCamera;
    public GameObject iPadcamera;
    public GameObject player;
	// Use this for initialization
	void Start () 
    {
        //ipadPlayer = CustomNetworkManager
        //ipadPlayer = GameObject.FindGameObjectWithTag("Player"); // find gameobject with "player" tag
        //ARCamera = GameObject.FindGameObjectWithTag("ARCamera"); // find gameobject with "ARCamera" tag
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (CustomNetworkManager.singleton.IsClientConnected()){
            if (player == null)
            {
                //search for player and
                player = GameObject.FindGameObjectWithTag("Player");
            }else{
                print("Updating player coordinates with ARCamera coordinates");
                if (iPadcamera != null)
                {
                    player.transform.position = iPadcamera.transform.position;
                    player.transform.rotation = iPadcamera.transform.rotation;
                    print("In Player Movement class :\n player x: " + player.transform.position.x + "\nplayer y: " + player.transform.position.y + "\nplayer z: " + player.transform.position.z);

                }
                else
                {
                    print("Camera component not found. Deactivate the code which checks ARCamera to run in editor");
                }
            }
        }

        //if (ipadPlayer != null && ARCamera != null){
            
        //}
        //if(VuforiaSimulationManager.isSimulationOn){
            
        //}else{

        //    ipadPlayer.transform.position = ARCamera.transform.position;
        //    ipadPlayer.transform.rotation = ARCamera.transform.rotation;
        //}

	}

    public static void updatePlayerTransform(GameObject player){
       
    }
}
