using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// This script deals with weapon animations and item pickup. inventory manages item storage and usage. 
/// </summary>
public class Actions : MonoBehaviour, IDamageable
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

    [Header("Player Stats")]
    [SerializeField]
    private float playerMaxHealth;               ///MOVE PLAYER HEALTH TO A SEPARATE SCRIPT FOR PLAYER STATS TO SEPARATE CONCERNS 
    private bool canTakeDamage = true;
    public float HitPoints
    {
        get { return playerMaxHealth; }
        set { playerMaxHealth = value; }
    }


    [Header("Weapon Settings")]
    public Weapons currentWeapon;
    public ActionAnims currentState;
    private Animator animator;
    //private bool attackInput;
    public bool isAttacking;
    private Dictionary<Weapons, Dictionary<string, ActionAnims>> attackAnims = new();
    private Dictionary<Weapons, float> weaponDamages = new();

    private InputPickupItem closestInputPickupItem;
    private BaseNonPickup closestNonPickupInteractable;
    private HashSet<Weapons> availableWeapons = new();
    public HashSet<Weapons> GetAvailableWeapons()
    {
        return availableWeapons;
    }
    public void SetAvailableWeapons(HashSet<Weapons> weapons)
    {
        availableWeapons = weapons;
    }
    public void SetCurrentWeapon(Weapons weapon)
    {
        currentWeapon = weapon;
    }
    public void AddWeapon(Weapons weapon)
    {
        availableWeapons.Add(weapon);
        
    }

    private MeleeWeaponItem closeRangeItem; 
    public delegate void AttackAnimFinished(Weapons weapon);
    public AttackAnimFinished attackAnimFinishedCallback;
    [Header("Events")]
    
    public GameEvent onHealthChange;


    // Start is called before the first frame update
    void Start()
    {
       
       // attackInput = false;
        animator = GetComponent<Animator>();
        currentWeapon = Weapons.None; //set this to be set to whatever has been saved. 
        InitialiseAttackAnims();
        InitialisePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForUseItem();
        closestInputPickupItem = UpdateClosestInputPickupItem();
        closestNonPickupInteractable = UpdateClosestNonPickupInteractable();
    }

    public void InitialisePlayer()
    {
        HitPoints = playerMaxHealth;
    }

    public void ChangeWeaponAnimation(Component sender, object data)
    {
        if(data is object[] dataArray && dataArray.Length > 0)
        {
            data = dataArray[0];
        }
        if (data is InventoryItem item)
        {
            Debug.Log("Changing weapon animation");
            if (item.item.itemData as WeaponData != null)
            {
                Weapons weapon = (item.item.itemData as WeaponData).weaponType;
                if (!availableWeapons.Contains(weapon))
                {

                    AddWeapon(weapon);
                    Debug.Log("Adding weapon to available weapons animations");
                }

                currentWeapon = weapon;
                if (currentWeapon == Weapons.Sword || currentWeapon == Weapons.Spear)
                {
                    closeRangeItem = item.item as MeleeWeaponItem;
                }
                else
                {
                    closeRangeItem = null;
                }



            }
            else
            {
                currentWeapon = Weapons.None;
                //Debug.Log("This is not a weapon so no animation required"); 
            }
        }
       
    }

    public void RemoveWeaponAnim(Component sender, object data)
    {
        if (data is object[] dataArray && dataArray.Length > 0)
        {
            data = dataArray[0];
        }
        if (data is InventoryItem item)
        {
            if (item.item.itemData as WeaponData != null)
            {
                Weapons weapon = (item.item.itemData as WeaponData).weaponType;
                if (availableWeapons.Contains(weapon))
                {
                    //Debug.Log("Removing weapon from available weapons animations");
                    availableWeapons.Remove(weapon);
                    if (availableWeapons.Count < 1)
                    {
                        currentWeapon = Weapons.None;
                    }

                }
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

    //public void OnUseInput(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        attackInput = true;
    //    }
    //    else if (context.canceled)
    //        attackInput = false;
    //}

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

    public void CheckForUseItem(Component sender, object data) //change to "check for item use" and have polymorphism determine what each item does. 
    {
        if (data is object[] dataArray && dataArray.Length > 0)
        {
            data = dataArray[0];
        }
        //attackInput = false;
        if (data is InventoryItem item)
        {
            if (item.item.itemData as WeaponData != null)
            {
                isAttacking = true;
                WeaponData weaponData = item.item.itemData as WeaponData;
                Weapons weapon = weaponData.weaponType;
                switch (weapon)
                {
                    case Weapons.Bow:
                        SetAnimationState(Weapons.Bow, "Player_Bow_Attack");
                        break;
                    case Weapons.Spear:
                        SetAnimationState(Weapons.Spear, "Player_Spear_Attack1");
                        break;
                    case Weapons.Sword:
                        SetAnimationState(Weapons.Sword, "Player_Sword_Attack1");
                        break;
                }
            }
        }
        

    }

    public void CheckForSecondaryItemUse(Component sender, object data)
    {
        if (data is object[] dataArray && dataArray.Length > 0)
        {
            data = dataArray[0];
        }
        if (data is InventoryItem item)
        {
            if (item.item.itemData as WeaponData != null)
            {
                isAttacking = true;
                WeaponData weaponData = item.item.itemData as WeaponData;
                Weapons weapon = weaponData.weaponType;
                switch (weapon)
                {
                    case Weapons.Spear:
                        SetAnimationState(Weapons.Spear, "Player_Spear_Throw");
                        break;
                }
            }
        }
         
    }

    public void EndOfAttackAnim()
    {
        isAttacking = false;
        attackAnimFinishedCallback?.Invoke(currentWeapon);
    }


    public InputPickupItem UpdateClosestInputPickupItem()
    {
        float nearestDistance = float.MaxValue;
        InputPickupItem nearestItem = null;
        foreach (var item in InputPickupItem.itemsInRange)
        {
            if(item != null)
            {
                float distance = Vector2.Distance(item.transform.position, transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestItem = item;
                }
            }
           
        }
        return nearestItem;
    }

    public BaseNonPickup UpdateClosestNonPickupInteractable()
    {
        float nearestDistance = float.MaxValue;
        BaseNonPickup nearestItem = null;
        foreach (var item in BaseNonPickup.itemsInRange)
        {
            if (item != null)
            {
                float distance = Vector2.Distance(item.transform.position, transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestItem = item;
                }
            }

        }
        return nearestItem;
    }

    public void OnPickupInput(InputAction.CallbackContext context)
    {
        if (context.performed && closestInputPickupItem != null)
        {
            Debug.Log("Interacting with closest item");
            HandlePickup(closestInputPickupItem);
            closestInputPickupItem.OnInteract();
        }
        else if (context.performed && closestNonPickupInteractable != null)
        {
            Debug.Log("Interacting with closest non pickup item");
            closestNonPickupInteractable.OnInteract();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NonInputPickup item))
        {
            HandlePickup(item);
        }
    }

    public void HandlePickup(BasePickupItem item)
    {
        Inventory inventory = TryGetComponent(out Inventory inventoryComponent) ? inventoryComponent : null;
        if (inventory != null)
        {
            item.HandlePickup(this, inventory);
        }
        else
        {
            Debug.Log("No Inventory Found");
        }
        //itemData?.HandlePickup(this);
    }



    ///referenced by animation to get correct time to check for sword contact, but passed out to items to control the damage dealt. 
    public void CheckWeaponContact()
    {
       if(closeRangeItem != null)
        {
            closeRangeItem.CheckWeaponDamage();
        }
    }

    public void RangedProjectileTime()
    {

    }

    public void TakeDamage(float amount)
    {
        if (canTakeDamage)
        {
            HitPoints -= amount;
            onHealthChange.Announce(this,HitPoints);
            if (HitPoints <= 0)
            {
                //death logic 
            }
        }

    }

    public void AddHealth(float amount)
    {
        HitPoints += amount;
        //Debug.Log("Added " + amount + " health");
    }
}
