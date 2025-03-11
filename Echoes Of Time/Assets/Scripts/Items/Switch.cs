using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : BaseNonPickup
{
    private Animator anim;
    private bool switchedOn;
    public GameEvent onSwitch;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnInteract()
    {
        InteractSwitch();
    }

    public void InteractSwitch()
    {
        if (!switchedOn)
        {
            anim.Play("Switch");
            switchedOn = true;
            onSwitch.Announce(this, null);
        }
        else if (switchedOn)
        {
            anim.Play("SwitchOff");
            switchedOn = false;
            onSwitch.Announce(this, null);
        }
    }
}
