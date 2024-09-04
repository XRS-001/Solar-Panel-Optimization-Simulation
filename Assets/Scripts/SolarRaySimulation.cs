using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class SolarRaySimulation : MonoBehaviour
{
    public Light sunLight; // Assign your directional light here
    public string rayCountSetting; // Number of rays to draw
    public int rayCount;
    public string spreadAngleSetting; // Angle variation to simulate scattered rays
    public float spreadAngle;
    public string distanceSetting;
    public float distance;
    public bool isSimulating;
    public enum increments { current, day, season, year }
    public increments timeIncrement;
    public enum Season { Spring, Summer, Autumn, Winter }
    public Season season;
    public GameManager gameManager;
    public bool visualizeRays;
    public GameObject hitPoint;
    public Transform hitPointsParent;
    [HideInInspector]
    public List<GameObject> spawnedHitPoints = new List<GameObject>();
    private Material lineMaterial;
    private void Start()
    {
        CreateLineMaterial();
        foreach (GameManager.BasicTextLengthLimiter text in gameManager.basicLimitedTexts)
        {
            if (text.identifier == rayCountSetting)
                rayCount = (int)float.Parse(text.text.text);
        }
        foreach (GameManager.BasicTextLengthLimiter text in gameManager.basicLimitedTexts)
        {
            if (text.identifier == spreadAngleSetting)
                spreadAngle = float.Parse(text.text.text);
        }
        foreach (GameManager.BasicTextLengthLimiter text in gameManager.basicLimitedTexts)
        {
            if (text.identifier == distanceSetting)
                distance = float.Parse(text.text.text);
        }
    }
    void OnRenderObject()
    {
        if (!lineMaterial || !visualizeRays)
            return;

        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.Begin(GL.LINES);
        GL.Color(new Color(1, 0.92f, 0.016f, 0.015f));  // Set the line color (yellowish with some transparency)

        Vector3 sunDirection = sunLight.transform.forward;
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate spherical coordinates for even distribution
            float theta = Mathf.Acos(2f * (i + 0.5f) / rayCount - 1f);
            float phi = Mathf.PI * (1 + Mathf.Sqrt(5)) * i;

            // Convert spherical coordinates to Cartesian coordinates
            Vector3 randomOffset = new Vector3(
                Mathf.Sin(theta) * Mathf.Cos(phi) * spreadAngle,
                Mathf.Sin(theta) * Mathf.Sin(phi) * spreadAngle,
                Mathf.Cos(theta) * spreadAngle
            );

            Vector3 rayDirection = (sunDirection + randomOffset).normalized;

            // Define the starting point of the ray
            Vector3 rayOrigin = transform.position - transform.forward * distance;

            // Calculate the endpoint of the ray
            Vector3 rayEndPoint = rayOrigin + rayDirection * (distance + 50);

            // Draw the line
            GL.Vertex(rayOrigin);
            GL.Vertex(rayEndPoint);
        }

        GL.End();
        GL.PopMatrix();
    }
    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }
    public IEnumerator Simulating()
    {
        if (!isSimulating)
        {
            isSimulating = true;
            foreach (GameObject hitPoint in spawnedHitPoints)
            {
                Destroy(hitPoint);
            }
            spawnedHitPoints.Clear();
            gameManager.sunCalculations.timeInHours = 0;
            switch (timeIncrement)
            {
                case increments.current:
                    Raycast();
                    break;
                case increments.day:
                    while (gameManager.sunCalculations.timeInHours < 24)
                    {
                        Raycast();
                        gameManager.sunCalculations.timeInHours += 0.01666666666f;
                        yield return null;
                    }
                    break;
                case increments.year:
                    gameManager.sunCalculations.dayOfYear = 0;
                    while (gameManager.sunCalculations.dayOfYear < 365)
                    {
                        while (gameManager.sunCalculations.timeInHours < 24)
                        {
                            Raycast();
                            gameManager.sunCalculations.timeInHours += 1;
                            yield return null;
                        }
                        gameManager.sunCalculations.dayOfYear++;
                        gameManager.sunCalculations.timeInHours = 0;
                        yield return null;
                    }
                    break;

                case increments.season:
                    int startDay = 0;
                    int endDay = 0;
                    GetSeasonDays(gameManager.sunCalculations.latitude, out startDay, out endDay);
                    gameManager.sunCalculations.dayOfYear = startDay;
                    while (gameManager.sunCalculations.dayOfYear < endDay || (endDay < startDay && gameManager.sunCalculations.dayOfYear != endDay))
                    {
                        while (gameManager.sunCalculations.timeInHours < 24)
                        {
                            Raycast();
                            gameManager.sunCalculations.timeInHours += 1;
                            yield return null;
                        }
                        gameManager.sunCalculations.timeInHours = 0;
                        gameManager.sunCalculations.dayOfYear++;
                        if (gameManager.sunCalculations.dayOfYear == 366)
                            gameManager.sunCalculations.dayOfYear = 0;
                        yield return null;
                    }
                    break;
            }
            gameManager.sunCalculations.timeInHours = gameManager.sunCalculations.timeInHours = (float)TimeSpan.Parse(gameManager.timeButtonText.text).TotalHours;
            gameManager.sunCalculations.dayOfYear = (int)float.Parse(gameManager.dayButtonText.text);
            isSimulating = false;
        }
        yield return null;
    }
    void Raycast()
    {
        Vector3 sunDirection = sunLight.transform.forward;
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate spherical coordinates for even distribution
            float theta = Mathf.Acos(2f * (i + 0.5f) / rayCount - 1f);  // Angle from the vertical axis
            float phi = Mathf.PI * (1 + Mathf.Sqrt(5)) * i;             // Golden angle for even distribution

            // Convert spherical coordinates to Cartesian coordinates
            Vector3 randomOffset = new Vector3(
                Mathf.Sin(theta) * Mathf.Cos(phi) * spreadAngle,
                Mathf.Sin(theta) * Mathf.Sin(phi) * spreadAngle,
                Mathf.Cos(theta) * spreadAngle
            );

            Vector3 rayDirection = (sunDirection + randomOffset).normalized;

            // Define the starting point of the ray
            Vector3 rayOrigin = transform.position - transform.forward * distance;

            RaycastHit hitInfo;
            if (Physics.Raycast(rayOrigin, rayDirection, out hitInfo, distance + 50))
            {
                if (hitInfo.transform)
                {
                    if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Roof"))
                    {
                        spawnedHitPoints.Add(Instantiate(hitPoint, hitInfo.point, Quaternion.LookRotation(hitInfo.normal) * Quaternion.Euler(90, 0, 0), hitPointsParent));
                    }
                }
            }
        }
    }
    void GetSeasonDays(float latitude,out int startDay, out int endDay)
    {
        if(latitude > 0)
        {
            switch (season)
            {
                case Season.Spring:
                    startDay = 80; 
                    endDay = 171; 
                    break;
                case Season.Summer:
                    startDay = 172; 
                    endDay = 264; 
                    break;
                case Season.Autumn:
                    startDay = 265; 
                    endDay = 334; 
                    break;
                case Season.Winter:
                    startDay = 335; 
                    endDay = 59;
                    break;
                default:
                    startDay = 1;
                    endDay = 365;
                    break;
            }
        }
        else
        {
            switch (season)
            {
                case Season.Spring:
                    startDay = 265; 
                    endDay = 334; 
                    break;
                case Season.Summer:
                    startDay = 335; 
                    endDay = 59; 
                    break;
                case Season.Autumn:
                    startDay = 80; 
                    endDay = 171; 
                    break;
                case Season.Winter:
                    startDay = 172;
                    endDay = 264;
                    break;
                default:
                    startDay = 1;
                    endDay = 365;
                    break;
            }
        }
    }
}
