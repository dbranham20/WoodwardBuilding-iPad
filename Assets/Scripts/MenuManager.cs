using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GameObject destinationPanel;
    public GameObject MainViewPanel;

    public void showDestinations(){
        destinationPanel.SetActive(true);
        MainViewPanel.SetActive(false);
    }


    public void DonePressed(){
        hideDestination();
        NodeNavigation.instance.GetAllPaths();
    }

    private void hideDestination(){
        destinationPanel.SetActive(false);
        MainViewPanel.SetActive(true);
    }

}
