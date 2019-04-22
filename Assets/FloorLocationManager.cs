using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLocationManager : MonoBehaviour {

    [SerializeField] GameObject firstFloorModel, secondFloorModel, thirdFloorModel, fourthFloorModel;

    [SerializeField] Transform defaultCameraParent, topDownCameraParent, stairCaseCameraParent;

    [SerializeField] Camera TheCamera;

    public BuildingFloors defautViz = new BuildingFloors();
    BuildingFloors stairCaseViz = new BuildingFloors();
    BuildingFloors topDistinctViz = new BuildingFloors();

    private void Awake()
    {
        //Set defult Viz Location
        defautViz.firstFloorLocation = new Vector3(0f, -12.97f, -7.85f);
        defautViz.secondFloorLocation = new Vector3(0f, -8.56f, -7.85f);
        defautViz.thirdFloorLocation = new Vector3(0f, -4.1f, -7.9f);
        defautViz.fourthFloorLocation = new Vector3(0f, 0f, 0f);

        //Set staircase Viz Location
        stairCaseViz.firstFloorLocation = new Vector3(30.1f, 5.2f, -7.85f);
        stairCaseViz.secondFloorLocation = new Vector3(-6.9f, 14.1f, -7.85f);
        stairCaseViz.thirdFloorLocation = new Vector3(-34.8f, 25.3f, -7.9f);
        stairCaseViz.fourthFloorLocation = new Vector3(-54.8f, 36.8f, 0f);

        //Set top Viz Location
        topDistinctViz.firstFloorLocation = new Vector3(126.3f, 105.94f, 30.7f);
        topDistinctViz.secondFloorLocation = new Vector3(70.6f, 105.94f, 30.7f);
        topDistinctViz.thirdFloorLocation = new Vector3(14.3f, 105.94f, 30.7f);
        topDistinctViz.fourthFloorLocation = new Vector3(-34.8f, 105.94f, 37.7f);

    }
    // Use this for initialization
    void Start () {

       

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void setDefaultVizualization(){
        firstFloorModel.transform.position = defautViz.firstFloorLocation;
        secondFloorModel.transform.position = defautViz.secondFloorLocation;
        thirdFloorModel.transform.position = defautViz.thirdFloorLocation;
        fourthFloorModel.transform.position = defautViz.fourthFloorLocation;

        TheCamera.transform.SetParent(defaultCameraParent);
        TheCamera.transform.localPosition = Vector3.zero;
        TheCamera.transform.localRotation = Quaternion.identity;
        RefreshPath();
    }

    public void setStairCaseVisualization()
    {
        firstFloorModel.transform.position = stairCaseViz.firstFloorLocation;
        secondFloorModel.transform.position = stairCaseViz.secondFloorLocation;
        thirdFloorModel.transform.position = stairCaseViz.thirdFloorLocation;
        fourthFloorModel.transform.position = stairCaseViz.fourthFloorLocation;

        TheCamera.transform.SetParent(stairCaseCameraParent);
        TheCamera.transform.localPosition = Vector3.zero;
        TheCamera.transform.localRotation = Quaternion.identity;
        RefreshPath();
    }

    public void topDownVisualization()
    {
        firstFloorModel.transform.position = topDistinctViz.firstFloorLocation;
        secondFloorModel.transform.position = topDistinctViz.secondFloorLocation;
        thirdFloorModel.transform.position = topDistinctViz.thirdFloorLocation;
        fourthFloorModel.transform.position = topDistinctViz.fourthFloorLocation;

        TheCamera.transform.SetParent(topDownCameraParent);
        TheCamera.transform.localPosition = Vector3.zero;
        TheCamera.transform.localRotation = Quaternion.identity;
        RefreshPath();
    }

    void RefreshPath(){
        NodeNavigation navigationObject = FindObjectOfType<NodeNavigation>();
        navigationObject.reDrawPath();
    }
}

public class BuildingFloors{

    public Vector3 firstFloorLocation, secondFloorLocation, thirdFloorLocation, fourthFloorLocation;

}
