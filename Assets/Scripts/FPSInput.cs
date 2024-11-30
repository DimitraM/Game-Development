using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPSInput")]

public class FPSInput : MonoBehaviour
{
    private CharacterController charController;

    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.5f;  // Jump height
    
    private int jumpCount = 0;
    private float verticalVelocity = 0; // Tracks the player's vertical velocity


    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;

        Vector3 movement = new Vector3(deltaX, gravity, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        movement = transform.TransformDirection(movement);

  
        // Apply gravity
        if (charController.isGrounded || jumpCount >2)
        {
            verticalVelocity = 0; // Reset vertical velocity when on the ground
            jumpCount =0;

            // Handle one jump
            if (Input.GetButtonDown("Jump") )
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount++;
            }
        }
        else 
        { 
             // Allow double jump while in the air
            if (jumpCount < 2 && Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCount++; // Increment jump count for the second jump
            }
            // Apply gravity when not grounded
            verticalVelocity += gravity * Time.deltaTime;
            
        }

        // Add vertical velocity to movement
        movement.y = verticalVelocity;

        // Move the character
        charController.Move(movement * Time.deltaTime);
    
    }
}
