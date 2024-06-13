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

        // Local variables
        private Controls controls;

        private void OnEnable()
        {
            // Create a new instance of the controls and set up the callbacks
            if (controls == null)
            {
                controls = new Controls();
                controls.Player.SetCallbacks(this);
            }

            // Enable the Player action map
            controls.Player.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }
}
