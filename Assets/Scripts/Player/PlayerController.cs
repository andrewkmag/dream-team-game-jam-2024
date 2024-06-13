using eXplorerJam.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Serialized fields
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputReader inputReader;

    // TODO: Implement the following fields
    // [Header("Settings")]
    // [Tooltip("Move speed of the character in m/s")]
    // [SerializeField] private float moveSpeed = 4.0f;

    // Local variables
    private Vector2 previousMovementInput;


    private void Start()
    {
        // Subscribe to the MoveEvent specified in InputReader.cs
        inputReader.MoveEvent += HandleMove;
    }




    private void Update()
    {
        
    }

    /// <summary>
    /// Updates the player movement vector based on the input vector
    /// </summary>
    /// <param name="vector">Movement input vector</param>
    private void HandleMove(Vector2 movementInput)
    {
        previousMovementInput = movementInput;
    }

    private void MovePlayer()
    { 
        // Store the player's movement input
        Vector3 desiredMoveDirection = new Vector3(previousMovementInput.x, 0, previousMovementInput.y);

        // TODO: Move the player based on the input

    }
}
