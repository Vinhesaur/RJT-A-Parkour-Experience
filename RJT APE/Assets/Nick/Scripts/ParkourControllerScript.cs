using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParkourControllerScript : MonoBehaviour
{
    public EnvironmentChecker environmentChecker;
    public Animator animator;
    public PlayerScript playerScript;

    [SerializeField] NewParkourAction jumpDownParkourAction;

    [SerializeField] NewParkourAction jumpAction;

    [Header("Parkour Action Area")]
    public List<NewParkourAction> newParkourAction;

    void Update()
    {
        if(Input.GetButton("Jump") && !playerScript.playerInAction && !playerScript.playerHanging) 
        {
            var hitData = environmentChecker.CheckObstacle();

            if (hitData.hitFound)
            {
                foreach (var action in newParkourAction)
                {
                    if (action.CheckIfAvailable(hitData, transform))
                    {
                        //perform parkour acction
                        StartCoroutine(PerformParkourAction(action));
                        break;
                    }
                }
            }

        }

        if(Input.GetButton("Jump") && !playerScript.playerInAction && !playerScript.playerHanging) 
        {
            if (jumpDownParkourAction.maximumH <= 0)
            {
                foreach (var action in newParkourAction)
                {
                        StartCoroutine(PerformParkourAction(jumpAction));
                }
            }

        }

        if(playerScript.playerOnLedge && !playerScript.playerInAction && Input.GetButtonDown("Jump"))
        {
            if(playerScript.LedgeInfo.angle <= 50)
            {
                playerScript.playerOnLedge = false;
                StartCoroutine(PerformParkourAction(jumpDownParkourAction));
            }
        }
        
    }

    IEnumerator PerformParkourAction(NewParkourAction action)
    {
        playerScript.SetControl(false);

        CompareTargetParameter compareTargetParameter = null;
        if(action.AllowTargetMatching)
        {
            compareTargetParameter = new CompareTargetParameter()
            {
                position = action.ComparePosition,
                bodyPart = action.CompareBodyPart,
                positionWeight = action.ComparePositionWeight,
                startTime = action.CompareStartTime,
                endTime = action.CompareEndTime

            };
        }

        yield return playerScript.PerformAction(action.AnimationName, compareTargetParameter, action.RequiredRotation, action.LookAtObstacle, action.ParkourActionDelay);

        playerScript.SetControl(true);
    }

    void CompareTarget(NewParkourAction action)
    {
        animator.MatchTarget(action.ComparePosition, transform.rotation, action.CompareBodyPart, new MatchTargetWeightMask(action.ComparePositionWeight, 0), action.CompareStartTime, action.CompareEndTime);
    }
}
