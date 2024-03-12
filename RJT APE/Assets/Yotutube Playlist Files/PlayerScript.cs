using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 5f;
    public CameraController CC;
    public float rotSpeed = 600f;
    Quaternion requiredRotation;
    bool playerControl = true;

    [Header("PlayerScript Animator")]
    public Animator animator;

    [Header("Player Collision & Gravity")]
    public CharacterController cC;
    public float surfaceCheckRadius = 0.3f;
    public Vector3 surfaceCheckOffset;
    public LayerMask surfaceLayer;
    bool onSurface;
    [SerializeField]float fallingSpeed;
    [SerializeField] Vector3 moveDir;

    private void Update()
    {
        PlayerMovement();

        if (!playerControl)
        {
            return;
        }
       
        if(onSurface)
        {
            fallingSpeed = 0f;
        }
        else
        {
            fallingSpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * movementSpeed;
        velocity.y = fallingSpeed;

        SurfaceCheck();
        Debug.Log("Player on Surface" + onSurface);
    }

    void PlayerMovement()
        {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;

        var movementDirection = CC.flatRotation * movementInput;

        cC.Move(movementDirection * movementSpeed * Time.deltaTime);

        if (movementAmount > 0)
        {
            requiredRotation = Quaternion.LookRotation(movementDirection);
        }

        movementDirection = moveDir;


        transform.rotation = Quaternion.RotateTowards(transform.rotation, requiredRotation, rotSpeed * Time.deltaTime);

        animator.SetFloat("movementValue",  movementAmount, 0f, Time.deltaTime);

        }
    void SurfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
    }

    public void SetControl(bool hasControl)
    {
        this.playerControl = hasControl;
        cC.enabled = hasControl;

        if (!hasControl)
        {
            animator.SetFloat("movementValue", 0f);
            requiredRotation = transform.rotation;
        }
    }

}
