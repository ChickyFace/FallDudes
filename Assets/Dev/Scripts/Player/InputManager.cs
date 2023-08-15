using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerLocomotion playerLocomotion;
    private PlayerInputActions playerInputActions;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool jump_Input;
    

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();    
        animatorManager = GetComponent<AnimatorManager>();
    }
    private void OnEnable()
    {
        if(playerInputActions == null)
        {
            playerInputActions = new PlayerInputActions();
            playerInputActions.Player.Move.performed += i => movementInput = i.ReadValue<Vector2>();
            playerInputActions.Action.Jump.performed += i => jump_Input = true;
        }

        playerInputActions.Enable();
    }
    private void OnDisable()
    {
        playerInputActions.Disable();
    }
    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleJumpInput();
        //HanleActionInput
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
    private void HandleJumpInput()
    {
        if (jump_Input)
        {
            jump_Input = false;
            playerLocomotion.HandleJumping();
        }
    }

}




