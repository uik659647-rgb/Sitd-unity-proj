using UnityEngine;

public class SizeChecker : MonoBehaviour
{
    public float maxScreenSize = 0.2f; // Max allowed size (as screen ratio)
    public Camera cameraToCheck;

    private Renderer objectRenderer;

    void Start()
    {
        if (cameraToCheck == null)
            cameraToCheck = Camera.main;

        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
            Debug.LogWarning("SizeChecker requires a Renderer on the object.");
    }

    void Update()
    {
        if (objectRenderer == null || cameraToCheck == null) return;

        Bounds bounds = objectRenderer.bounds;
        Vector3 boundsCenter = bounds.center;
        Vector3 boundsExtents = bounds.extents;

        // Project corners to screen space
        Vector3[] worldCorners = new Vector3[]
        {
            boundsCenter + new Vector3(+boundsExtents.x, +boundsExtents.y, +boundsExtents.z),
            boundsCenter + new Vector3(+boundsExtents.x, +boundsExtents.y, -boundsExtents.z),
            boundsCenter + new Vector3(+boundsExtents.x, -boundsExtents.y, +boundsExtents.z),
            boundsCenter + new Vector3(+boundsExtents.x, -boundsExtents.y, -boundsExtents.z),
            boundsCenter + new Vector3(-boundsExtents.x, +boundsExtents.y, +boundsExtents.z),
            boundsCenter + new Vector3(-boundsExtents.x, +boundsExtents.y, -boundsExtents.z),
            boundsCenter + new Vector3(-boundsExtents.x, -boundsExtents.y, +boundsExtents.z),
            boundsCenter + new Vector3(-boundsExtents.x, -boundsExtents.y, -boundsExtents.z),
        };

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (var corner in worldCorners)
        {
            Vector3 screenPoint = cameraToCheck.WorldToViewportPoint(corner);
            if (screenPoint.z > 0)
            {
                minX = Mathf.Min(minX, screenPoint.x);
                maxX = Mathf.Max(maxX, screenPoint.x);
                minY = Mathf.Min(minY, screenPoint.y);
                maxY = Mathf.Max(maxY, screenPoint.y);
            }
        }

        float width = maxX - minX;
        float height = maxY - minY;
        float screenArea = width * height;

        if (screenArea > maxScreenSize)
        {
            Debug.LogWarning($"{gameObject.name} appears too large on screen (area: {screenArea:F3})");
            // You could trigger a resize, hide, or alert here
        }
    }
}
