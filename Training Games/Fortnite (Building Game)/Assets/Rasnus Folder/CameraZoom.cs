using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;

    public Vector2 minCameraBounds;
    public Vector2 maxCameraBounds;

    public bool allowPan = true;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleZoom();
        if (allowPan)
            ClampCameraView();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            float targetZoom = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);

            if (allowPan)
            {
                Vector3 mouseWorldBeforeZoom = cam.ScreenToWorldPoint(Input.mousePosition);
                cam.orthographicSize = targetZoom;
                Vector3 mouseWorldAfterZoom = cam.ScreenToWorldPoint(Input.mousePosition);

                Vector3 offset = mouseWorldBeforeZoom - mouseWorldAfterZoom;
                cam.transform.position += offset;
            }
            else
            {
                cam.orthographicSize = targetZoom;
            }
        }
    }

    void ClampCameraView()
    {
        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        Vector3 pos = cam.transform.position;

        float left = pos.x - horzExtent;
        float right = pos.x + horzExtent;
        float bottom = pos.y - vertExtent;
        float top = pos.y + vertExtent;

        if (left < minCameraBounds.x)
            pos.x = minCameraBounds.x + horzExtent;
        if (right > maxCameraBounds.x)
            pos.x = maxCameraBounds.x - horzExtent;
        if (bottom < minCameraBounds.y)
            pos.y = minCameraBounds.y + vertExtent;
        if (top > maxCameraBounds.y)
            pos.y = maxCameraBounds.y - vertExtent;

        cam.transform.position = pos;
    }
}
