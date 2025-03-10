using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : NonInputPickup
{
    private Animator anim;
    public GameEvent keyCollected;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void HandlePickup(Actions player, Inventory i)
    {
        //either store the paired door in the item data, or have a game event. 
        //also quickly pan camera to the door to demonstrate what the key did. 
        keyCollected.Announce(this,null);

    }

    protected override void Collect()
    {
        anim.Play("KeyPickup");
    }
}
