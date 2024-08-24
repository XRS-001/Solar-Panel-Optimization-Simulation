using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isDragging = false;
    public float dragSpeed = 1f;
    public LayerMask moveableLayer;
    private Transform objectMoving;

    void Update()
    {
        // Check for mouse input to start dragging
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Perform the raycast and check if the object is in the moveable layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, moveableLayer))
            {
                objectMoving = hit.transform.parent;
                isDragging = true;
                screenPoint = Camera.main.WorldToScreenPoint(objectMoving.position);
                offset = objectMoving.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            }
        }

        // While dragging
        if (isDragging)
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
            cursorPosition.y = objectMoving.position.y; // Lock the Y axis
            objectMoving.position = Vector3.Lerp(objectMoving.position, cursorPosition, dragSpeed * Time.deltaTime);
        }

        // Stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
