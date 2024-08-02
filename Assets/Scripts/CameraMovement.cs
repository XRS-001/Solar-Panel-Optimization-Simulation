using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float lookSpeed = 2f;
    public float shiftMultiplier = 2f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Update()
    {
        // Handle camera rotation
        if (Input.GetMouseButton(1)) // Right mouse button to look around
        {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed;
            rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);

            transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
        }

        // Handle camera movement
        float currentSpeed = movementSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= shiftMultiplier;
        }

        Vector3 direction = new Vector3(
            Input.GetAxis("Horizontal"), // A/D keys or Left/Right arrow keys
            0,
            Input.GetAxis("Vertical") // W/S keys or Up/Down arrow keys
        );

        if (Input.GetKey(KeyCode.Q)) // Q to move down
        {
            direction.y = -1;
        }
        else if (Input.GetKey(KeyCode.E)) // E to move up
        {
            direction.y = 1;
        }

        transform.Translate(direction * currentSpeed * Time.deltaTime, Space.Self);
    }
}
