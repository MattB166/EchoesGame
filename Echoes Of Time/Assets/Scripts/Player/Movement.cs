using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum AnimationTypes
{
    Player_Idle,
    Player_Run,
    Player_Jump,
    Player_Fall,
    Player_Climb,
    Player_Roll,
    Player_Die,
   Player_Bow_Idle,
   Player_Bow_Run,
   Player_Bow_Jump,
   Player_Bow_Fall,
   Player_Bow_Climb,
   Player_Bow_Roll,
   Player_Bow_Die,
   Player_Bow_Attack,
   Player_Sword_Idle,
   Player_Sword_Run,
   Player_Sword_Jump,
   Player_Sword_Fall,
   Player_Sword_Climb,
   Player_Sword_Roll,
   Player_Sword_Die,
   Player_Sword_Attack1,
   Player_Sword_Attack2,
   Player_Sword_Attack3,
   Player_Spear_Idle,
   Player_Spear_Run,
   Player_Spear_Jump,
   Player_Spear_Fall,
   Player_Spear_Climb,
   Player_Spear_Roll,
   Player_Spear_Die,
   Player_Spear_Attack1,
   Player_Spear_Attack2,
   Player_Spear_Throw,
}
[System.Serializable]
public struct AnimationContainer
{
    public string name;
    public List<AnimationTypes> animations;
  
}
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
    private Animator animator;
    private string currentState;
    public List<AnimationContainer> animationContainers;
    public Dictionary<string, Animation> MainAnims; //dictionary of basic animations
    public Dictionary<string, Animation> BowAnims; //dictionary of Bow animations
    public Dictionary<string, Animation> SwordAnims; //dictionary of Sword animations
    public Dictionary<string, Animation> SpearAnims; //dictionary of Spear animations


    private bool jumpInput;
    private float yVelocity;
    private bool isJumping;
    [HideInInspector] public bool isGrounded;
    [Space(10)]
    [Header("Jump Settings")]
    [Range(-20f,-0.05f)] public float gravity;
    [Range(0.01f,0.5f)] public float groundcheckDistance;
    public float coyoteTime = 0.5f;
    private float coyoteCounter;
    //add jump buffer next 
    public LayerMask groundLayer;
    private Transform groundCheck;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.Find("groundcheck");
        animator = GetComponent<Animator>();
      
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
       // Debug.Log("reading input"); 
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
        if (coyoteCounter > 0 && context.performed)
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
        //Debug.Log(isGrounded);
        Jump();
        if (isGrounded && yVelocity < 0)
        {
            yVelocity = 0f;
            coyoteCounter = coyoteTime;
        }
        else if (!isGrounded)
        {
            yVelocity += gravity * Time.deltaTime * customTimeScale;
            coyoteCounter -= Time.deltaTime;
            Debug.Log(coyoteCounter);
        }
           
    }
    

    private void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }
}
