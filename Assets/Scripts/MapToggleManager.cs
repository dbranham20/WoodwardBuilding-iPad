using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToggleManager : MonoBehaviour {

    bool isMapActive = false;
    [SerializeField]GameObject worldMap;

    public void toggleMap(){
        if (isMapActive){
            isMapActive = false;
            worldMap.SetActive(false);
        }else{
            isMapActive = true;
            worldMap.SetActive(true);
        }
    }
}
