using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the target object (your character)

    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        // Get the Cinemachine Virtual Camera component
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (target != null)
        {
            // Set the Virtual Camera's Follow target to the specified target
            virtualCamera.Follow = target;
        }
    }
}