using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
 private InputManager _inputManager;
 public Transform _targetTransform; //The object the camera will follow
 public Transform _cameraPivot; // the object the camera uses to pivot
 public Transform _cameraTransform; //transform of camera object in the scene
 public LayerMask _collisionLayer; //Layers the camera will collide with
 private float _defaultPosition;
 private Vector3 _cameraFollowVelocity = Vector3.zero;
 private Vector3 _cameraVectorPosion;

 public float _cameraCollisionOffset = 0.2f; // how much the camera will jump off an object it collides with
 public float minimumCollisionOffet = 0.2f;
 public float _cameraCollisionRadius = 0.2f;
 public float _camerafollowSpeed = 0.2f;
 public float _cameraLookSpeed = 2;
 public float _cameraPivotSpeed = 2;
 
 public float _lookAngle;
 public float _pivotAngle;
 
 public float _minimumPivotAngle = -35f;
 public float _maximumPivotAngle = 35f;

 private void Awake()
 {
  _inputManager = FindObjectOfType<InputManager>();
  _targetTransform = FindObjectOfType<PlayerManager>().transform;
  _cameraTransform = Camera.main.transform;
  _defaultPosition = _cameraTransform.localPosition.z;
 }

 public void HandleAllCameraMovement()
 {
   FollowTarget();
   RotateCamera();
   HandleCameraCollision();
 }

 //used for the camera to follow the player
 private void FollowTarget()
 {
  Vector3 targetPosition = Vector3.SmoothDamp
  (transform.position, _targetTransform.position, ref _cameraFollowVelocity, _camerafollowSpeed);
  transform.position = targetPosition;
 }

 private void RotateCamera()
 {
  Vector3 rotation;
  Quaternion targetRotation;
  _lookAngle = _lookAngle + (_inputManager.cameraInputX * _cameraLookSpeed);
  _pivotAngle = _pivotAngle - (_inputManager.cameraInputY * _cameraPivotSpeed);
  _pivotAngle = Mathf.Clamp(_pivotAngle, _minimumPivotAngle, _maximumPivotAngle);
  
   rotation = Vector3.zero;
  rotation.y = _lookAngle;
  targetRotation = Quaternion.Euler(rotation);
  transform.rotation = targetRotation;

  rotation = Vector3.zero;
  rotation.x = _pivotAngle;
  targetRotation = Quaternion.Euler(rotation);
  _cameraPivot.localRotation = targetRotation;
 }

 private void HandleCameraCollision()
 {
  //pushing camera forward or backward if it collides with an object
  float targetPosition = _defaultPosition;
  RaycastHit hit;
  Vector3 direction = _cameraTransform.position - _cameraPivot.position; 
  direction.Normalize();
  
  if(Physics.SphereCast(
      _cameraPivot.transform.position, _cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), _collisionLayer))
  {
   float distance = Vector3.Distance(_cameraPivot.position, hit.point);
   targetPosition =- (distance - _cameraCollisionOffset); ;
  }

  if (Mathf.Abs(targetPosition) < minimumCollisionOffet)
  {
   targetPosition = targetPosition - minimumCollisionOffet;
  }
  _cameraVectorPosion.z = Mathf.Lerp(_cameraTransform.localPosition.z, targetPosition, 0.2f);
  _cameraTransform.localPosition = _cameraVectorPosion;
 }
}
