using Cinemachine;
using eXplorerJam.Input;
using System;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Serialized fields
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private grapple grapple;

    // TODO: Implement the following fields
    [Header("Settings")]
    [Tooltip("Move speed of the character in m/s")]
    [SerializeField] private float moveSpeed = 4.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    [SerializeField] private float sprintSpeed = 10.0f;

    [Tooltip("Terminal speed of the character in m/s")]
    [SerializeField] private float verticalTerminalSpeed = 50f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [SerializeField] private float gravity = -9.81f;

    [Tooltip("Jump height of the character")]
    [SerializeField] private float jumpHeight = 3.0f;

    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField] private float jumpTimeout = 0.3f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    [SerializeField] private float fallTimeout = 0.15f;

    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField] private bool playerGrounded = true;

    [Tooltip("Useful for rough ground")]
    [SerializeField] private float groundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    [SerializeField] private float groundedRadius = 0.5f;

    [Tooltip("What layers the character is considered grounded on")]
    [SerializeField] private LayerMask GroundLayers;

    // Local variables
    private Vector2 previousMovementInput;
    private Transform virtualCameraTransform;

    private bool isSprinting;
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    private float verticalSpeed;

    private void Start()
    {
        // Subscribe to the MoveEvent specified in InputReader.cs
        inputReader.MoveEvent += HandleMove;
        inputReader.SprintEvent += HandleSprint;

        // Set the virtualcamera's transform
        virtualCameraTransform = cinemachineVirtualCamera.transform;

        // Reset the timeouts
        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;
    }

    private void Update()
    {
        groundedCheck(); // Check if the player is grounded
        MovePlayer(); // Poll for movement every frame
        Jump(); // Poll for jumping every frame
        Grapple();
    }

    #region Player Movement Mechanics
    /// <summary>
    /// Updates the player movement vector based on the input vector
    /// </summary>
    /// <param name="vector">Movement input vector</param>
    private void HandleMove(Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }

    private void HandleSprint(bool sprinting) 
    { 
        isSprinting = sprinting;
    }

    /// <summary>
    /// Moves the player based on the input vector. The camera's current orientation is taken into account
    /// As well as sprinting and jumping inputs
    /// </summary>
    private void MovePlayer()
    { 
        // Store the player's movement input
        Vector3 desiredMoveDirection = new Vector3(previousMovementInput.x, 0, previousMovementInput.y);

        desiredMoveDirection.y = 0f;
        desiredMoveDirection.Normalize();

        Vector3 cameraForward = virtualCameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 cameraRight = virtualCameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
        Vector3 finalMoveDirection = cameraForward * desiredMoveDirection.z + cameraRight * desiredMoveDirection.x;

        if (desiredMoveDirection.magnitude > 1f)
        {
            desiredMoveDirection.Normalize();
        }

        float sprintSpeedMultiplier = isSprinting ? sprintSpeed : 1f;

        // Move the player and consider jumping by applying the vertical speed in the final vector
        characterController.Move(finalMoveDirection * (moveSpeed * sprintSpeedMultiplier * Time.deltaTime) +
                                 new Vector3(0.0f, verticalSpeed, 0.0f) * Time.deltaTime);
    }
    #endregion

    #region Player Jump Mechanics

    private void groundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, 
                                             transform.position.y - groundedOffset,
                                             transform.position.z);

        playerGrounded = Physics.CheckSphere(spherePosition, groundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        Debug.Log($"Grounded: {playerGrounded}, Sphere Position: {spherePosition}");
    }

    private void Jump()
    {
        if (playerGrounded)
        {
            if (verticalSpeed < 0.0f)
            {
                verticalSpeed = -2f;
            }

            fallTimeoutDelta = fallTimeout;

            if (inputReader.Jump && jumpTimeoutDelta <= 0.0f)
            {
                verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
                inputReader.Jump = false;
            }

            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = jumpTimeout;

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }

            inputReader.Jump = false;
        }

        if (verticalSpeed < verticalTerminalSpeed)
        {
            verticalSpeed += gravity * Time.deltaTime;
        }
    }
    #endregion

    private void Grapple()
    {

        if (inputReader.Grapple == true)
        {
            grapple.StartGrapple();
            
        }
        
    }
    #region Debug Helpers
    // TODO: Delete these before submission
    private void OnDrawGizmos()
    {
        Vector3 spherePosition = new Vector3(transform.position.x,
                                             transform.position.y - groundedOffset,
                                             transform.position.z);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spherePosition, groundedRadius);
    }
    #endregion
}
