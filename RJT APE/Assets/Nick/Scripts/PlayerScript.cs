using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript: MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 5f;
    public CameraController CC;
    public EnvironmentChecker environmentChecker;
    public float rotSpeed = 600f;
    Quaternion requiredRotation;
    bool playerControl = true;
    public bool playerInAction{get; private set;}


    [Header("PlayerScript Animator")]
    public Animator animator;

    [Header("Player Collision & Gravity")]
    public CharacterController cC;
    public float surfaceCheckRadius = 0.3f;
    public Vector3 surfaceCheckOffset;
    public LayerMask surfaceLayer;
    bool onSurface;

    public bool playerOnLedge {get; set;}
    public bool playerHanging{get; set;}

    public LedgeInfo LedgeInfo {get; set;}
    [SerializeField]float fallingSpeed;
    [SerializeField] Vector3 moveDir;
    [SerializeField] Vector3 requiredMoveDir;
    Vector3 velocity;

    // WWise
    [Header("Wwise Events")]
    public AK.Wwise.Event myFootstep;
    private bool footstepplay = false;
    private float lastFootstepTime = 0;
    private void Update()
    {
        if (!playerControl)
        {
            return;
        }

        if(playerHanging)
        {
            return;
        }
        
        velocity = Vector3.zero;

        if (onSurface)
        {
            fallingSpeed = 0f;
            velocity = moveDir * movementSpeed;

            playerOnLedge = environmentChecker.CheckLedge(moveDir, out LedgeInfo ledgeInfo);

            if(playerOnLedge)
            {
                LedgeInfo = ledgeInfo;
                playerLedgeMovement();
                Debug.Log("Player is on Ledge");
            }
            
            animator.SetFloat("movementValue", velocity.magnitude / movementSpeed, 0f, Time.deltaTime);
        }
        else
        {
            fallingSpeed += Physics.gravity.y * Time.deltaTime;

            velocity = transform.forward * movementSpeed / 2;
        }

        velocity.y = fallingSpeed;

        PlayerMovement();
        SurfaceCheck();
        animator.SetBool("onSurface", onSurface);
        //Debug.Log("Player on Surface" + onSurface);
    }

    void PlayerMovement()
        {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float movementAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        var movementInput = (new Vector3(horizontal, 0, vertical)).normalized;
        
        requiredMoveDir = CC.flatRotation * movementInput;

        cC.Move(velocity * Time.deltaTime);
        if (movementAmount == 0)
        {
            footstepplay = false;
        }
        if (movementAmount > 0 && moveDir.magnitude > 0.2f)
        {
            requiredRotation = Quaternion.LookRotation(moveDir);
            if (!footstepplay)
            {
                myFootstep.Post(gameObject);
                lastFootstepTime = Time.time;
                footstepplay = true;
            }
            else
            {
                if (movementSpeed > 1)
                {
                    if (Time.time - lastFootstepTime > 750 / movementSpeed * Time.deltaTime)
                    {
                        footstepplay = false;
                    }
                }
             
            }
        }

        moveDir = requiredMoveDir;


        transform.rotation = Quaternion.RotateTowards(transform.rotation, requiredRotation, rotSpeed * Time.deltaTime);
        }
    void SurfaceCheck()
    {
        onSurface = Physics.CheckSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius, surfaceLayer);
    }

    void playerLedgeMovement()
    {
        float angle = Vector3.Angle(LedgeInfo.surfaceHit.normal, requiredMoveDir);
        
        if (angle < 90) 
        {
            velocity = Vector3.zero;
            moveDir = Vector3.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(surfaceCheckOffset), surfaceCheckRadius);
    }

    public IEnumerator PerformAction(string AnimationName, CompareTargetParameter ctp = null, Quaternion RequiredRotation = new Quaternion(),
     bool LookAtObstacle = false, float ParkourActionDelay = 0f)
    {
        playerInAction = true;

        animator.CrossFadeInFixedTime(AnimationName, 0.2f);
        yield return null;

        var animationState = animator.GetNextAnimatorStateInfo(0);
        if (!animationState.IsName(AnimationName))
        {
            Debug.Log("Animation Name Incorrect");
        }
        float rotateStartTime = (ctp != null) ? ctp.startTime : 0f;
        float timerCounter = 0f;

        while(timerCounter <= animationState.length)
        {
            timerCounter += Time.deltaTime;

            float normalizedTimerCounter = timerCounter / animationState.length;

            //make player look towards obstacle
            if(LookAtObstacle && normalizedTimerCounter > rotateStartTime)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, RequiredRotation, rotSpeed * Time.deltaTime);
            }

            if(ctp != null)
            {
                CompareTarget(ctp);
            }

            if(animator.IsInTransition(0) && timerCounter > 0.5f)
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(ParkourActionDelay);

       playerInAction = false;
    }

    void CompareTarget(CompareTargetParameter compareTargetParameter)
    {
        animator.MatchTarget(compareTargetParameter.position, transform.rotation, compareTargetParameter.bodyPart, new MatchTargetWeightMask(compareTargetParameter.positionWeight, 0), compareTargetParameter.startTime, compareTargetParameter.endTime);
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

    public void EnableCC(bool enabled)
    {
        cC.enabled = enabled;
    }

    public void ResetRequiredRotation()
    {
        requiredRotation = transform.rotation;
    }

    public bool HasPlayerControl
    {
        get => playerControl;
        set => playerControl = value;
    }
    private void Awake()
    {
        lastFootstepTime = Time.time;
    }
}

public class CompareTargetParameter
{
    public  Vector3 position;
    public AvatarTarget bodyPart;
    public Vector3  positionWeight;
    public float startTime;
    public float endTime;
}