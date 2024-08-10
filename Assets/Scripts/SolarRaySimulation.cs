using UnityEngine;

public class SolarRaySimulation : MonoBehaviour
{
    public Light sunLight; // Assign your directional light here
    public int rayCount = 100; // Number of rays to draw
    public float rayLength = 10f; // Length of each ray
    public float spreadAngle = 0.1f; // Angle variation to simulate scattered rays

    void OnDrawGizmos()
    {
        if (sunLight == null || sunLight.type != LightType.Directional)
        {
            Debug.LogWarning("Please assign a directional light to simulate the sun rays.");
            return;
        }

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
            Gizmos.DrawLine(transform.position - transform.forward * 10000, transform.position + rayDirection * rayLength);
        }
    }
}
