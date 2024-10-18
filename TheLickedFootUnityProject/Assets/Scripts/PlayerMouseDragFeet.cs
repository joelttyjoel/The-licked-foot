using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;



public class PlayerMouseDragFeet : MonoBehaviour
{

    public Vector3 targetPosition; // The position to move towards
    public Vector3 startingPosition; // The original position of the object
    public Vector3 test;
    public float magnitude;

    public float speed; // Speed of movement
    public float currentSpeed;
    public float AtoB;
    public Vector3 AtoB1;
    public Vector3 FootToTarget;
    public float FootToTargetMagnitude;
    public float DistansToGoal;

    private Camera TheCamera;

    void Start()
    {
        //camera
        TheCamera = Camera.main;
        TheCamera.enabled = true;
        AtoB1 = targetPosition - startingPosition;
        AtoB = AtoB1.magnitude;
    }

    private void OnMouseDown()
    {
        if (test == startingPosition)
        {
            test = targetPosition;
        }
        else
        {
            test = startingPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FootToTarget = transform.position - test;
        FootToTargetMagnitude = FootToTarget.magnitude;
        DistansToGoal = FootToTarget.magnitude / AtoB;


        // Move the object towards the target position
        transform.position = Vector3.MoveTowards(transform.position, test, speed * Time.deltaTime * (DistansToGoal * 5f));
        //speed * Time.deltaTime


    }
}