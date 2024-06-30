using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using eXplorerJam.Input;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;

public class grapple : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputReader inputReader;
    [Tooltip("The point the rope emits from")]
    public Transform wrist;
    public Transform player;


    [Tooltip("Objects in this layer can be grappled onto")]
    public LayerMask isGrappable;
    

    [Header("Settings")]
    [Tooltip("How far the grapple rope can go")]
    public float maxGrappleDistance;
    [Tooltip("Time between mouse click and player movement")]
    public float grappleDelayTime;
    [Tooltip("The curve of the y axis path the player takes to get to the target position")]
    public float overshootYAxis;
    
    //local variables
    private bool isGrappling;
    private Vector3 mousePosition;
    private Vector3 grapplePoint;
    private LineRenderer lineRenderer;
    private Vector3 velocityToSet;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        lineRenderer = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, wrist.position);  
        }
    }

    public void StartGrapple()
    {
        isGrappling = true;
        Vector3 mousePos = Mouse.current.position.ReadValue();

        RaycastHit hit; ;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if(Physics.Raycast(ray, out hit, maxGrappleDistance, isGrappable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = player.position + player.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grapplePoint);
    }

    public void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false;


    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        //move player
        isGrappling = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        characterController.Move(velocityToSet * Time.deltaTime);
        Invoke(nameof(SetVelocity), 0.1f);
    }

    //private Vector3 velocityToSet;
   
    private void SetVelocity()
    {
        Vector3 characterVelocity = characterController.velocity;
        characterVelocity = velocityToSet;
    }
}
