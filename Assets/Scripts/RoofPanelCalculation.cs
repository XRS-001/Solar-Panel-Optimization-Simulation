using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofPanelCalculation : MonoBehaviour
{
    public SolarRaySimulation simulation;
    public int amountOfTimesHit = 0;
    public float efficiency;
    public void CalculateEfficiency()
    {
        efficiency = amountOfTimesHit / simulation.potentialHits;
        Debug.Log(efficiency);
    }
}
