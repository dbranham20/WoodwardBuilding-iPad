using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    
    GameObject ipadPlayer;
    //GameObject ARCamera;
    public GameObject iPadcamera;
    public GameObject player;
    Vector3 playerOffset = Vector3.zero;
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
        if (CustomNetworkManager.imageTartgetDetected == ""){
            print("In Arbitrary location");

        }else{
            print("Target found. Offset is being calculated...");
            //todo get image target name and take its transform origin and add to player's movement.
            GameObject imageTarget = GameObject.FindWithTag("Room1");
            playerOffset = new Vector3(6.5f,0.005f,-7.5f);
        }


        if (CustomNetworkManager.singleton.IsClientConnected()){
            if (player == null)
            {
                //search for player and
                player = GameObject.FindGameObjectWithTag("Player");
            }else{
                print("Updating player coordinates with ARCamera coordinates");
                if (iPadcamera != null)
                {
                    print("Camera coordinates:" +
                          "\n x : " + iPadcamera.transform.position.x
                          + "\n y : " + iPadcamera.transform.position.y
                          + "\n z: " + iPadcamera.transform.position.z);

                    print("Offset is " + playerOffset.x + ", " + playerOffset.y + ", " + playerOffset.z + "**********");

                    player.transform.position = iPadcamera.transform.position + playerOffset;
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
