using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Manages the behavior of the portal item. not collectable, and not interactable by input and cannot be picked up, so
/// worth having its own individual class. 
/// </summary>
public class Portal : MonoBehaviour
{
    ///needs its own data set - the portal it will teleport to, time its open for, 
    ///if applicable, and the time it will take to teleport <summary>
    /// </summary>

    public PortalData portalData;
    private GameObject linkedPortal;
    public bool openPortal;
    private bool isOpening;
    public bool closePortal;
    private bool isClosing;

    //when the player enters the portal, they will be teleported to the other portal, and if it is one way, both portals close and the player cannot return, portals destroyed. 
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(openPortal && !isOpening)
        {
            StartCoroutine(OpenPortal());
        }
        if (closePortal && !isClosing)
        {
            StartCoroutine(ClosePortal());
        }
    }

    private IEnumerator OpenPortal()
    {
        openPortal = false;
        isOpening = true;
        Vector3 startScale = gameObject.transform.localScale;
        Vector3 endScale = new Vector3(1.5f, 1.5f, 0);
        float elapsed = 0;
        while (elapsed < 0.5f)
        {
            float t = Mathf.Clamp01(elapsed / 0.5f);
            gameObject.transform.localScale = Vector3.Lerp(startScale,endScale, t);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180 * t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(1.5f, 1.5f, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 180);
        isOpening = false;
        //have the scale of the portal increase gradually. 
    }

    private IEnumerator ClosePortal()
    {
        closePortal = false;
        isClosing = true;
        Vector3 startScale = gameObject.transform.localScale;
        Vector3 endScale = new Vector3(0.0f, 0.0f, 0.0f);
        float elapsed = 0;
        while (elapsed < 0.5f)
        {
            float t = Mathf.Clamp01(elapsed / 0.5f);
            gameObject.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180 * t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        isClosing = false;
    }

    //make own animation for portal opening and closing, like spinning and glowing, expanding and contracting when teleporting items.
}
