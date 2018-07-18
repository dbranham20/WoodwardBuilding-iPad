using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager {
    
    public Text connectionText;
    protected static short messageID = 777;
    public GameObject ARCamera, topCamera, map, camManager;

    public class customMessage : MessageBase
    {
        public string deviceType;
        public Vector3 devicePosition;
    }

    private void Start()
    {
        StartTopDown(); 
    }

    private void OnGUI()
    {
        //Buttons
        if (GUI.Button(new Rect(5, 30, 200, 75), "Connect to Server"))
            StartClient();
        
        if (GUI.Button(new Rect(10, 150, 200, 75), "Disconnect from Server"))
            StopClient();
        
        if (GUI.Button(new Rect(10, 270, 200, 75), "Top Down"))
            StartTopDown();
        
        if (GUI.Button(new Rect(10, 390, 200, 75), "AR Camera"))
            StartARCamera();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        connectionText.text = "Connected";
        connectionText.color = Color.green;
        Debug.Log("Connected to server " + conn.address + " with ID " + conn.connectionId);

        MessageToServer("iPad");
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
    
        if(ARCamera.activeSelf) // turn off ar camera
            ARCamera.SetActive(false);

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
