using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private float playerMaxHealth;
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
    private bool attackInput;
    public bool isAttacking;
    private Dictionary<Weapons, Dictionary<string, ActionAnims>> attackAnims = new();
    private Dictionary<Weapons, float> weaponDamages = new();

    private InputPickupItem closestInputPickupItem;
    private HashSet<Weapons> availableWeapons = new();
    public HashSet<Weapons> GetAvailableWeapons()
    {
        return availableWeapons;
    }
    public void AddWeapon(Weapons weapon, int damage)
    {
        availableWeapons.Add(weapon);
        weaponDamages[weapon] = damage;
    }



    // Start is called before the first frame update
    void Start()
    {
        attackInput = false;
        animator = GetComponent<Animator>();
        currentWeapon = Weapons.None;
        InitialiseAttackAnims();
        InitialisePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        BaseAttack();
        closestInputPickupItem = UpdateClosestInputPickupItem();
    }

    public void InitialisePlayer()
    {
        HitPoints = playerMaxHealth;
    }

    public void ChangeWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Weapons nextWeapon = GetNextAvailableWeapon();
            if (nextWeapon != Weapons.None)
            {
                currentWeapon = nextWeapon;
                Debug.Log("Current weapon is now " + currentWeapon);
            }
        }
    }

    private Weapons GetNextAvailableWeapon()
    {
        List<Weapons> availableWeaponsList = new List<Weapons>(availableWeapons);
        if (availableWeaponsList.Count == 0)
        {
            return Weapons.None;
        }

        int currentIndex = availableWeaponsList.IndexOf(currentWeapon);
        int nextIndex = (currentIndex + 1) % availableWeaponsList.Count;
        return availableWeaponsList[nextIndex];
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
            attackInput = false;
            isAttacking = true;
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
                    SetAnimationState(Weapons.None, "Player_Idle");
                    break;
            }
            isAttacking = false;
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
            HandlePickup(closestInputPickupItem.itemData);
            closestInputPickupItem.OnInteract();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out NonInputPickup item))
        {
            HandlePickup(item.itemData);
        }
    }

    public void HandlePickup(ItemData itemData)
    {
        itemData?.HandlePickup(this);
    }

    public void AddHealth(float amount)
    {

    }


    ///referenced by animation to get correct time to check for sword contact
    public void CheckWeaponContact()
    {
        if (weaponDamages.ContainsKey(currentWeapon))
        {
            float damage = weaponDamages[currentWeapon];

            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.6f);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                    Debug.Log("Dealt " + damage + " damage to " + hit.name);
                }
            }

        }

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
}
