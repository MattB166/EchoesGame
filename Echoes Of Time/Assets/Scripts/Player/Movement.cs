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

/// <summary>
/// Script which manages the movement of the player
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Movement : MonoBehaviour,IDistortable
{
    [HideInInspector] public Vector2 input;

    [Header("Movement Settings")]
    [Range(1, 10)] public float walkSpeed;
    [Range(1, 10)] public float sprintSpeed;
    [Range(1, 10)] public float dashSpeed;
    [Range(0.1f,0.5f)] public float dashDuration;
    [Range(0.0f, 5.0f)] public float dashCooldown;
    [Range(1, 10)] public float jumpForce;
    
    public float direction;
    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Dictionary<Actions.Weapons, Dictionary<string, AnimationStates>> weaponMovementAnims = new();
    public Actions.Weapons currentWeapon;
    public AnimationStates currentState;
    public bool isAttacking;

    private float airTime = 0.0f;
    private bool jumpInput;
    private bool isJumping;
    private bool doubleJumping;
    private bool dashInput;
    private bool isDashing = false;
    private bool canDash = true;
    private float initialZRotation;
    private bool climbInput;
    private bool canClimb = false;
    private bool isClimbing;
    public float climbSpeed;
    private bool descendInput;
    private bool canDescend = false;
    public float descendSpeed;
    private bool isDescending;
    private bool onClimbable;
    [HideInInspector] public bool isGrounded;
    [Space(10)]
    [Header("Jump Settings")]
    [Range(-20f, -0.05f)] public float gravity;
    private float internalGravity;
    [Range(0.01f, 0.5f)] public float groundcheckDistance;
    public float coyoteTime;
    private float coyoteCounter;
    //add jump buffer next 
    public LayerMask groundLayer;
    private Transform groundCheck;

    [SerializeField]
    private float customTimeScale;
    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }

            }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        groundCheck = transform.Find("groundcheck");
        animator = GetComponent<Animator>();
        currentWeapon = GetComponent<Actions>().currentWeapon;
        InitialiseWeaponAnims();
        isAttacking = GetComponent<Actions>().isAttacking;
        initialZRotation = transform.rotation.z;
        internalGravity = gravity;
        GetComponent<Actions>().attackAnimFinishedCallback += ResetAnimations;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundcheckDistance, groundLayer);
        currentWeapon = GetComponent<Actions>().currentWeapon;
        
        CalculateGroundChecks();
        CalculateMovementAnimationChecks();
        PerformDoubleJump();
        ApplyGravity();
        isAttacking = GetComponent<Actions>().isAttacking;
        
       
    }

    private void ResetAnimations(Actions.Weapons weapon) //re determines what animation should be playing after attacking
    {
        SetAnimationState(currentWeapon, "Player_Idle");
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    private Vector2 Move()
    {
        Vector2 movement;
        direction = GetComponent<SpriteRenderer>().flipX ? -1 : 1;
        if(input.x == 0 && isDashing)
        {
            
            float newSpeed = dashSpeed;
            movement = new Vector2(direction * newSpeed * customTimeScale, rb.velocity.y);
        }
        else
        {
            float speed = isDashing ? dashSpeed : walkSpeed;
            movement = input * speed * customTimeScale;
            movement.y = rb.velocity.y;
        }
        rb.velocity = movement;
        
        return movement;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (coyoteCounter > 0 && context.performed)
            jumpInput = true;
        if(coyoteCounter > 0 && !isGrounded && context.performed)
        {
            doubleJumping = true;
        }
        else if (context.canceled)
            jumpInput = false;
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
           
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }


    private void Jump()
    {
        if(jumpInput) ////change this so on one press it does a decent height jump, but on a hold it does a higher jump until a cut. 
        {
            canClimb = false;
            isJumping = true;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            airTime += Time.deltaTime;
            SetAnimationState(currentWeapon, "Player_Jump");
            
            if (airTime > 0.3f)
            {
                
                isJumping = false;
                jumpInput = false;
                gravity *= 2;
                airTime = 0.0f;
                
            }
        }
    }

    private void PerformDoubleJump()
    {
        if (doubleJumping)
 
        if (doubleJumping && isGrounded)
        {
            doubleJumping = false;
        } 
    }

    public void OnClimbInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Climbing");
            climbInput = true;
        }
        if (context.canceled)
        {
            climbInput = false;
            isClimbing = false;
            //stay on ladder logic 
            
        }
        
    }

    public void OnDescendInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            descendInput = true;
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundcheckDistance);
            if (hit.collider.TryGetComponent(out ClimbableRoof roof))
            {
                roof.CheckDescent(descendInput);
            }

        }
        if (context.canceled)
        {
            descendInput = false;
            isDescending = false;
            //stay on ladder logic 
            
        }
    }

    private void Climb()
    {
        //Debug.Log(canClimb);
        if(climbInput && canClimb)
        {
            isClimbing = true;
            SetAnimationState(currentWeapon, "Player_Climb");
            rb.velocity = new Vector2(0, climbSpeed);
        }


    }

    private void Descend()
    {
        if (descendInput && canDescend && !isGrounded)
        {
            isDescending = true;
            SetAnimationState(currentWeapon, "Player_Climb");
            rb.velocity = new Vector2(0, -descendSpeed);

        }
    }

    private void CheckClimbing()
    {
        if(canClimb && canDescend && !climbInput && !descendInput && !isGrounded && !isJumping)
        {
                //freeze player in place
                rb.velocity = new Vector2(0, 0);
                rb.gravityScale = 0;
                SetAnimationState(currentWeapon, "Player_Climb");
                //set animation speed to 0. 
                animator.speed = 0;
            
        }
        else
        {
            rb.gravityScale = 1;
            animator.speed = 1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IClimbable climbable))
        {
            gravity = 0;
            onClimbable = true;
            canClimb = true;
            canDescend = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IClimbable climbable))
        {
            gravity = internalGravity;
            onClimbable = false;
            canClimb = false;
            canDescend = false;
            
        }
    }

    private void CalculateGroundChecks()
    {

        Jump();
        Dash();
        CheckClimbing();
        Climb();
        Descend();
        if (isGrounded)
        {
            coyoteCounter = coyoteTime;
            airTime = 0.0f;
            isJumping = false;
            gravity = internalGravity; 


        }
        else if (!isGrounded)
        {
            coyoteCounter -= Time.deltaTime;
           
        }


    }
    public void CalculateMovementAnimationChecks()
    {
        Vector2 move = Move();
        boxCollider.isTrigger = false;

        if (move.x < 0 && isGrounded && !isDashing)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            SetAnimationState(currentWeapon, "Player_Run");
        }
        if (move.x < 0 && isGrounded && isDashing)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            SetAnimationState(currentWeapon, "Player_Roll");
        }
        if (move.x > 0 && isGrounded && !isDashing)
        {
            GetComponent<SpriteRenderer>().flipX = false;    ////move into overarching check function. also maybe stop the player from controlling movement when jumping ? 
            SetAnimationState(currentWeapon, "Player_Run");
        }
        if (move.x > 0 && isGrounded && isDashing)
        {
            GetComponent<SpriteRenderer>().flipX = false;    ////move into overarching check function. also maybe stop the player from controlling movement when jumping ? 
            SetAnimationState(currentWeapon, "Player_Roll");
        }
        if (move.x > 0 && !isGrounded && !onClimbable)
        { 
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (move.x < 0 && !isGrounded && !onClimbable)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (move.x == 0 && move.y == 0 && !onClimbable)
        {
            SetAnimationState(currentWeapon, "Player_Idle");
        }
        if(move.x > 0 && onClimbable && isGrounded)
        {
            SetAnimationState(currentWeapon, "Player_Run");
        }
        if(move.x == 0 && onClimbable && isGrounded)
        {
            SetAnimationState(currentWeapon, "Player_Idle");
        }
        if (!isGrounded && rb.velocity.y < 0 && !isAttacking && !onClimbable)
        {
            boxCollider.isTrigger = true;

            if (rb.velocity.x > 0)
            {
                boxCollider.isTrigger = false;
            }
            if (rb.velocity.x < 0)
            {
                boxCollider.isTrigger = false;
            }
            if(!isDashing || rb.velocity.x > 0)
            {
                SetAnimationState(currentWeapon, "Player_Fall");
            }
            
            
            //canClimb = false;


        }
        


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
        if (weaponMovementAnims.ContainsKey(currentWeapon) && weaponMovementAnims[currentWeapon].ContainsKey(State))
        {
            ChangeAnimationState(weaponMovementAnims[currentWeapon][State]);
        }
    }
    private void ChangeAnimationState(AnimationStates newState)
    {
        if (currentState == newState) return;
        string state = newState.ToString();
        animator.Play(state);
        currentState = newState;
    }



    //public void EndOfAttackAnim()
    //{
    //    isAttacking = false;
    //}

    private void ApplyGravity()
    {
        if (!isGrounded && !isClimbing && !isDescending)
        {
            rb.velocity += new Vector2(0, gravity * Time.deltaTime);
        }
    }

    public void Distort(float timeScale)
    {
        CustomTimeScale = timeScale; 
        //animator.speed = timeScale;
    }
    
    public void Distort(float timeScale, float duration)
    {
        Distort(timeScale);
        StartCoroutine(DistortionTime(duration));
    }

    public void Distort(float timeScale, float duration, float distortionRadius) //probably useless inside player class, much more useful in something like a grenade. 
    {
        Vector2 groundZero = new Vector2(transform.position.x, transform.position.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundZero, distortionRadius);
        foreach(Collider2D col in colliders)
        {
            if(col.TryGetComponent<IDistortable>(out IDistortable distortable))
            {
                distortable.Distort(timeScale, duration); 
                break;
            }
        }
    }

    private IEnumerator DistortionTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        CustomTimeScale = 1;
    }

}
