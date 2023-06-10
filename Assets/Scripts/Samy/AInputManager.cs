using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AInputManager : MonoBehaviour
{
    public static AInputManager instance; // instance reference
    private Vector2 panAxis = Vector2.zero;


    public float panAngleThreshold = 1f;
    private Vector2 initialTouch1Position;
    private Vector2 initialTouch2Position;
    private Vector2 previousTouch1Position;
    private Vector2 previousTouch2Position;
    private Vector2 touchDelta1;
    private Vector2 touchDelta2;

    public float rotationAngleThreshold = 5f;
    public float panThreshold = 5f;
    public float zoomThreshold = 5f;

    private void Update()
    {
        HandleTouchInput();
    }

    private void HandleTouchInput()
    {

        // Check for touch input
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialTouch1Position = touch1.position;
                initialTouch2Position = touch2.position;
                previousTouch1Position = touch1.position;
                previousTouch2Position = touch2.position;
                touchDelta1 = Vector2.zero;
                touchDelta2 = Vector2.zero;
            }


            // Calculate touch deltas
            touchDelta1 = touch1.position - previousTouch1Position;
            touchDelta2 = touch2.position - previousTouch2Position;

            // Calculate rotation angle
            float previousAngle = Mathf.Atan2(previousTouch2Position.y - previousTouch1Position.y, previousTouch2Position.x - previousTouch1Position.x) * Mathf.Rad2Deg;
            float currentAngle = Mathf.Atan2(touch2.position.y - touch1.position.y, touch2.position.x - touch1.position.x) * Mathf.Rad2Deg;
            float rotationAngle = Mathf.DeltaAngle(previousAngle, currentAngle);

            // Calculate zoom distance
            float initialDistance = Vector2.Distance(initialTouch1Position, initialTouch2Position);
            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            float zoomDistance = currentDistance - initialDistance;
            // Update camera movements based on touch input
            if (Mathf.Abs(rotationAngle) > rotationAngleThreshold)
            {
                FinalCameraController.Instance.Rotate(rotationAngle);
            }
            else if (Mathf.Abs(touchDelta1.x) > panThreshold || Mathf.Abs(touchDelta1.y) > panThreshold)
            {
                FinalCameraController.Instance.Pan(touchDelta1);
            }
            else if (Mathf.Abs(zoomDistance) > zoomThreshold)
            {
                FinalCameraController.Instance.Zoom(zoomDistance);
            }

            // Store current touch positions as previous for the next frame
            previousTouch1Position = touch1.position;
            previousTouch2Position = touch2.position;
            // Handle touch phase end
            if (touch1.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Canceled ||
                touch2.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Canceled)
            {
                initialTouch1Position = Vector2.zero;
                initialTouch2Position = Vector2.zero;
                previousTouch1Position = Vector2.zero;
                previousTouch2Position = Vector2.zero;
                touchDelta1 = Vector2.zero;
                touchDelta2 = Vector2.zero;
            }
        }
    }
}
