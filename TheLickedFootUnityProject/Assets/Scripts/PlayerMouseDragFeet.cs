using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;



public class PlayerMouseDragFeet : MonoBehaviour
{
    public float WhereOnYAxis;
    public float TimeOutsideBlanket;
    public float InputFromMasterDarkness;
    public Vector3 targetPosition; // The position to move towards
    public Vector3 startingPosition; // The original position of the object
   /* public Vector3 VectorWhere;
    public float magnitude;
    public bool test;

    public float speed;
    public float currentSpeed;
    public float AtoB;
    public Vector3 AtoB1;
    public Vector3 FootToTarget;
    public float FootToTargetMagnitude;
    public float DistansToGoal;
    public float TimeOutsideBlanket;
    public float WhereOnYAxis;

    //Foot Out
    private float AccelerationOut;
    public float velocityOut;
    public float maxVelOut;
    public float minVelOut; */

    //Foot In
    public Vector3 StartToTarget;
    public Vector3 TargetToStart;
    public AnimationCurve VelocityOverTime;
    public float MaxTimeForMove = 2;
    public float MinTimeForMove = 1;
    public float CurrentTimeForMove;
    public float MaxTimeTriggerLoop = 10;
    public float MinTimeTriggerLoop = 0.5f;
    public bool MoveIn;
    public bool MoveOut;

    private float TimerMove;
    private float PercentageMove;

    private float TimerTriggerOutLoop;

    void Start()
    {
        MoveIn = false;
        MoveOut = false;

        StartToTarget = targetPosition - startingPosition;
        TargetToStart = startingPosition - targetPosition;

        transform.position = startingPosition;

        TimerTriggerOutLoop = MaxTimeTriggerLoop;
    }

    private void OnMouseDown()
    {
        if(MoveIn || MoveOut)
        {
            return;
        }

        MoveIn = true;
    }

 
    void Update()
    {
        TimerTriggerOutLoop = TimerTriggerOutLoop - Time.deltaTime;
        if(TimerTriggerOutLoop < 0)
        {
            TimerTriggerOutLoop = (1 - InputFromMasterDarkness) * MaxTimeTriggerLoop;
            if(TimerTriggerOutLoop < MinTimeTriggerLoop)
            {
                TimerTriggerOutLoop = MinTimeTriggerLoop;
            }

            if(transform.position == startingPosition)
            {
                MoveOut = true;
            }
        }


        CurrentTimeForMove = (1 - InputFromMasterDarkness) * MaxTimeForMove;
        if(CurrentTimeForMove < MinTimeForMove)
        {
            CurrentTimeForMove = MinTimeForMove;
        }

        if (MoveIn)
        {
            TimerMove = TimerMove + Time.deltaTime;
            PercentageMove = TimerMove / CurrentTimeForMove;

            transform.position = targetPosition + (TargetToStart * VelocityOverTime.Evaluate(PercentageMove));

            if(PercentageMove > 1)
            {
                PercentageMove = 0;
                TimerMove = 0;
                MoveIn = false;
                transform.position = startingPosition;
            }
        }

        if (MoveOut)
        {
            TimerMove = TimerMove + Time.deltaTime;
            PercentageMove = TimerMove / CurrentTimeForMove;

            transform.position = startingPosition + (StartToTarget * VelocityOverTime.Evaluate(PercentageMove));

            if (PercentageMove > 1)
            {
                PercentageMove = 0;
                TimerMove = 0;
                MoveOut = false;
                transform.position = targetPosition;
            }
        }

        // Move the object towards the target position
        // transform.position = Vector3.MoveTowards(transform.position, VectorWhere, speed * Time.deltaTime * (DistansToGoal * 5f));

        //Time outside blanket
        if (transform.position.y > WhereOnYAxis)
        {
         TimeOutsideBlanket += Time.deltaTime;
        }
        else
        {
         TimeOutsideBlanket = 0;
        }
    }
}