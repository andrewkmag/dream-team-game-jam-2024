using eXplorerJam.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static eXplorerJam.Input.Controls;

namespace eXplorerJam.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        // Events
        public event Action<Vector2> MoveEvent;
        public event Action<bool> SprintEvent;

        // Local variables
        private Controls _controls;
        public bool Grapple;
        public bool jump;
        public bool dash;
        public bool interact;

        private void OnEnable()
        {
            // Create a new instance of the controls and set up the callbacks
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }

            // Enable the Player action map
            _controls.Player.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            //read the sprint value as a bool (button press) and invoke sprintEvent
            bool isSprinting = context.ReadValueAsButton();
            SprintEvent?.Invoke(isSprinting);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            jump = context.phase switch
            {
                InputActionPhase.Started => true,
                InputActionPhase.Canceled => false,
                _ => jump
            };
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            dash = context.phase switch
            {
                InputActionPhase.Started => true,
                InputActionPhase.Canceled => false,
                _ => dash
            };
        }


        public void OnInteract(InputAction.CallbackContext context)
        {
            interact = context.phase switch
            {
                InputActionPhase.Started => true,
                InputActionPhase.Canceled => false,
                _ => interact
            };
        }

        public void OnGrapple(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                Grapple = true;
            }
            else if(context.phase == InputActionPhase.Canceled) 
            { 
                Grapple = false; 
            }
        }
    }
}
