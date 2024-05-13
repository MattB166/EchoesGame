using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script which manages the movement of the player
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [HideInInspector] public Vector2 input;
    private CharacterController controller;

    [Header("Movement Settings")]
    [Range(1, 10)] public float walkSpeed;
    [Range(1,10)] public float sprintSpeed;
    [Range(1,10)] public float jumpForce;
    public float customTimeScale;

    [Header("Jump Settings")]
    private bool jumpInput;
    private float yVelocity;
    private bool isJumping;
    [HideInInspector] public bool isGrounded;
    [Range(-20f,-0.05f)] public float gravity;
    [Range(0.01f,0.5f)] public float groundcheckDistance;
    public LayerMask groundLayer;
    private Transform groundCheck;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("groundcheck");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = Move();
        CalculateGroundChecks();

        if(move.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(move.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;    ////move into overarching check function 
        }
        controller.Move(new Vector2(move.x, yVelocity) * Time.deltaTime * customTimeScale);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        Debug.Log("reading input"); 
    }

    private Vector2 Move()
    {
        Vector2 movement = input *walkSpeed;
        movement.Normalize();
        movement*=walkSpeed;
        return movement;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
            jumpInput = true;
        else if(context.canceled)
            jumpInput = false;
    }

    private void Jump()
    {
        if(jumpInput)
        {
            yVelocity = Mathf.Sqrt(jumpForce * -2 * gravity);
            jumpInput = false;
        }
    }

    private void CalculateGroundChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundcheckDistance, groundLayer);
        Debug.Log(isGrounded);
        Jump();
        if (isGrounded && yVelocity < 0)
        {
            yVelocity = 0f;
        }
        else if (!isGrounded)
            yVelocity += gravity * Time.deltaTime * customTimeScale;
    }
    
}
