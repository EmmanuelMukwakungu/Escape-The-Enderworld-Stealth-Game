using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
   private InputManager _inputManager;
   private Vector3 _moveDirection;
   private Transform _cameraObject;
   private Rigidbody _playerRigidbody;

   public float _walkingSpeed = 1.5f;
   public float _runningSpeed = 5f;
   public float _sprintingSpeed = 11f;
   public float _rotationSpeed = 15f;
   
   public bool _isSprinting;
   public bool _isCrouching;
   public float _crounchSpeed = 1f;
   
   private void Awake()
   {
      _inputManager = GetComponent<InputManager>();
      _playerRigidbody = GetComponent<Rigidbody>();
      _cameraObject = Camera.main.transform;
      

   }
   
   public void HandleAllMovement()
   {
        HandleMovement();
        HandleRotation();
   }

   private void HandleMovement()
   {
      _moveDirection = _cameraObject.forward * _inputManager._verticalInput;
      _moveDirection = _moveDirection + _cameraObject.right * _inputManager._horizontalInput;
      _moveDirection.Normalize();
      _moveDirection.y = 0;

      if (_isSprinting)
      {
         _moveDirection = _moveDirection * _sprintingSpeed;
      }
      else if (_isCrouching)
      {
         _moveDirection = _moveDirection * _crounchSpeed;
      }
      else
      {
         if (_inputManager.moveAmount >= 0.5f)
         {
            _moveDirection = _moveDirection * _runningSpeed;
         }
         else
         {
            _moveDirection = _moveDirection * _walkingSpeed;
         }
      }

     

      Vector3 movementVelocity = _moveDirection;
      _playerRigidbody.linearVelocity = movementVelocity;
   }

   private void HandleRotation()
   {
      Vector3 _targetDirection = Vector3.zero;
      _targetDirection = _cameraObject.forward * _inputManager._verticalInput;
      _targetDirection = _targetDirection + _cameraObject.right * _inputManager._horizontalInput;
      _targetDirection.Normalize();
      _targetDirection.y = 0;

      if (_targetDirection == Vector3.zero)
         _targetDirection = transform.forward;

      Quaternion _targetRotation = Quaternion.LookRotation(_targetDirection);
      Quaternion _playerRotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

      transform.rotation = _playerRotation;
   }

   
}
