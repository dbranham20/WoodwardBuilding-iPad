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

    public class customMessage : MessageBase
    {
        public string deviceType, purpose, ipAddress;
        public Vector3 devicePosition;
        public Quaternion deviceRotation;
    }

    private void Start()
    {
        StartTopDown(); 
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
            print("didn't go in is client connected condition");
        }
    }

     void updateLocation()
    {
        var msg = new customMessage();
        msg.deviceType = "iPAD";
        //if(VuforiaSimulationManager.isSimulationOn){
        //    msg.purpose = "Simulation";
        //    msg.ipAddress = localIP;
        //}else{
        //    msg.purpose = "Yet to decide";
        //}
        msg.ipAddress = localIP;
        msg.purpose = "Simulation";
        msg.devicePosition = playerObject.transform.position;
        msg.deviceRotation = playerObject.transform.rotation;
        print("Printing ip address sending in object: " + msg.ipAddress);
        client.Send(messageID, msg);
        print("Client sent message..");
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

        if (camManager.activeSelf) // turn off ar camera manager
            camManager.SetActive(false);
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


   
}
