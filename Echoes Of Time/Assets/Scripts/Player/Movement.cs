using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AnimationStates
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
    Player_Spear_Idle,
    Player_Spear_Run,
    Player_Spear_Jump,
    Player_Spear_Fall,
    Player_Spear_Climb,
    Player_Spear_Roll,
    Player_Spear_Die,
    Player_Sword_Idle,
    Player_Sword_Run,
    Player_Sword_Jump,
    Player_Sword_Fall,
    Player_Sword_Climb,
    Player_Sword_Roll,
    Player_Sword_Die,
   
}
//public enum Weapons
//{
//    Sword,
//    Spear,
//    Bow,
//    None
//}

/// <summary>
/// Script which manages the movement of the player
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Movement : MonoBehaviour
{
    [HideInInspector] public Vector2 input;

    [Header("Movement Settings")]
    [Range(1, 10)] public float walkSpeed;
    [Range(1,10)] public float sprintSpeed;
    [Range(1,10)] public float jumpForce;
    public float customTimeScale;
    private Animator animator;
    private Rigidbody2D rb;
    private Dictionary<Actions.Weapons, Dictionary<string, AnimationStates>> weaponMovementAnims = new();
    public Actions.Weapons currentWeapon;
    public AnimationStates currentState;


    private bool jumpInput;
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
       rb = GetComponent<Rigidbody2D>();
       groundCheck = transform.Find("groundcheck");
        animator = GetComponent<Animator>();
        currentWeapon = GetComponent<Actions>().currentWeapon;
        InitialiseWeaponAnims();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundcheckDistance, groundLayer);
        currentWeapon = GetComponent<Actions>().currentWeapon;
        Vector2 move = Move();
        CalculateGroundChecks();

        if(move.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            SetAnimationState(currentWeapon, "Player_Run");
        }
        if(move.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;    ////move into overarching check function 
            SetAnimationState(currentWeapon, "Player_Run"); 
        }
        else if(move.x == 0)
        {
            SetAnimationState(currentWeapon, "Player_Idle");
        }

       
       
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
       
       // Debug.Log("reading input"); 
    }

    private Vector2 Move()
    {
        Vector2 movement = input *walkSpeed * customTimeScale;
        movement.y = rb.velocity.y;
        rb.velocity = movement;
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
           Debug.Log("Jumping");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            SetAnimationState(currentWeapon, "Player_Jump");
            jumpInput = false;
            
        }
    }

    private void CalculateGroundChecks()
    {
       
        Jump();
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
        }
        else if (!isGrounded)
        {
            
            coyoteCounter -= Time.deltaTime;
           // Debug.Log(coyoteCounter);
        }
           
        //if(rb.velocityY < 0)
        //    SetAnimationState(currentWeapon, "Player_Fall");  ///little jittery 
    }
    
    private void InitialiseWeaponAnims()
    {
        weaponMovementAnims.Add(Actions.Weapons.None, new Dictionary<string, AnimationStates>
      {
          {"Player_Idle",AnimationStates.Player_Idle},
          {"Player_Run",AnimationStates.Player_Run},
          {"Player_Jump",AnimationStates.Player_Jump},
          {"Player_Fall",AnimationStates.Player_Fall},
          {"Player_Climb",AnimationStates.Player_Climb},
          {"Player_Roll",AnimationStates.Player_Roll},
          {"Player_Die",AnimationStates.Player_Die },
      });

        weaponMovementAnims.Add(Actions.Weapons.Sword, new Dictionary<string, AnimationStates>
        {
            {"Player_Idle",AnimationStates.Player_Sword_Idle },
            {"Player_Run",AnimationStates.Player_Sword_Run},
            {"Player_Jump",AnimationStates.Player_Sword_Jump},
            {"Player_Fall",AnimationStates.Player_Sword_Fall},
            {"Player_Climb",AnimationStates.Player_Sword_Climb},
            {"Player_Roll",AnimationStates.Player_Sword_Roll},
            {"Player_Die",AnimationStates.Player_Sword_Die },
        });

        weaponMovementAnims.Add(Actions.Weapons.Spear, new Dictionary<string, AnimationStates>
        {
            {"Player_Idle",AnimationStates.Player_Spear_Idle },
            {"Player_Run",AnimationStates.Player_Spear_Run},
            {"Player_Jump",AnimationStates.Player_Spear_Jump},
            {"Player_Fall",AnimationStates.Player_Spear_Fall},
            {"Player_Climb",AnimationStates.Player_Spear_Climb},
            {"Player_Roll",AnimationStates.Player_Spear_Roll},
            {"Player_Die",AnimationStates.Player_Spear_Die },
           
        });

        weaponMovementAnims.Add(Actions.Weapons.Bow, new Dictionary<string, AnimationStates>
        {
            {"Player_Idle",AnimationStates.Player_Bow_Idle },
            {"Player_Run",AnimationStates.Player_Bow_Run},
            {"Player_Jump",AnimationStates.Player_Bow_Jump},
            {"Player_Fall",AnimationStates.Player_Bow_Fall},
            {"Player_Climb",AnimationStates.Player_Bow_Climb},
            {"Player_Roll",AnimationStates.Player_Bow_Roll},
            {"Player_Die",AnimationStates.Player_Bow_Die },
        });
    }


    private void SetAnimationState(Actions.Weapons currentWeapon, string State)
    {
       if(weaponMovementAnims.ContainsKey(currentWeapon) && weaponMovementAnims[currentWeapon].ContainsKey(State))
        {
            ChangeAnimationState(weaponMovementAnims[currentWeapon][State]);
        }
    }
    private void ChangeAnimationState(AnimationStates newState)
    {
        if(currentState == newState) return;
        string state = newState.ToString();
        animator.Play(state);
        currentState = newState;
    }

    private void CheckWeaponForAnim()
    {

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}



}
