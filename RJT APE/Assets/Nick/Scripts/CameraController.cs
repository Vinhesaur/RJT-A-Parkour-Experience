using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float rotationY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        //Get the current Camera state
        var state = vcam.State;

        //Extract camera rotation from state
        var rotation = state.FinalOrientation;

        //convert rotation to euler
        var euler = rotation.eulerAngles;

        //get y axis from euler angle
        rotationY = euler.y;

        //round rotation to integer for smoothing
        var roundedRotationY = Mathf.Round(rotationY);
    }

    public Quaternion flatRotation => Quaternion.Euler(0, rotationY, 0);
}
