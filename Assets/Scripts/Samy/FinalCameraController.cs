using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCameraController : MonoBehaviour
{
    public static FinalCameraController Instance;
    public Camera _cameraTransform;
    private Vector3 _cameraForwardDirection;
    public Transform focusPoint;

    public float panSpeed = 1f;
    public float rotateSpeed = 1f;
    public float zoomSpeed = 1f;
    public float zoomMinLimit = 1f;
    public float zoomMaxLimit = 10f;
    public Vector2 panClampLimits = new Vector2(-10, 10);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _cameraTransform = Camera.main;
        _cameraForwardDirection = _cameraTransform.transform.forward;


    }
    private void Update()
    {
        //RefreshFocusPoint();
    }

    public void Pan(Vector2 panDelta)
    {
        Vector3 panDirection = new Vector3(panDelta.x, 0f, panDelta.y);
        Vector3 panTranslation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * panDirection;
        //transform.Translate(panTranslation * panSpeed * Time.deltaTime, Space.World);\
        Vector3 newPosition = transform.position + panTranslation * panSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, panClampLimits.x, panClampLimits.y);
        newPosition.z = Mathf.Clamp(newPosition.z, panClampLimits.x, panClampLimits.y);

        transform.position = newPosition;
    }

    public void Rotate(float rotationAngle)
    {
        transform.RotateAround(focusPoint.position, Vector3.up, rotationAngle * rotateSpeed * Time.deltaTime);
    }

    public void Zoom(float zoomDistance)
    {
        float newDistance = Vector3.Distance(transform.position, focusPoint.position) - zoomDistance * zoomSpeed * Time.deltaTime;
        newDistance = Mathf.Clamp(newDistance, zoomMinLimit, zoomMaxLimit);

        Vector3 zoomPosition = transform.position - focusPoint.position;
        Vector3 zoomDirection = zoomPosition.normalized;
        Vector3 newZoomPosition = zoomDirection * newDistance;

        transform.position = focusPoint.position + newZoomPosition;
    }
    private void RefreshFocusPoint()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, _cameraForwardDirection, out hitInfo, 100))
        {
            focusPoint.transform.position = hitInfo.point;
        }
    }
}


