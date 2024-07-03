using Cinemachine;
using eXplorerJam.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Serialized References

    [Header("References")] [SerializeField]
    private CharacterController characterController;

    [SerializeField] private InputReader inputReader;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private Animator animator;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private MapManager mapManager;

    #endregion

    #region Serialized Settings

    // TODO: Implement the following fields
    [Header("Settings")] [Tooltip("Move speed of the character in m/s")] [SerializeField]
    private float moveSpeed = 4.0f;

    [Tooltip("Sprint speed of the character in m/s")] [SerializeField]
    private float sprintSpeed = 6.0f;

    [Tooltip("Terminal speed of the character in m/s")] [SerializeField]
    private float verticalTerminalSpeed = 50f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")] [SerializeField]
    private float gravity = -9.81f;

    [Tooltip("Dash speed of the character")] [SerializeField]
    private float dashSpeed = 30.0f;

    [Tooltip("Time required to pass before finishing the dash.")] [SerializeField]
    private float dashTime = 0.1f;
    
    [Tooltip("Number of avaliable dashes")]
    [SerializeField]
    private int remainingDashes = MAX_DASHES;

    [Tooltip("Time required to pass before being able to dash again. Set to 0f to instantly dash again")]
    [SerializeField]
    private float dashCooldown = 3.0f;
    
    [Tooltip("Time required to pass before being able to dash again. Set to 0f to instantly dash again")]
    [SerializeField]
    private float dashTimeout = 0.5f;

    [Tooltip("Jump height of the character")] [SerializeField]
    private float jumpHeight = 3.0f;

    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField]
    private float jumpTimeout = 0.3f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")] [SerializeField]
    private float fallTimeout = 0.15f;

    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField]
    private bool playerGrounded = true;

    [Tooltip("Number of avaliable jumps")]
    [SerializeField]
    private int remainingJumps = MAX_JUMPS;

    [Tooltip("Useful for rough ground")] [SerializeField]
    private float groundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")] [SerializeField]
    private float groundedRadius = 0.5f;

    [Tooltip("What layers the character is considered grounded on")] [SerializeField]
    private LayerMask GroundLayers;

    [Header("Animation Smoothing")]

    [Tooltip("Smooth time for the animation blend tree")] [SerializeField]
    private float animationSmoothTime = 0.05f;

    #endregion

    #region Local variables

    private Vector2 previousMovementInput;
    private Transform virtualCameraTransform;

    private bool isSprinting;
    private float dashTimeoutDelta;
    private float dashCooldownDelta;
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    private float verticalSpeed;

    // Animation related variables
    private int moveXAnimationParameterId; // Store the animator parameter id for movement x
    private int moveZAnimationParameterId; // Store the animator parameter id for movement z
    private int jumpAnimationParameterId; // Store the animator parameter id for jumping
    private int freeFallAnimationParameterId; // Store the animator parameter id for free falling
    private int groundAnimationParameterId; // Store the animator parameter id for grounded

    private Vector2 currentAnimationBlendVector; // Store the current animation blend vector
    private Vector2 animationBlendVelocity; // Store the velocity of the blend vector
    
    private static readonly int OnDeath = Animator.StringToHash("onDeath");
    private static readonly int OnRespawn = Animator.StringToHash("onRespawn");
    private static readonly int OnSpawn = Animator.StringToHash("OnSpawn");

    #endregion

    #region Constants

    private const int MAX_JUMPS = 2;
    private const int MAX_DASHES = 2;
    private const int NO_REMAINING = 0;
    private const float NO_TIME = 0f;
    private const float ZERO_X = 0f;
    private const float ZERO_Z = 0f;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        pauseManager = FindObjectOfType<PauseManager>();

        // Subscribe to the MoveEvent specified in InputReader.cs
        inputReader.MoveEvent += HandleMove;
        inputReader.SprintEvent += HandleSprint;
        inputReader.PauseEvent += HandlePause;

        // Set the virtualcamera's transform
        virtualCameraTransform = cinemachineVirtualCamera.transform;

        // Reset the timeouts
        dashTimeoutDelta = dashTimeout;
        dashCooldownDelta = dashCooldown;
        jumpTimeoutDelta = jumpTimeout;
        fallTimeoutDelta = fallTimeout;

        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");
        jumpAnimationParameterId = Animator.StringToHash("Jump");
        freeFallAnimationParameterId = Animator.StringToHash("FreeFall");
        groundAnimationParameterId = Animator.StringToHash("Grounded");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        PlayerSpawn();
    }

    private void Update()
    {
        GroundedCheck(); // Check if the player is grounded
        MovePlayer(); // Poll for movement every frame
        Jump(); // Poll for jumping every frame
    }

    private void OnEnable()
    {
        GameManager.OnDeath += PlayerDeath;
        GameManager.OnRespawn += PlayerRespawn;
        GameManager.OnSpawn += PlayerSpawn;
        GameManager.OnPause += PlayerPause;
        GameManager.OnResume += PlayerResume;
    }

    private void OnDisable()
    {
        GameManager.OnDeath -= PlayerDeath;
        GameManager.OnRespawn -= PlayerRespawn;
        GameManager.OnSpawn -= PlayerSpawn;
        GameManager.OnPause -= PlayerPause;
        GameManager.OnResume -= PlayerResume;
    }

    #endregion

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
        
        Vector2 movementInputForAnimationBlend = new Vector2(previousMovementInput.x, previousMovementInput.y);
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector,
                                                            movementInputForAnimationBlend,
                                                            ref animationBlendVelocity,
                                                            animationSmoothTime);

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

        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);
        
        if (inputReader.dash && dashTimeoutDelta <= NO_TIME && remainingDashes > NO_REMAINING)
        {
            remainingDashes--;
            StartCoroutine(Dash(finalMoveDirection));
                   
            inputReader.dash = false;
            dashTimeoutDelta = dashTimeout;
            dashCooldownDelta = dashCooldown;
        }
        
        if (dashTimeoutDelta >= NO_TIME)
        {
            dashTimeoutDelta -= Time.deltaTime;
        }
        if (dashCooldownDelta >= NO_TIME)
        {
            dashCooldownDelta -= Time.deltaTime;
        }
        else
        {
            if(remainingDashes>=MAX_DASHES) return;
            remainingDashes++;
            dashCooldownDelta = dashCooldown;
        }
        
    }

    private IEnumerator Dash(Vector3 finalMoveDirection)
    {
        var startTime = Time.time;
        while (Time.time < startTime+dashTime)
        {
            characterController.Move(finalMoveDirection * (moveSpeed * dashSpeed * Time.deltaTime) +
                                     new Vector3(ZERO_X, verticalSpeed, ZERO_Z) * Time.deltaTime);
            
            yield return null;
        }
    }

    #endregion

    #region Player Jump Mechanics

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z);

        playerGrounded =
            Physics.CheckSphere(spherePosition, groundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        // Debug.Log($"Grounded: {playerGrounded}, Sphere Position: {spherePosition}");
        animator.SetBool(groundAnimationParameterId, playerGrounded);
    }

    private void Jump()
    {
        if (!playerGrounded)
        {
            if (remainingJumps <= NO_REMAINING)
            {
                jumpTimeoutDelta = jumpTimeout;
            }

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            } else
            {
                //Debug.Log("Jam is free falling");
                animator.SetBool(freeFallAnimationParameterId, true);
            }
        }
        else // If the player is grounded
        {
            if (verticalSpeed < 0.0f)
            {
                verticalSpeed = -2f;
                remainingJumps = MAX_JUMPS;
            }

            fallTimeoutDelta = fallTimeout;
            //Debug.Log("Jam is grounded");
            animator.SetBool(jumpAnimationParameterId, false);
            animator.SetBool(freeFallAnimationParameterId, false);
        }

        if (inputReader.jump && jumpTimeoutDelta <= 0.0f && remainingJumps > NO_REMAINING)
        {
            //Debug.Log("Jam is Jumping");
            animator.SetBool(jumpAnimationParameterId, true);
            remainingJumps--;
            verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
            inputReader.jump = false;
            jumpTimeoutDelta = jumpTimeout;
        }

        if (jumpTimeoutDelta >= 0.0f)
        {
            jumpTimeoutDelta -= Time.deltaTime;
        }

        if (verticalSpeed < verticalTerminalSpeed)
        {
            verticalSpeed += gravity * Time.deltaTime;
        }
    }

    #endregion
    
    #region Player State Mechanics

    private void PlayerDeath()
    {
        PlayerPause();
        AnimPlayerDeath();
    }
    
    private void PlayerRespawn()
    {
        PlayerSpawn();
        PlayerResume();
        AnimPlayerRespawn();
    }
    
    private void PlayerSpawn()
    {
        if (GameManager.Instance == null) return;
        characterController.enabled = false;
        transform.position = GameManager.Instance.CheckpointPosition;
        characterController.enabled =true;
    }

    private void PlayerPause()
    {
        inputReader.PausedInputs();
    }
    
    private void PlayerResume()
    {
        inputReader.ResumeInputs();
    }
    
    #endregion
    
    #region Player AnimationTriggers

    private void AnimPlayerDeath()
    {
        animator.SetTrigger(OnDeath);
    }
    
    private void AnimPlayerRespawn()
    {
        animator.SetTrigger(OnRespawn);
    }
    #endregion

    #region Pause and Map Callbacks
    private void HandlePause()
    {
        if (pauseManager != null)
        {
            pauseManager.TogglePause();
        }
    }

    private void HandleMap()
    {         
        if (mapManager != null)
        {
            mapManager.ToggleMap();
        }
    }
    #endregion

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