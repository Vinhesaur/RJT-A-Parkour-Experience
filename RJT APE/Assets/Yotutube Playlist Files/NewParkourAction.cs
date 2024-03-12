using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour Menu/Create New Parkour Action")]
public class NewParkourAction : ScriptableObject
{
    [Header("Checking Obstacle Height")]
    [SerializeField] string animationName;
    [SerializeField] float minimumHeight;
    [SerializeField] float maximumHeight;

    [Header("Rotating Player towards Obstacle")]
    [SerializeField] bool lookAtObstacle;
    public Quaternion RequiredRotation { get; set; }

    [Header("Target Matching")]
    [SerializeField] bool allowTargetMatching = true;
    [SerializeField] AvatarTarget compareBodyPart;
    [SerializeField] float compareStartTime;
    [SerializeField] float compareEndTime;

    public Vector3 ComparePosition { get; set;}

    public bool CheckIfAvailable(ObstacleInfo hitData, Transform player)
    {
        float checkHeight = hitData.heightinfo.point.y - player.position.y;

        if(checkHeight < minimumHeight || checkHeight > maximumHeight)
            return false;
        if(lookAtObstacle)
        {
            RequiredRotation = Quaternion.LookRotation(-hitData.hitInfo.normal);
        }

        if(allowTargetMatching) 
        {
            ComparePosition = hitData.heightinfo.point;
        }
        
        return true;
    }

    public string AnimationName => animationName;
    public bool LookAtObstacle => lookAtObstacle;

    public bool AllowTargetMatching => allowTargetMatching;
    public AvatarTarget CompareBodyPart => compareBodyPart;
    public float CompareStartTime => compareStartTime;
    public float CompareEndTime => compareEndTime;
}
