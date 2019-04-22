using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VuforiaSimulationManager : MonoBehaviour {


    public Text simulateToggleButtonText;
    public static bool isSimulationOn = false;


    public void toggleSimulation()
    {
        if (isSimulationOn)
        {
            isSimulationOn = false;
            simulateToggleButtonText.text = "Turn ON Simulation";
        }
        else
        {
            isSimulationOn = true;
            simulateToggleButtonText.text = "Turn OFF Simulation";
        }
    }
}
