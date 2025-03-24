using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Switch : BaseNonPickup, IDistortable
{
    private Animator anim;
    private bool switchedOn;
    public GameEvent onSwitch;
    public GameEvent offSwitch;
    public bool timedSwitch;
    public float switchTime;

    public float CustomTimeScale { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
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
            if (timedSwitch)
            {
                StartCoroutine(SwitchOff());
            }
            onSwitch.Announce(this, null);
            //remove switch game event so it can't be triggered again.
            onSwitch = null;
        }
        else if (switchedOn)
        {
            anim.Play("SwitchOff");
            switchedOn = false;
            //onSwitch.Announce(this, null);
            //onSwitch = null;
        }
    }

    private IEnumerator SwitchOff()
    {
        yield return new WaitForSeconds(switchTime);
        InteractSwitch();

    }

    public void Distort(float timeScale)
    {
        throw new System.NotImplementedException();
    }
}
