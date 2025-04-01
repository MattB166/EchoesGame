using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
    public static Movement instance;

    [HideInInspector] public Vector2 input;
    public Vector2 spawnPos;
    public GameEvent OnPlayerDirectionFacing;
    public GameEvent moveInput;
    public GameEvent jumpInputEvent;
    public GameEvent doubleJumpInputEvent;
    public GameEvent dashInputEvent;
    public GameEvent climbInputEvent;

    [Header("Movement Settings")]
    [Range(1, 10)] public float walkSpeed;
    [Range(1, 10)] public float sprintSpeed;
    [Range(1, 10)] public float dashSpeed;
    [Range(0.1f,0.5f)] public float dashDuration;
    [Range(0.0f, 5.0f)] public float dashCooldown;
    [Range(1, 15)] public float jumpForce;
    [Range(1, 15)] public float doubleJumpForce;
    [Range(1, 2)] public float fallMultiplier;
    private Vector2 initialPos;

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
    public bool isOnLedge;
    [Space(10)]
    [Header("Jump Settings")]
    [Range(-20f, -0.05f)] public float gravity;
    private float internalGravity;
    [Range(0.01f, 0.5f)] public float groundcheckDistance;
    [Range(0.01f, 1.5f)] public float ledgeCheckDistance;
    public GameEvent OnPlayerLedging;
    public GameEvent OnEndOfLedging;
    private bool ledgeEventSent = false;
    private bool endOfLedgeEventSent = false;
    public float coyoteTime;
    private float coyoteCounter;
    //add jump buffer next 
    public LayerMask groundLayer;
    private Transform groundCheck;
    private Transform LeftLedgeCheck;
    private Transform RightLedgeCheck;

    [SerializeField]
    private float customTimeScale;
    public float CustomTimeScale
    {
        get { return customTimeScale; }
        set { customTimeScale = value; }

            }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        groundCheck = transform.Find("groundcheck");
        LeftLedgeCheck = transform.Find("leftledgecheck");
        RightLedgeCheck = transform.Find("rightledgecheck");
        animator = GetComponent<Animator>();
        currentWeapon = GetComponent<Actions>().currentWeapon;
        InitialiseWeaponAnims();
        isAttacking = GetComponent<Actions>().isAttacking;
        initialZRotation = transform.rotation.z;
        internalGravity = gravity;
        GetComponent<Actions>().attackAnimFinishedCallback += ResetAnimations;
        //startPos = CheckPointSystem.instance.activeCheckPoint.transform.position;
        initialPos = transform.position;
        ResetPlayerPosition();

        //start pos needs to be latest checkpoint of latest level. 
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
        if (transform.position.y < -30)
        {
            ResetPlayerPosition(); //send event to reset other objects in the scene that need resetting. 
        }


    }

    private void ResetAnimations(Actions.Weapons weapon) //re determines what animation should be playing after attacking
    {
        SetAnimationState(currentWeapon, "Player_Idle");
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        moveInput.Announce(this, input);
    }

    private Vector2 Move()
    {
        Vector2 movement;
        float oldDirection = direction;
        direction = GetComponent<SpriteRenderer>().flipX ? -1 : 1;
        if (oldDirection != direction)
        {
            OnPlayerDirectionFacing.Announce(this, direction); 
            //Debug.Log("Direction change event sent");
        }
        if (input.x == 0 && isDashing)
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
        if (context.performed)
        {
            jumpInputEvent.Announce(this, null);
            if (isGrounded || coyoteCounter > 0)
            {
                jumpInput = true;
            }
            else if (!isGrounded && !doubleJumping) 
            {
                doubleJumping = true;
            }
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
           dashInputEvent.Announce(this, null);
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
        if(jumpInput) //need a smoother double jump value i think.  
        {
            
            canClimb = false;
            isJumping = true;
            if (!doubleJumping && isGrounded)
            {
                jumpInputEvent.Announce(this, null);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                SetAnimationState(currentWeapon, "Player_Jump");
            }
            else if (!doubleJumping && !isGrounded)
            {
                doubleJumpInputEvent.Announce(this, null);
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                SetAnimationState(currentWeapon, "Player_Jump");
                doubleJumping = false;

            }
            jumpInput = false;

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
            climbInputEvent.Announce(this, null);
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
            bool leftLedgeCheck = Physics2D.Raycast(LeftLedgeCheck.position, Vector2.down, ledgeCheckDistance, groundLayer);
            bool rightLedgeCheck = Physics2D.Raycast(RightLedgeCheck.position, Vector2.down, ledgeCheckDistance, groundLayer);
            isOnLedge = !leftLedgeCheck || !rightLedgeCheck;
            if (isOnLedge && !ledgeEventSent)
            {
                //pump one event to camera to move to ledge position.
                //Debug.Log("Ledge event sent");
                OnPlayerLedging.Announce(this, null);
                ledgeEventSent = true;
                endOfLedgeEventSent = false;
            }
            else if (!isOnLedge && !endOfLedgeEventSent)
            {
                //reset ledge event sent and ask for normal camera follow.
                OnEndOfLedging.Announce(this, null);
                //Debug.Log("End of ledge event sent");
                ledgeEventSent = false;
                endOfLedgeEventSent = true;
            }

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
            if (rb.velocity.y > 0) 
            {
                rb.velocity += new Vector2(0, gravity * Time.deltaTime);
            }
            else 
            {
                rb.velocity += new Vector2(0, gravity * fallMultiplier * Time.deltaTime);
            }
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

    public void ResetPlayerPosition()
    {
        if(CheckPointSystem.instance.activeCheckPoint != null)
        {
            spawnPos = CheckPointSystem.instance.activeCheckPoint.transform.position;
            gameObject.transform.position = spawnPos;
        }
        else
        {
            gameObject.transform.position = initialPos;
        }


    }

    public void TemporarilyDisableMovement(Component sender, object data)
    {
        if(data is object[] dataArray && dataArray.Length > 0)
        {
            data = dataArray[0];
        }
        float duration = (float)data;
        StartCoroutine(DisableMovement(duration + 1));

    }

    private IEnumerator DisableMovement(float duration)
    {
        customTimeScale = 0;
        yield return new WaitForSeconds(duration);
        customTimeScale = 1;
    }


    public void ToggleGravity(Component sender, object data)
    {
        //toggle between 0 and internal gravity.
        if (gravity == 0)
        {
            //Debug.Log("Gravity on");
            gravity = internalGravity;
        }
        else
        {
            //Debug.Log("Gravity off");
            gravity = 0;
        }
    }

}
