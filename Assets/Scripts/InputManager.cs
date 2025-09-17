using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{ 
    public InputManager _inputManager;
  private PlayerControls _playerControls;
  PlayerLocomotion _playerLocomotion;
  private AnimatorManager _animatorManager;
  
  public Vector2 _movementInput;
  public Vector2 CameraInput;


  public float cameraInputX;
  public float cameraInputY;
  
  public float moveAmount;
  public float _verticalInput;  
  public float _horizontalInput;
  
  private float _sprintPower = 10f;
  private float _sprintSpeed = 5f;

  public bool _sprintInput;




  private void Awake()
  {
      _inputManager = FindObjectOfType<InputManager>();
      _playerLocomotion = GetComponent<PlayerLocomotion>();
      _animatorManager = GetComponent<AnimatorManager>();
  }

  private void OnEnable()
  {
      if (_playerControls == null)
      {
          _playerControls = new PlayerControls();

          _playerControls.PlayerMovement.Movement.performed += i => 
              _movementInput = i.ReadValue<Vector2>();
          _playerControls.PlayerMovement.Camera.performed += i =>
              CameraInput = i.ReadValue<Vector2>();
          
          _playerControls.PlayerMovement.Sprint.performed += i => _sprintInput = true;
          _playerControls.PlayerMovement.Sprint.canceled += i => _sprintInput = false;
         
        
          
      }
    
      _playerControls.Enable();
      
  }

  private void OnDisable()
  {
      _playerControls.Disable();
  }

  public void HandleAllInputs()
  {
      HandleMovementInput();
      HandleSprintInput();
  }

  private void HandleMovementInput()
  {
      _verticalInput = _movementInput.y;
      _horizontalInput = _movementInput.x;
      
      //Camera Movement
      cameraInputY = CameraInput.y;
      cameraInputX = CameraInput.x;
      
      moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput));
      _animatorManager.UpdateAnimatorValues(_horizontalInput, _verticalInput, _playerLocomotion._isSprinting);
      
  }

  //Method to make player sprint
  private void HandleSprintInput()
  {
      if (_sprintInput && moveAmount > 0.5f)
      {
          _playerLocomotion._isSprinting = true;
      }
      else
      {
          _playerLocomotion._isSprinting = false;
      }
  }
}
