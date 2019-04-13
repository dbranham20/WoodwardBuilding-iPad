using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager {
    
    public Text connectionText;
    public static string imageTartgetDetected = "";
    protected static short messageID = 777;
    [SerializeField]public GameObject ARCamera, topCamera, map, camManager;
    public GameObject playerObject;
    string hostName;
    string localIP;
    static public GameObject[] clientsRawData = new GameObject[8];
    float[] floatArray = new float[10000];

    public class customMessage : MessageBase
    {
        public string deviceType, purpose, ipAddress;
        public Vector3 devicePosition;
        public Quaternion deviceRotation;
        public float[] hugeArrayOfFloats;
    }

    public class ClientLocations : MessageBase
    {
        public Vector3 devicePosition1;
        public Quaternion deviceRotation1;
        public Vector3 devicePosition2;
        public Quaternion deviceRotation2;
        public Vector3 devicePosition3;
        public Quaternion deviceRotation3;
        public Vector3 devicePosition4;
        public Quaternion deviceRotation4;
        public Vector3 devicePosition5;
        public Quaternion deviceRotation5;
        public Vector3 devicePosition6;
        public Quaternion deviceRotation6;
        public Vector3 devicePosition7;
        public Quaternion deviceRotation7;
        public Vector3 devicePosition8;
        public Quaternion deviceRotation8;

    }

    private void Start()
    {
        StartTopDown();

        CreateAndSetFloatArrayToSend();
    }

    private void CreateAndSetFloatArrayToSend()
    {

        for (int i = 0; i < 9999; i++)
        {
            floatArray[i] = i;
        }
    }

    private void Update()
    {
        if (this.IsClientConnected()) {
            if (playerObject == null){
                
                playerObject = GameObject.Instantiate(playerPrefab, new Vector3(5,1,-6), Quaternion.identity);
            }else{
                //if(VuforiaSimulationManager.isSimulationOn){
                    //PlayerMovement.updatePlayerTransform(playerObject);
                    updateLocation();
                //}
            }
        }else{
           // print("didn't go in is client connected condition");
        }
    }

     void updateLocation()
    {
        var msg = new customMessage();
        msg.deviceType = "iPAD";

        msg.ipAddress = localIP;
        msg.purpose = "Simulation";
        msg.devicePosition = playerObject.transform.position;
        msg.deviceRotation = playerObject.transform.rotation;
        msg.hugeArrayOfFloats = floatArray;
        print("Printing ip address sending in object: " + msg.ipAddress);
        updateLocationToServer(msg);
        print("Client sent message..");
    }

    [SerializeField] float updateFrequency = 0.2f;
    float nextUpdate = 0.0f;
    private void updateLocationToServer(customMessage msg)
    {
        if(Time.time >= nextUpdate){
            nextUpdate = Time.time + updateFrequency;
            client.Send(messageID, msg);
        }

    }

    private void OnGUI()
    {
        //Buttons
        if (GUI.Button(new Rect(5, 30, 200, 75), "Connect to Server"))
            StartClient();
        
        if (GUI.Button(new Rect(10, 150, 200, 75), "Disconnect from Server"))
            StopClient();
        
        //if (GUI.Button(new Rect(10, 270, 200, 75), "Top Down"))
        //    StartTopDown();
        
        //if (GUI.Button(new Rect(10, 390, 200, 75), "AR Camera"))
            //StartARCamera();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        
        Debug.Log("connected to server. Connection address is "+conn.address.ToString());
        base.OnClientConnect(conn);

        print("local ip of this machine is " + localIP);
        connectionText.text = "Connected";
        connectionText.color = Color.green;
        Debug.Log("Connected to server " + conn.address + " with ID " + conn.connectionId);

        MessageToServer("iPad");

        this.hostName = System.Net.Dns.GetHostName();
        this.localIP = "::ffff:"+System.Net.Dns.GetHostEntry(hostName).AddressList[0].ToString();

        //TODO: Register event handler to server to client communication

        this.client.RegisterHandler(778, OnReceivedMessage);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        connectionText.text = "Disconnected";
        connectionText.color = Color.red;
        Debug.Log("Disconnected from server " + conn.address);
    }

    void MessageToServer(string deviceConnecting)
    {
        var sendMSG = new customMessage();
        sendMSG.deviceType = deviceConnecting; // adding values to message class
        sendMSG.devicePosition = new Vector3(16.5f, 0.5f, 59);

        client.Send(messageID, sendMSG); // send message
    }

    void StartTopDown()
    {
        if(!topCamera.activeSelf) // turn on top down camera
            topCamera.SetActive(true);

        if (ARCamera.activeSelf) // turn off ar camera
            //ARCamera.SetActive(false);
            ARCamera.GetComponent<Camera>().enabled = true;

        if (!map.activeSelf) // turn on map
            map.SetActive(true);

        //if (camManager.activeSelf) // turn off ar camera manager
            //camManager.SetActive(false);
    }

    void StartARCamera()
    {
        if (topCamera.activeSelf) // turn off top down camera
            topCamera.SetActive(false);
        
        if (!ARCamera.activeSelf) // turn on ar camera
            ARCamera.SetActive(true);
        
        if (map.activeSelf) // turn off map
            map.SetActive(false);

        if (!camManager.activeSelf) // turn on camera manager
            camManager.SetActive(true);


    }

    //TODO: make this function more dynamic and a starting point to switch to multiple type of operations that the device woudl perform on server's command.
    protected void OnReceivedMessage(NetworkMessage netMsg)
    {
        //TODO: Disable self player renderer. Hide self player oject which is manupulated locally. Show the contents being received from server.
        //HideLocalPlayer();

        var msg = netMsg.ReadMessage<ClientLocations>();
        print("Message recieved from server");

        if (msg.devicePosition1 != Vector3.zero && msg.deviceRotation1 != Quaternion.identity)
        {
            GameObject localPlayer;
            if (clientsRawData[0] == null)
            {
                localPlayer = (GameObject)Instantiate(playerObject, new GameObject().transform, true);
                clientsRawData[0] = localPlayer;
            }
            else
            {
                localPlayer = clientsRawData[0];
            }
            localPlayer.transform.localPosition = msg.devicePosition1;
            localPlayer.transform.localRotation = msg.deviceRotation1;

            clientsRawData[0] = localPlayer;
        }
        if (msg.devicePosition2 != Vector3.zero && msg.deviceRotation2 != Quaternion.identity)
        {
            print("Second player is added");
            GameObject localPlayer;
            if (clientsRawData[1] == null)
            {
                localPlayer = (GameObject)Instantiate(playerObject, new GameObject().transform, true);
                clientsRawData[1] = localPlayer;
            }
            else
            {
                localPlayer = clientsRawData[1];
            }
            localPlayer.transform.position = msg.devicePosition2;
            localPlayer.transform.rotation = msg.deviceRotation2;
            //localPlayer.transform.SetParent(WorldMap.transform, true);
            clientsRawData[1] = localPlayer;
        }
        if (msg.devicePosition3 != Vector3.zero && msg.deviceRotation3 != Quaternion.identity)
        {
            print("Second player is added");
            GameObject localPlayer;
            if (clientsRawData[2] == null)
            {
                localPlayer = (GameObject)Instantiate(playerObject, new GameObject().transform, true);
                clientsRawData[2] = localPlayer;
            }
            else
            {
                localPlayer = clientsRawData[2];
            }
            localPlayer.transform.position = msg.devicePosition3;
            localPlayer.transform.rotation = msg.deviceRotation3;
            //localPlayer.transform.SetParent(WorldMap.transform, true);
            clientsRawData[2] = localPlayer;
        }
        if (msg.devicePosition4 != Vector3.zero && msg.deviceRotation4 != Quaternion.identity)
        {
            GameObject localPlayer = clientsRawData[3];
            localPlayer.transform.position = msg.devicePosition4;
            localPlayer.transform.rotation = msg.deviceRotation4;
            clientsRawData[3] = localPlayer;
        }
        if (msg.devicePosition5 != Vector3.zero && msg.deviceRotation5 != Quaternion.identity)
        {
            GameObject localPlayer = clientsRawData[4];
            localPlayer.transform.position = msg.devicePosition5;
            localPlayer.transform.rotation = msg.deviceRotation5;
            clientsRawData[4] = localPlayer;
        }
        if (msg.devicePosition6 != Vector3.zero && msg.deviceRotation6 != Quaternion.identity)
        {
            GameObject localPlayer = clientsRawData[5];
            localPlayer.transform.position = msg.devicePosition6;
            localPlayer.transform.rotation = msg.deviceRotation6;
            clientsRawData[5] = localPlayer;
        }
        if (msg.devicePosition7 != Vector3.zero && msg.deviceRotation7 != Quaternion.identity)
        {
            GameObject localPlayer = clientsRawData[6];
            localPlayer.transform.position = msg.devicePosition7;
            localPlayer.transform.rotation = msg.deviceRotation7;
            clientsRawData[6] = localPlayer;
        }
        if (msg.devicePosition8 != Vector3.zero && msg.deviceRotation8 != Quaternion.identity)
        {
            GameObject localPlayer = clientsRawData[7];
            localPlayer.transform.position = msg.devicePosition8;
            localPlayer.transform.rotation = msg.deviceRotation8;
            clientsRawData[7] = localPlayer;
        }

        //clientsRawData = msg.clients;
    }

    private void HideLocalPlayer()
    {
        if (playerObject.GetComponent<Renderer>().enabled == true){
            playerObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
