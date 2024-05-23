using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalInputHandler : MonoBehaviour
{
    public InputAction pickupAction;
    private InputPickupItem pickupItemScript;
}
//    private void Awake()
//    {
//        pickupItemScript = FindFirstObjectByType<InputPickupItem>();


//        pickupAction.performed += ctx => pickupItemScript.OnPickupInput(ctx);
//    }
    
//    private void OnEnable()
//    {
//        pickupAction.Enable();
//    }

//    private void OnDisable()
//    {
//        pickupAction.Disable();
//    }

//    private void OnPickupInput(InputAction.CallbackContext context)
//    {
//        pickupItemScript.OnPickupInput(context);
//    }
//}
