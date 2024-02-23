using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScripts : MonoBehaviour
{
    public Transform target;
    public float gap = 3f;
    private void Update()
    {
        transform.position = target.position - new Vector3(0, 0, gap);
    }

}
