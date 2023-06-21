using UnityEngine;
using Cinemachine;
using System.Collections;

public class WaypointCameraController : MonoBehaviour
{

    public CinemachineVirtualCamera virtualCamera;
    public Transform[] waypoints;
    public GameObject[] Slides;
    private int currentWaypointIndex = 0;

    private void Start()
    {
        deactivateAllWayPoints();
        Slides[currentWaypointIndex].SetActive(true);
        if (waypoints.Length > 0)
        {
            // Set the initial position and rotation of the virtual camera
            virtualCamera.transform.position = waypoints[currentWaypointIndex].position;
            virtualCamera.transform.rotation = waypoints[currentWaypointIndex].rotation;
        }
    }

    private void Update()
    {
        if (waypoints.Length == 0)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // Move to the next waypoint
            MoveToNextWaypoint();

        }
        else if (Input.GetMouseButtonDown(1))
        {
            // Move to the previous waypoint if not on the first waypoint
            if (currentWaypointIndex > 0)
            {
                MoveToPreviousWaypoint();
            }
        }
    }

    private void MoveToNextWaypoint()
    {
        StartCoroutine(
                // Increment the waypoint index
                DeactivateGameObject(Slides[currentWaypointIndex]));
        currentWaypointIndex++;
        // Check if the waypoint index exceeds the array bounds
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = waypoints.Length - 1;
        }

        // Move the virtual camera to the next waypoint
        MoveCameraToWaypoint(currentWaypointIndex);
    }

    private void MoveToPreviousWaypoint()
    {
        StartCoroutine(
                // Decrement the waypoint index
                DeactivateGameObject(Slides[currentWaypointIndex]));
        currentWaypointIndex--;
        // Move the virtual camera to the previous waypoint
        MoveCameraToWaypoint(currentWaypointIndex);
    }

    private void MoveCameraToWaypoint(int index)
    {
        // Start the interpolation coroutine for position and rotation
        Slides[currentWaypointIndex].SetActive(true);
        StartCoroutine(InterpolateCameraToWaypoint(waypoints[index]));
    }

    private IEnumerator InterpolateCameraToWaypoint(Transform targetWaypoint)
    {
        float elapsedTime = 0f;
        float duration = 1f; // Change this value to adjust the interpolation speed

        Vector3 initialPosition = virtualCamera.transform.position;
        Quaternion initialRotation = virtualCamera.transform.rotation;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Interpolate camera position
            virtualCamera.transform.position = Vector3.Lerp(initialPosition, targetWaypoint.position, t);

            // Interpolate camera rotation
            Quaternion targetRotation = targetWaypoint.rotation;
            virtualCamera.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

            yield return null;
        }


        // Set the final position and rotation to ensure accuracy
        virtualCamera.transform.position = targetWaypoint.position;
        virtualCamera.transform.rotation = targetWaypoint.rotation;
    }
    void deactivateAllWayPoints()
    {
        foreach (GameObject plan in Slides)
        {
            plan.SetActive(false);
        }
    }
    private IEnumerator DeactivateGameObject(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }

}
