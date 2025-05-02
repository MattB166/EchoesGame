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
    public bool shouldAllowReUse;
    private GameObject switchTimerImage;
    public AudioClip switchSound;

    private float customTimeScale;
    public float CustomTimeScale 
    {
        get => customTimeScale;
        set => customTimeScale = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        customTimeScale = 1;
        if(timedSwitch)
        {
            switchTimerImage = transform.GetChild(1).gameObject;
        }
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
                switchTimerImage.SetActive(true);
                StartCoroutine(SwitchOff());
            }
            onSwitch.Announce(this, null);
            //if cannot re use, remove switch game event so it can't be triggered again.
            if (!shouldAllowReUse)
            {
                onSwitch = null;
            }
            
        }
        else if (switchedOn)
        {
            anim.Play("SwitchOff");
            switchedOn = false;
            if (timedSwitch)
            {
                switchTimerImage.SetActive(false);
            }
            offSwitch.Announce(this, null);
            customTimeScale = 1;
            if (!shouldAllowReUse)
            {
                offSwitch = null;
            }
        }
        if (switchSound != null)
        {
            MusicManager.instance.PlaySFX(switchSound, transform.position);
        }
    }

    private IEnumerator SwitchOff()
    {
        float elapsed = 0f;
        while (elapsed < switchTime)
        {
            
            yield return null;
            if(customTimeScale > 0)
            {
                elapsed += Time.deltaTime * customTimeScale;
            }
        }
        elapsed = 0;
        InteractSwitch();

    }

    public void Distort(float timeScale)
    {
        customTimeScale = timeScale;
    }
}
