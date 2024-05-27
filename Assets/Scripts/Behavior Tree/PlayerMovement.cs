using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotationSpeed = 700.0f;
    public float gravity = 20.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Handle movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (characterController.isGrounded)
        {
            // Get input direction based on the current camera's perspective
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;

            // Normalize direction vectors
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // Calculate movement direction
            moveDirection = (forward * vertical + right * horizontal);
            moveDirection *= speed;
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the player
        characterController.Move(moveDirection * Time.deltaTime);

        // Handle rotation
        if (horizontal != 0f || vertical != 0f)
        {
            Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

}
