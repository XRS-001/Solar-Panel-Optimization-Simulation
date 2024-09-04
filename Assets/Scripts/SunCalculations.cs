using UnityEngine;
using System;

[ExecuteInEditMode]
public class SunCalculations : MonoBehaviour
{
    public Transform sun;
    public float latitude = 0;  // Example latitude
    public int dayOfYear;
    public float timeInHours;

    void Update()
    {
        // Calculate solar declination
        float declination = 23.45f * Mathf.Sin(Mathf.Deg2Rad * (360f / 365f * (284 + dayOfYear)));

        // Calculate the equation of time (EoT)
        float B = 360f / 365f * (dayOfYear - 81);
        float EoT = 9.87f * Mathf.Sin(2 * Mathf.Deg2Rad * B) - 7.53f * Mathf.Cos(Mathf.Deg2Rad * B) - 1.5f * Mathf.Sin(Mathf.Deg2Rad * B);

        // Calculate the solar time angle (hour angle)
        float solarTime = timeInHours + (EoT / 60f);
        float hourAngle = 15f * (solarTime - 12f);

        // Calculate the solar elevation angle
        float latitudeRad = Mathf.Deg2Rad * latitude;
        float declinationRad = Mathf.Deg2Rad * declination;
        float hourAngleRad = Mathf.Deg2Rad * hourAngle;
        float elevationAngle = Mathf.Asin(Mathf.Sin(latitudeRad) * Mathf.Sin(declinationRad) + Mathf.Cos(latitudeRad) * Mathf.Cos(declinationRad) * Mathf.Cos(hourAngleRad));

        // Calculate the solar azimuth angle
        float azimuthAngle = Mathf.Acos((Mathf.Sin(declinationRad) - Mathf.Sin(elevationAngle) * Mathf.Sin(latitudeRad)) / (Mathf.Cos(elevationAngle) * Mathf.Cos(latitudeRad)));

        // Convert angles to degrees
        float elevationAngleDeg = Mathf.Rad2Deg * elevationAngle;
        float azimuthAngleDeg = Mathf.Rad2Deg * azimuthAngle;

        // Adjust azimuth angle based on hour angle
        if (hourAngle > 0)
        {
            azimuthAngleDeg = 360f - azimuthAngleDeg;
        }

        // Calculate the sun's position in Cartesian coordinates
        Vector3 sunPosition = new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * elevationAngleDeg) * Mathf.Sin(Mathf.Deg2Rad * azimuthAngleDeg),
            Mathf.Sin(Mathf.Deg2Rad * elevationAngleDeg),
            Mathf.Cos(Mathf.Deg2Rad * elevationAngleDeg) * Mathf.Cos(Mathf.Deg2Rad * azimuthAngleDeg)
        );

        // Set the sun transform's position and rotation
        sun.position = sunPosition * 10f; // Multiply by a distance factor
        sun.LookAt(Vector3.zero); // Assuming the scene's origin is the center of the Earth
    }
}
