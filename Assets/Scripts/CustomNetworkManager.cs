using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager {

    const int MaxClients = 10;
    public Text connectionText;
    public Text floorText;
    public static string imageTartgetDetected = "";
    protected static short messageID = 777;
    [SerializeField]public GameObject ARCamera, map, camManager;
    [SerializeField] public GameObject Building, FirstFloor, SecondFloor, ThirdFloor, FourthFloor;
    public GameObject playerObject;
    string hostName;
    string localIP;
    static public GameObject[] ClientRawGameObjects = new GameObject[MaxClients];
    BuildingFloors defaultBuildingLocation = new BuildingFloors();

    public class customMessage : MessageBase
    {
        public string deviceType, purpose, ipAddress;
        public Vector3 devicePosition;
        public Quaternion deviceRotation;

    }

    //enum FloorLevel{
    //    first, second, third, fourth, unknown
    //}

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

    public class ClientCoordinates : MessageBase
    {
        public Vector3[] positions;
        public Quaternion[] rotations;
        public int numberOfClients;

    }

    private void Start()
    {

        defaultBuildingLocation.firstFloorLocation = FirstFloor.transform.position;
        defaultBuildingLocation.secondFloorLocation = SecondFloor.transform.position;
        defaultBuildingLocation.thirdFloorLocation = ThirdFloor.transform.position;
        defaultBuildingLocation.fourthFloorLocation = FourthFloor.transform.position;
       
    }

    private void Update()
    {

        //Update the location if the connected with server
        if (this.IsClientConnected()) {
            if (playerObject == null){
                playerObject = GameObject.Instantiate(playerPrefab, new Vector3(5,1,-6), Quaternion.identity);
            }else{
                CreateMessageAndSend();
            }
        }
    }

    void CreateMessageAndSend()
    {
        var msg = new customMessage();

        msg.deviceType = "iPAD";
        msg.ipAddress = localIP;
        msg.purpose = "Simulation";
        msg.devicePosition = playerObject.transform.position;
        msg.deviceRotation = playerObject.transform.rotation;
        SendToServer(msg);
    }

    [SerializeField] float updateFrequency = 0.2f;
    float nextUpdate = 0.0f;
    private void SendToServer(customMessage msg)
    {
        //TODO:- Update frequency is set. Modify this function properly
        if(Time.time >= nextUpdate){
            nextUpdate = Time.time + updateFrequency;
            client.Send(messageID, msg);
        }

    }

    //TODO: See if this function can go out of network manager
    private void OnGUI()
    {
        //Buttons
        if (GUI.Button(new Rect(5, 30, 200, 75), "Connect to Server"))
            StartClient();
        
        if (GUI.Button(new Rect(10, 150, 200, 75), "Disconnect from Server"))
            StopClient();

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        //UI Task
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

        //TODO: Remove 778 handler once 800 is tested well.
        //this.client.RegisterHandler(778, OnReceivedMessage);
        this.client.RegisterHandler(800, OnReceivedCoordinates);
    }

    //Function call back when this client disconnects the server
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        connectionText.text = "Disconnected";
        connectionText.color = Color.red;
        Debug.Log("Disconnected from server " + conn.address);
    }

    //TODO: Not so important function. Can remove this implementation altogether
    void MessageToServer(string deviceConnecting)
    {
        var sendMSG = new customMessage();
        sendMSG.deviceType = deviceConnecting; // adding values to message class
        sendMSG.devicePosition = new Vector3(16.5f, 0.5f, 59);
        client.Send(messageID, sendMSG); // send message
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
            if (ClientRawGameObjects[0] == null)
            {
                localPlayer = (GameObject)Instantiate(playerObject, new GameObject().transform, true);
                ClientRawGameObjects[0] = localPlayer;
            }
            else
            {
                localPlayer = ClientRawGameObjects[0];
            }
            localPlayer.transform.localPosition = msg.devicePosition1;
            localPlayer.transform.localRotation = msg.deviceRotation1;

            ClientRawGameObjects[0] = localPlayer;
        }
        if (msg.devicePosition2 != Vector3.zero && msg.deviceRotation2 != Quaternion.identity)
        {
            print("Second player is added");
            GameObject localPlayer;
            if (ClientRawGameObjects[1] == null)
            {
                localPlayer = (GameObject)Instantiate(playerObject, new GameObject().transform, true);
                ClientRawGameObjects[1] = localPlayer;
            }
            else
            {
                localPlayer = ClientRawGameObjects[1];
            }
            localPlayer.transform.position = msg.devicePosition2;
            localPlayer.transform.rotation = msg.deviceRotation2;
            //localPlayer.transform.SetParent(WorldMap.transform, true);
            ClientRawGameObjects[1] = localPlayer;
        }
        if (msg.devicePosition3 != Vector3.zero && msg.deviceRotation3 != Quaternion.identity)
        {
            print("Second player is added");
            GameObject localPlayer;
            if (ClientRawGameObjects[2] == null)
            {
                localPlayer = (GameObject)Instantiate(playerObject, new GameObject().transform, true);
                ClientRawGameObjects[2] = localPlayer;
            }
            else
            {
                localPlayer = ClientRawGameObjects[2];
            }
            localPlayer.transform.position = msg.devicePosition3;
            localPlayer.transform.rotation = msg.deviceRotation3;
            //localPlayer.transform.SetParent(WorldMap.transform, true);
            ClientRawGameObjects[2] = localPlayer;
        }
        if (msg.devicePosition4 != Vector3.zero && msg.deviceRotation4 != Quaternion.identity)
        {
            GameObject localPlayer = ClientRawGameObjects[3];
            localPlayer.transform.position = msg.devicePosition4;
            localPlayer.transform.rotation = msg.deviceRotation4;
            ClientRawGameObjects[3] = localPlayer;
        }
        if (msg.devicePosition5 != Vector3.zero && msg.deviceRotation5 != Quaternion.identity)
        {
            GameObject localPlayer = ClientRawGameObjects[4];
            localPlayer.transform.position = msg.devicePosition5;
            localPlayer.transform.rotation = msg.deviceRotation5;
            ClientRawGameObjects[4] = localPlayer;
        }
        if (msg.devicePosition6 != Vector3.zero && msg.deviceRotation6 != Quaternion.identity)
        {
            GameObject localPlayer = ClientRawGameObjects[5];
            localPlayer.transform.position = msg.devicePosition6;
            localPlayer.transform.rotation = msg.deviceRotation6;
            ClientRawGameObjects[5] = localPlayer;
        }
        if (msg.devicePosition7 != Vector3.zero && msg.deviceRotation7 != Quaternion.identity)
        {
            GameObject localPlayer = ClientRawGameObjects[6];
            localPlayer.transform.position = msg.devicePosition7;
            localPlayer.transform.rotation = msg.deviceRotation7;
            ClientRawGameObjects[6] = localPlayer;
        }
        if (msg.devicePosition8 != Vector3.zero && msg.deviceRotation8 != Quaternion.identity)
        {
            GameObject localPlayer = ClientRawGameObjects[7];
            localPlayer.transform.position = msg.devicePosition8;
            localPlayer.transform.rotation = msg.deviceRotation8;
            ClientRawGameObjects[7] = localPlayer;
        }

        //clientsRawData = msg.clients;
    }



    protected void OnReceivedCoordinates(NetworkMessage netMsg)
    {
        print("Received location updates from server");
        var msg = netMsg.ReadMessage<ClientCoordinates>();

        //Scan through the client coordinate response one by one and check if there are any updates. If so update the list of clients on the client side.
        for (int i = 0; i < MaxClients ; i ++){
            Vector3 playerPosition = msg.positions[i];
            Quaternion playerRotation = msg.rotations[i];

            if (playerPosition != Vector3.zero && playerRotation != Quaternion.identity)
            {
                GameObject localPlayer;
                if (ClientRawGameObjects[i] == null)
                {
                    localPlayer = (GameObject)Instantiate(playerObject, new GameObject().transform, true);
                    ClientRawGameObjects[i] = localPlayer;
                }
                else
                {
                    localPlayer = ClientRawGameObjects[i];
                }



                //Determine the floor
                FloorLevel floor = DetermineFloor(playerPosition);

                //Select floor model as a parent
                GameObject parent = Building;
                parent = SelectParent(floor);

                //Get position based on floor's location in the world
                Vector3 floorOffset = GetFloorOffsetForFloor(floor);

                Vector3 localizedPosition = playerPosition - floorOffset;
                localPlayer.transform.SetParent(parent.transform, false);
                //Set the local players parent as the floor model
                // Make transformation based on where the floor currently is
                // assign the localposition as required
                localPlayer.transform.localPosition = localizedPosition;
                localPlayer.transform.localRotation = playerRotation;
                ClientRawGameObjects[i] = localPlayer;

            }
            else
            { //TODO: Have to handle else condition
                ClientRawGameObjects[i] = null;
            }


        }
    }

    private Vector3 GetFloorOffsetForFloor(FloorLevel floorLevel)
    {
        Vector3 offset = Vector3.zero;
        switch (floorLevel){
            case FloorLevel.first:
                offset = defaultBuildingLocation.firstFloorLocation;
                break;
            case FloorLevel.second:
                offset = defaultBuildingLocation.secondFloorLocation;
                break;
            case FloorLevel.third:
                offset = defaultBuildingLocation.thirdFloorLocation;
                break;
            case FloorLevel.fourth:
                offset = defaultBuildingLocation.fourthFloorLocation;
                break;
            case FloorLevel.unknown:
                break;
        }

        return offset;
    }

    //TODO: THe function to get floor value is getting redundant in many classes. Try to optimize it.
    private GameObject SelectParent(FloorLevel floor)
    {
        GameObject parent;
        switch (floor)
        {
            case FloorLevel.first:
                {
                    parent = FirstFloor;
                    break;
                }

            case FloorLevel.second:
                {
                    parent = SecondFloor;
                    break;
                }

            case FloorLevel.third:
                {
                    parent = ThirdFloor;
                    break;
                }

            case FloorLevel.fourth:
                {
                    parent = FourthFloor;
                    break;
                }

            case FloorLevel.unknown:
                {
                    parent = Building;
                    break;
                }

            default:
                {
                    parent = Building;
                    print("Going in default case...");
                    break;
                }
        }

        return parent;
    }

    private FloorLevel DetermineFloor(Vector3 playerPosition)
    {
        //TODO: Determine the floor based on y axis.
        FloorLevel floor = FloorLevel.unknown;
        // +2 ... -1 -> Fourth Floor
        // -1... -4 -> Third Floor
        // -4 ... -8 -> Second floor
        // -8 ... -15 > First Floor
        var yPosition = playerPosition.y;
        if (yPosition > -1 && yPosition <= 4) { floor = FloorLevel.fourth; }
        else if (yPosition > -4 && yPosition <= -1) { floor = FloorLevel.third; }
        else if (yPosition > -8 && yPosition <= -4) { floor = FloorLevel.second; }
        else if (yPosition > -15 && yPosition <= -8) { floor = FloorLevel.first; }
        else { floor = FloorLevel.unknown; }
        floorText.text = floor.ToString();
        return floor;
    }

    private void HideLocalPlayer()
    {
        if (playerObject.GetComponent<Renderer>().enabled == true){
            playerObject.GetComponent<Renderer>().enabled = false;
        }
    }
}
