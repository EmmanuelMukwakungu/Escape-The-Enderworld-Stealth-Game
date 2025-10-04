using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator _animator;
    InputManager _inputManager;
     int horizontal;
     int vertical;
     

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _inputManager = GetComponent<InputManager>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        

    }

    public void UpdateAnimatorValues(float HorizontalMovement, float VerticalMovement, bool isSprinting)
    {
        float snapperHorizontal;
        float snapperVertical;
        

        // --- Snapping Horizontal ---
        if (HorizontalMovement > 0 && HorizontalMovement < 0.55f)
            snapperHorizontal = 0.5f;
        else if (HorizontalMovement >= 0.55f)
            snapperHorizontal = 1;
        else if (HorizontalMovement < 0 && HorizontalMovement > -0.55f)
            snapperHorizontal = -0.5f;
        else if (HorizontalMovement <= -0.55f)
            snapperHorizontal = -1;
        else
            snapperHorizontal = 0;

        // --- Snapping Vertical ---
        if (VerticalMovement > 0 && VerticalMovement < 0.55f)
            snapperVertical = 0.5f;
        else if (VerticalMovement >= 0.55f)
            snapperVertical = 1;
        else if (VerticalMovement < 0 && VerticalMovement > -0.55f)
            snapperVertical = -0.5f;
        else if (VerticalMovement <= -0.55f)
            snapperVertical = -1;
        else
            snapperVertical = 0;

        // --- Sprint override ---
        if (isSprinting)
        {
            snapperHorizontal = HorizontalMovement; // still allow strafe while sprinting
            snapperVertical = 2f; // IMPORTANT: must match Blend Tree threshold
        }

        // ✅ Apply snapped values
        _animator.SetFloat(horizontal, snapperHorizontal, 0.1f, Time.deltaTime);
        _animator.SetFloat(vertical, snapperVertical, 0.1f, Time.deltaTime);
       
        
    }
    
    public void UpdateCrouchAnimation(bool isCrouching)
    {
        _animator.SetBool("isCrouching", isCrouching);
    }


}
