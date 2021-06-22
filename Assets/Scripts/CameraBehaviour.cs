using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Vector2 minCoords;
    public Vector2 maxCoords;

    public float cameraMinZoom = 2;
    public float cameraMaxZoom = 11;

    private new Camera camera;
    private GameObject targetToFollow;

    // Start is called before the first frame update
    void Start()
    {
        // Get actual Camera component
        camera = GetComponent<Camera>();

        // Find player and move camera to focus on them
        targetToFollow = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Handle camera zoom
        Camera.main.orthographicSize = Mathf.Min(Mathf.Max(Camera.main.orthographicSize - Input.mouseScrollDelta.y, cameraMinZoom), cameraMaxZoom);

        // Determine camera width and height
        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        // Calculate initial new camera position
        var newX = targetToFollow.transform.position.x;
        var newY = targetToFollow.transform.position.y;

        // Constrain camera to bounds
        if (newX + halfWidth > maxCoords.x)
        {
            newX = maxCoords.x - halfWidth;
        }
        if (newX - halfWidth < minCoords.x)
        {
            newX = minCoords.x + halfWidth;
        }
        if (newY + halfHeight > maxCoords.y)
        {
            newY = maxCoords.y - halfHeight;
        }
        if (newY - halfHeight < minCoords.y)
        {
            newY = minCoords.y + halfHeight;
        }

        // Update camera position
        this.transform.position = new Vector3(
            newX,
            newY,
            this.transform.position.z
        );
    }
}
