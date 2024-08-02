using UnityEngine;
using System;

[ExecuteInEditMode]
public class SunPathCalculations : MonoBehaviour
{
    public Transform sun;
    public float latitude = 0;  // Example latitude
    public float longitude = 0;  // Example longitude
    public string date = ""; // Example date in dd/MM/yyyy
    public string timeOfDay = ""; // Example time in HH:MM

    void Update()
    {
        // Parse the date
        DateTime parsedDate = DateTime.ParseExact(date, "dd/MM/yyyy", null);
        int dayOfYear = parsedDate.DayOfYear;

        // Parse the time
        TimeSpan parsedTime = TimeSpan.Parse(timeOfDay);
        float timeInHours = (float)parsedTime.TotalHours;

        // Calculate solar declination
        float declination = 23.45f * Mathf.Sin(Mathf.Deg2Rad * (360f / 365f * (284 + dayOfYear)));

        // Calculate the solar time angle (hour angle)
        float solarTime = timeInHours + (longitude / 15f);
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
