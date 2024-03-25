using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingController : MonoBehaviour
{
    EnvironmentChecker ec;
    public PlayerScript playerScript;
    
    ClimbingPoint currentClimbPoint;

    public float InOutValue;
    public float UpDownValue;
    public float LeftRigthValue;

    void Awake()
    {
        ec =GetComponent<EnvironmentChecker>();
    }
    
    void Update()
    {
        if(!playerScript.playerHanging)
        {
            if(Input.GetButton("Jump") && !playerScript.playerInAction)
            {
             if(ec.CheckClimbing(transform.forward, out RaycastHit climbInfo))
                {
                    currentClimbPoint = climbInfo.transform.GetComponent<ClimbingPoint>();

                    playerScript.SetControl(false);
                    //change values to fit hands to ledge
                    InOutValue = -0.35f;
                    UpDownValue = -0.1f;
                    LeftRigthValue = 0.0f;
                    StartCoroutine(ClimbToLedge("IdleToClimb", climbInfo.transform, .40f, 54f, playerHandOffset: new Vector3(InOutValue, UpDownValue, LeftRigthValue)));
                }
            }

            if(Input.GetButton("Leave") && !playerScript.playerInAction)
            {
                if(ec.CheckDropPoint(out RaycastHit DropHit))
                {
                    currentClimbPoint = GetNearestClimbingPoint(DropHit.transform, DropHit.point);

                    playerScript.SetControl(false);
                    //change values to fit hands to ledge
                    InOutValue = 0.1f;
                    UpDownValue = -0.44f;
                    LeftRigthValue = 0.25f;
                    StartCoroutine(ClimbToLedge("DropToLedge", currentClimbPoint.transform, 0.40f, 0.55f, playerHandOffset: new Vector3(InOutValue, UpDownValue, LeftRigthValue)));
                }
            }
        }
        else
        {
            //jump off wall
            if(Input.GetButton("Leave") && !playerScript.playerInAction)
            {
                StartCoroutine(JumpOffWall());
                return;
            }

            float horizontal = Mathf.Round(Input.GetAxisRaw("Horizontal"));
            float vertical = Mathf.Round(Input.GetAxisRaw("Vertical"));

            var inputDirection = new Vector2(horizontal, vertical);
            
            if(playerScript.playerInAction || inputDirection == Vector2.zero)
            {
                return;
            }

            //climb to top
            if(currentClimbPoint.MountPoint && inputDirection.y == 1)
            {
                StartCoroutine(ClimbToTop());
                return;
            }

            //ledge to ledge parkour actions

            var neighbour = currentClimbPoint.GetNeighbour(inputDirection);

            if(neighbour == null) return;

            if(neighbour.connectionType == ConnectionType.Jump && Input.GetButton("Jump"))
            {
                currentClimbPoint = neighbour.climbingPoint;

                if(neighbour.pointDirection.y == 1)
                {
                    //change values to fit hands to ledge
                    InOutValue = 0.07f;
                    UpDownValue = 0.03f;
                    LeftRigthValue = 0.15f;
                    StartCoroutine(ClimbToLedge("ClimbUp", currentClimbPoint.transform, 0.34f, 0.64f, playerHandOffset: new Vector3(InOutValue, UpDownValue, LeftRigthValue)));
                }

                else if(neighbour.pointDirection.y == -1)
                {
                    //change values to fit hands to ledge
                    InOutValue = 0.09f;
                    UpDownValue = 0.05f;
                    LeftRigthValue = 0.15f;
                    StartCoroutine(ClimbToLedge("ClimbDown", currentClimbPoint.transform, 0.30f, 0.69f, playerHandOffset: new Vector3(InOutValue, UpDownValue, LeftRigthValue)));
                }

                else if(neighbour.pointDirection.x == 1)
                {
                   
                   //change values to fit hands to ledge
                    InOutValue = 0.07f;
                    UpDownValue = 0.0f;
                    LeftRigthValue = 0.0f; 
                    StartCoroutine(ClimbToLedge("ClimbRight", currentClimbPoint.transform, 0.20f, 0.51f, playerHandOffset: new Vector3(InOutValue, UpDownValue, LeftRigthValue)));
                }

                else if(neighbour.pointDirection.x == -1)
                {
                    //change values to fit hands to ledge
                    InOutValue = 0.08f;
                    UpDownValue = 0.02f;
                    LeftRigthValue = 0.0f;
                    StartCoroutine(ClimbToLedge("ClimbLeft", currentClimbPoint.transform, 0.20f, 0.51f, playerHandOffset: new Vector3(InOutValue, UpDownValue, LeftRigthValue)));
                }
            }

            else if(neighbour.connectionType == ConnectionType.Move)
            {
                currentClimbPoint = neighbour.climbingPoint;

                if(neighbour.pointDirection.x == 1)
                {
                    //change values to fit hands to ledge
                    InOutValue = 0.12f;
                    UpDownValue = -0.02f;
                    LeftRigthValue = 0.0f;
                    StartCoroutine(ClimbToLedge("ShimmyRight", currentClimbPoint.transform, 0f, 0.30f, playerHandOffset: new Vector3(InOutValue, UpDownValue, LeftRigthValue)));
                }
            }

             else if(neighbour.connectionType == ConnectionType.Move)
            {
                currentClimbPoint = neighbour.climbingPoint;

                if(neighbour.pointDirection.x == -1)
                {
                    //change values to fit hands to ledge
                    InOutValue = 0.12f;
                    UpDownValue = -0.02f;
                    LeftRigthValue = 0.0f;
                    StartCoroutine(ClimbToLedge("ShimmyLeft", currentClimbPoint.transform, 0f, 0.30f, AvatarTarget.LeftHand, playerHandOffset: new Vector3(InOutValue, UpDownValue, LeftRigthValue)));
                }
            }
        }
    }

    IEnumerator ClimbToLedge(string animationName,Transform ledgePoint, float compareStartTime, float compareEndTime, 
    AvatarTarget hand = AvatarTarget.RightHand, Vector3? playerHandOffset = null)
    {
        var compareParams = new CompareTargetParameter()
        {
            position = SetHandPosition(ledgePoint, hand, playerHandOffset),
            bodyPart = hand,
            positionWeight = Vector3.one,
            startTime = compareStartTime,
            endTime = compareEndTime
        };

        var requiredRot = Quaternion.LookRotation(-ledgePoint.forward);

        yield return playerScript.PerformAction(animationName, compareParams, requiredRot, true);

        playerScript.playerHanging = true;
    }

    Vector3 SetHandPosition(Transform ledge, AvatarTarget hand, Vector3? playerhandOffset)
    {
        var offsetValue = (playerhandOffset != null) ? playerhandOffset.Value : new Vector3 (InOutValue, UpDownValue, LeftRigthValue);

        var handDirection = (hand == AvatarTarget.RightHand) ? ledge.right : -ledge.right;
        return ledge.position + ledge.forward * InOutValue + Vector3.up * UpDownValue - handDirection * LeftRigthValue;
    }

    IEnumerator JumpOffWall()
    {
        playerScript.playerHanging = false;
        yield return playerScript.PerformAction("JumpOffWall");
        playerScript.ResetRequiredRotation();
        playerScript.SetControl(true);
    }

    IEnumerator ClimbToTop()
    {
        playerScript.playerHanging = false;
        yield return playerScript.PerformAction("ClimbToTop");
        
        playerScript.EnableCC(true);
        
        yield return new WaitForSeconds(0.5f);
        
        playerScript.ResetRequiredRotation();
        playerScript.SetControl(true);
    }

    ClimbingPoint GetNearestClimbingPoint(Transform dropPoint, Vector3 hitPoint)
    {
        var points = dropPoint.GetComponentsInChildren<ClimbingPoint>();

        ClimbingPoint nearestPoint = null;

        float nearestPointDistance = Mathf.Infinity;

        foreach(var point in points)
        {
            float distance = Vector3.Distance(point.transform.position, hitPoint);

            if(distance < nearestPointDistance)
            {
                nearestPoint = point;
                nearestPointDistance = distance;
            }
        }

        return nearestPoint;
    }
}
