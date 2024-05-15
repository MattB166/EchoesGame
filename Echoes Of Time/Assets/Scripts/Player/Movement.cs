using System.Collections;
using System.Collections.Generic;
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
    Player_Bow_Attack,
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
    private Dictionary<Actions.Weapons, Dictionary<string, AnimationStates>> weaponsAnims = new();
    public Actions.Weapons currentWeapon;
    public AnimationStates currentState;


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
        currentWeapon = GetComponent<Actions>().currentWeapon;
        InitialiseWeaponAnims();
    }

    // Update is called once per frame
    void Update()
    {
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
        else if(move.x == 0 && isGrounded)
        {
            SetAnimationState(currentWeapon, "Player_Idle");
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
    
    private void InitialiseWeaponAnims()
    {
        weaponsAnims.Add(Actions.Weapons.None, new Dictionary<string, AnimationStates>
      {
          {"Player_Idle",AnimationStates.Player_Idle},
          {"Player_Run",AnimationStates.Player_Run},
          {"Player_Jump",AnimationStates.Player_Jump},
          {"Player_Fall",AnimationStates.Player_Fall},
          {"Player_Climb",AnimationStates.Player_Climb},
          {"Player_Roll",AnimationStates.Player_Roll},
          {"Player_Die",AnimationStates.Player_Die },
      });

        weaponsAnims.Add(Actions.Weapons.Sword, new Dictionary<string, AnimationStates>
        {
            {"Player_Idle",AnimationStates.Player_Sword_Idle },
            {"Player_Run",AnimationStates.Player_Sword_Run},
            {"Player_Jump",AnimationStates.Player_Sword_Jump},
            {"Player_Fall",AnimationStates.Player_Sword_Fall},
            {"Player_Climb",AnimationStates.Player_Sword_Climb},
            {"Player_Roll",AnimationStates.Player_Sword_Roll},
            {"Player_Die",AnimationStates.Player_Sword_Die },
            {"Player_Attack1",AnimationStates.Player_Sword_Attack1 },
            {"Player_Attack2",AnimationStates.Player_Sword_Attack2 },
            {"Player_Attack3",AnimationStates.Player_Sword_Attack3}
        });

        weaponsAnims.Add(Actions.Weapons.Spear, new Dictionary<string, AnimationStates>
        {
            {"Player_Idle",AnimationStates.Player_Spear_Idle },
            {"Player_Run",AnimationStates.Player_Spear_Run},
            {"Player_Jump",AnimationStates.Player_Spear_Jump},
            {"Player_Fall",AnimationStates.Player_Spear_Fall},
            {"Player_Climb",AnimationStates.Player_Spear_Climb},
            {"Player_Roll",AnimationStates.Player_Spear_Roll},
            {"Player_Die",AnimationStates.Player_Spear_Die },
            {"Player_Attack1",AnimationStates.Player_Spear_Attack1 },
            {"Player_Attack2",AnimationStates.Player_Spear_Attack2 },
            {"Player_Throw",AnimationStates.Player_Spear_Throw}
        });

        weaponsAnims.Add(Actions.Weapons.Bow, new Dictionary<string, AnimationStates>
        {
            {"Player_Idle",AnimationStates.Player_Bow_Idle },
            {"Player_Run",AnimationStates.Player_Bow_Run},
            {"Player_Jump",AnimationStates.Player_Bow_Jump},
            {"Player_Fall",AnimationStates.Player_Bow_Fall},
            {"Player_Climb",AnimationStates.Player_Bow_Climb},
            {"Player_Roll",AnimationStates.Player_Bow_Roll},
            {"Player_Die",AnimationStates.Player_Bow_Die },
            {"Player_Attack",AnimationStates.Player_Bow_Attack }
        });
    }


    private void SetAnimationState(Actions.Weapons currentWeapon, string State)
    {
       if(weaponsAnims.ContainsKey(currentWeapon) && weaponsAnims[currentWeapon].ContainsKey(State))
        {
            ChangeAnimationState(weaponsAnims[currentWeapon][State]);
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


}
