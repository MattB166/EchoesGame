using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Actions : MonoBehaviour
{
    public enum ActionAnims
    {
        Player_Bow_Attack,
        Player_Spear_Attack1,
        Player_Spear_Attack2,
        Player_Spear_Throw,
        Player_Sword_Attack1,
        Player_Sword_Attack2,
        Player_Sword_Attack3,
    }
    public enum Weapons
    {
        Sword,
        Spear,
        Bow,
        None
    }
    [Header("Weapon Settings")]
    public Weapons currentWeapon;
    public ActionAnims currentState;
    private Animator animator;
    private bool attackInput;
    private Dictionary<Weapons,Dictionary<string,ActionAnims>> attackAnims = new();

    private InputPickupItem closestInputPickupItem;
    // Start is called before the first frame update
    void Start()
    {
        attackInput = false;
        animator = GetComponent<Animator>();
         currentWeapon = Weapons.None;
        InitialiseAttackAnims();
    }

    // Update is called once per frame
    void Update()
    {
        BaseAttack();
        closestInputPickupItem =  UpdateClosestInputPickupItem();
        if(closestInputPickupItem != null)
        Debug.Log("Nearest pickup is" + closestInputPickupItem.itemData.name);
    }

    public void ChangeWeapon(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            switch(currentWeapon)
            {
                case Weapons.Sword:
                    currentWeapon = Weapons.Spear;
                    break;
                    case Weapons.Spear:
                    currentWeapon = Weapons.Bow;
                    break;
                    case Weapons.Bow:
                    currentWeapon = Weapons.None;
                    break;
                    case Weapons.None:
                    currentWeapon = Weapons.Sword;
                    break;
                    default:
                    currentWeapon = Weapons.None;
                    break;
            }
        }
    }

    private void InitialiseAttackAnims()
    {
        attackAnims.Add(Weapons.Bow, new Dictionary<string, ActionAnims>
        {
            {"Player_Bow_Attack",ActionAnims.Player_Bow_Attack},
        });

        attackAnims.Add(Weapons.Spear, new Dictionary<string, ActionAnims>
        {
            {"Player_Spear_Attack1",ActionAnims.Player_Spear_Attack1},
            {"Player_Spear_Attack2",ActionAnims.Player_Spear_Attack2},
            {"Player_Spear_Throw",ActionAnims.Player_Spear_Throw},
        });

        attackAnims.Add(Weapons.Sword, new Dictionary<string, ActionAnims>
        {
            {"Player_Sword_Attack1",ActionAnims.Player_Sword_Attack1},
            {"Player_Sword_Attack2",ActionAnims.Player_Sword_Attack2},
            {"Player_Sword_Attack3",ActionAnims.Player_Sword_Attack3},
        });
    }

    public void OnFireInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attackInput = true;
        }
        else if (context.canceled)
            attackInput = false;
    }

    private void SetAnimationState(Weapons currentWeapon, string State)
    {
        if (attackAnims.ContainsKey(currentWeapon) && attackAnims[currentWeapon].ContainsKey(State))
        {
            ChangeAnimationState(attackAnims[currentWeapon][State]);
        }
    }
    private void ChangeAnimationState(ActionAnims newState)
    {
        string state = newState.ToString();  ////cant do current check with this or it wont play it again 
        animator.Play(state);
        currentState = newState;

    }

    public void BaseAttack()
    {
        if (attackInput)
        {
            switch (currentWeapon)
            {
                case Weapons.Sword:
                    SetAnimationState(Weapons.Sword, "Player_Sword_Attack1");
                    Debug.Log("Sword Attack");
                    break;
                case Weapons.Spear:
                    SetAnimationState(Weapons.Spear, "Player_Spear_Attack1");
                    Debug.Log("Spear Attack");
                    break;
                case Weapons.Bow:
                    SetAnimationState(Weapons.Bow, "Player_Bow_Attack");
                    Debug.Log("Bow Attack");
                    break;
                default:
                    break;
            }
            
        }
       
        
    }

    public InputPickupItem UpdateClosestInputPickupItem()
    {
        float nearestDistance = float.MaxValue;
        InputPickupItem nearestItem = null;
        foreach (var item in InputPickupItem.itemsInRange)
        {
            float distance = Vector2.Distance(item.transform.position, transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestItem = item;
            }
        }
        return nearestItem;
    }

    public void OnPickupInput(InputAction.CallbackContext context)
    {
        if (context.performed && closestInputPickupItem != null)
        {
            switch(closestInputPickupItem.itemData.dataType)
            {
                case ItemData.DataType.Weapon:
                    Debug.Log("Picked up weapon");
                    break;
                case ItemData.DataType.Health:
                    //increase health
                    break;
                case ItemData.DataType.Coin:
                    Debug.Log("Picked up coin");
                    break;
                default:
                    break;
            }
            closestInputPickupItem.OnInteract();
        }
    }
}
