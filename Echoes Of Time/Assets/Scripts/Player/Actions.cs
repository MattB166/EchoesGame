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
    private HashSet<Weapons> availableWeapons = new();
    public HashSet<Weapons> GetAvailableWeapons()
    {
        return availableWeapons;
    }
    public void AddWeapon(Weapons weapon/*, int damage*/)
    {
        availableWeapons.Add(weapon);
        // weaponDamages[weapon] = damage;
    }

    private Inventory Inventory;
    private MeleeWeaponItem closeRangeItem; 
    // Start is called before the first frame update
    void Start()
    {
        Inventory = GetComponent<Inventory>();
       // attackInput = false;
        animator = GetComponent<Animator>();
        currentWeapon = Weapons.None;
        InitialiseAttackAnims();
        InitialisePlayer();
        if (Inventory != null)
        {
            Inventory.itemChangedCallback += ChangeWeaponAnimation;
            Inventory.itemUsedCallback += CheckForUseItem;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForUseItem();
        closestInputPickupItem = UpdateClosestInputPickupItem();
    }

    public void InitialisePlayer()
    {
        HitPoints = playerMaxHealth;
    }

    public void ChangeWeaponAnimation(InventoryItem item)
    {
        if (item.item.itemData as WeaponData != null)
        {
            Weapons weapon = (item.item.itemData as WeaponData).weaponType;
            if(!availableWeapons.Contains(weapon))
            {
                //Debug.Log("Adding weapon to available weapons animations");
                AddWeapon(weapon);
            }
           // Debug.Log("Already have this weapon anim. Changing weapon animation to " + weapon);
            currentWeapon = weapon;
            if(currentWeapon == Weapons.Sword || currentWeapon == Weapons.Spear)
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

    //private Weapons GetNextAvailableWeapon()
    //{
    //    List<Weapons> availableWeaponsList = new List<Weapons>(availableWeapons);
    //    if (availableWeaponsList.Count == 0)
    //    {
    //        return Weapons.None;
    //    }

    //    int currentIndex = availableWeaponsList.IndexOf(currentWeapon);
    //    int nextIndex = (currentIndex + 1) % availableWeaponsList.Count;
    //    return availableWeaponsList[nextIndex];
    //}

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

    public void CheckForUseItem(InventoryItem item) //change to "check for item use" and have polymorphism determine what each item does. 
    {

        //attackInput = false;
        //isAttacking = true;
        if (item.item.itemData as WeaponData != null)
        {
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
            HandlePickup(closestInputPickupItem);
            closestInputPickupItem.OnInteract();
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
