using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;
public class SolarRaySimulation : MonoBehaviour
{
    public Light sunLight; // Assign your directional light here
    public int rayCount = 100; // Number of rays to draw
    public float spreadAngle = 0.1f; // Angle variation to simulate scattered rays
    public float distance;
    public bool isSimulating;
    public enum increments { seconds, minutes, hours }
    public increments timeIncrement;
    public SunCalculations sunCalculations;
    public int potentialHits;
    public int possibleHitsPerFrame;

    public IEnumerator Simulating()
    {
        isSimulating = true;
        float solarConeRadius = distance * spreadAngle;
        float solarConeArea = Mathf.PI * (solarConeRadius * solarConeRadius);
        Transform randomRoof = FindObjectOfType<RoofPanelCalculation>().transform;
        float roofArea = randomRoof.localScale.x * randomRoof.localScale.z;
        float roofPercentageOfCone = roofArea / solarConeArea;
        possibleHitsPerFrame = Mathf.RoundToInt(rayCount * roofPercentageOfCone);

        sunCalculations.timeInHours = 0;
        sunCalculations.dayOfYear = 0;
        potentialHits = 0;
        while(sunCalculations.dayOfYear < 365)
        {
            while (sunCalculations.timeInHours < 24)
            {
                // Get the direction of the light
                Vector3 sunDirection = sunLight.transform.forward;

                for (int i = 0; i < rayCount; i++)
                {
                    // Create a small random variation to simulate scattered rays
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-spreadAngle, spreadAngle),
                        Random.Range(-spreadAngle, spreadAngle),
                        Random.Range(-spreadAngle, spreadAngle)
                    );

                    Vector3 rayDirection = (sunDirection + randomOffset).normalized;

                    // Draw the ray as a gizmo line
                    Gizmos.color = Color.yellow;
                    RaycastHit hitInfo;
                    Physics.Raycast(transform.position - transform.forward * distance, rayDirection, out hitInfo, distance + 50);
                    if (hitInfo.transform)
                    {
                        if (hitInfo.transform.GetComponent<RoofPanelCalculation>())
                        {
                            hitInfo.transform.GetComponent<RoofPanelCalculation>().amountOfTimesHit++;
                        }
                    }
                }
                switch (timeIncrement)
                {
                    case increments.seconds:
                        sunCalculations.timeInHours += 0.00027777777f;
                        break;
                    case increments.minutes:
                        sunCalculations.timeInHours += 0.01666666666f;
                        break;
                    case increments.hours:
                        sunCalculations.timeInHours += 1;
                        break;
                }
                yield return null;
                potentialHits += possibleHitsPerFrame;
            }
            yield return null;
            sunCalculations.dayOfYear++;
            sunCalculations.timeInHours = 0;
        }
        RoofPanelCalculation[] roofs = FindObjectsOfType<RoofPanelCalculation>();
        foreach (RoofPanelCalculation roof in roofs)
        {
            Debug.Log(roof.name);
            roof.CalculateEfficiency();
        }
        isSimulating = false;
        yield return null;
    }
}
