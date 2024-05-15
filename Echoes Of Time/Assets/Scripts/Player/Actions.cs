using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Actions : MonoBehaviour
{
    public enum Weapons
    {
        Sword,
        Spear,
        Bow,
        None
    }

    public Weapons currentWeapon;
    // Start is called before the first frame update
    void Start()
    {
      currentWeapon = Weapons.None;
    }

    // Update is called once per frame
    void Update()
    {
      
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
}
