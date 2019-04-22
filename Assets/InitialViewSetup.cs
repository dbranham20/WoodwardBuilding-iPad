using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialViewSetup : MonoBehaviour {

    [SerializeField] public GameObject ARCamera, topCamera, map;
    // Use this for initializatio
    void Start () {
        StartTopDown();

    }

    void StartTopDown()
    {
        if (!topCamera.activeSelf) // turn on top down camera
            topCamera.SetActive(true);

        if (ARCamera.activeSelf) // turn off ar camera
            ARCamera.GetComponent<Camera>().enabled = true;

        if (!map.activeSelf) // turn on map
            map.SetActive(true);

    }


}
