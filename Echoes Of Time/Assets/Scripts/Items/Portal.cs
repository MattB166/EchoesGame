using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Manages the behavior of the portal item. not collectable, and not interactable by input and cannot be picked up, so
/// worth having its own individual class. 
/// </summary>
public enum PortalNode
{
    Start,
    End
}
public class Portal : MonoBehaviour
{
    ///needs its own data set - the portal it will teleport to, time its open for, 
    ///if applicable, and the time it will take to teleport <summary>
    /// </summary>
    /// 
    

    public PortalData portalData;
    private GameObject linkedPortal;
    private Portal linkedPortalScript;
    public bool openPortal;
    private bool isOpening;
    public bool closePortal;
    private bool isClosing;
    [SerializeField] private PortalNode portalNode;
    private Vector3 lastPosition;
    private float stillTimer;
    public bool portalBeingPlaced;
    private float portalPlacementTimer;
    private SpriteRenderer spriteRenderer;
    public float proximityRadius;
    public Collider2D triggerCol;
    Color portalColour;
    public LayerMask obstacleLayer;

    public GameEvent exitedPortal;
    //when the player enters the portal, they will be teleported to the other portal, and if it is one way, both portals close and the player cannot return, portals destroyed. 
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCol = GetComponent<Collider2D>();
        if(portalNode == PortalNode.Start)
        {
            Color randomCol = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), 1f);
            portalColour = randomCol;
            spriteRenderer.color = portalColour;
        }
        else
        {
            spriteRenderer.color = linkedPortalScript.portalColour;
        }


    }


    public void InitialisePortal(PortalData data, PortalNode node)
    {
        portalData = data;
        portalNode = node;
        openPortal = true;
        if (portalNode == PortalNode.End)
        {
            portalBeingPlaced = true;
            portalPlacementTimer = portalData.portalPlacementTimer;
            
        }

    }

    public void SetLinkedPortal(GameObject portal)
    {
        linkedPortal = portal;
        linkedPortalScript = linkedPortal.TryGetComponent(out Portal portalScript) ? portalScript : null;
    }

    // Update is called once per frame
    void Update()
    {
        if (openPortal && !isOpening)
        {
            StartCoroutine(OpenPortal());
        }
        if (closePortal && !isClosing)
        {
            StartCoroutine(ClosePortal());
        }
        if (portalBeingPlaced && portalNode == PortalNode.End)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
            Vector3 startingPos = linkedPortal.transform.position;
            if(Vector3.Distance(transform.position, startingPos) > portalData.portalPlacementDistance)
            {
                stillTimer = 0;
                
                return;
            }

            if(!CanPlacePortal())
            {
                stillTimer = 0;
                
                return;
            }

            if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
            {
                stillTimer += Time.deltaTime;
                if (stillTimer >= portalData.portalPlacementTimer)
                {
                    spriteRenderer.enabled = true;
                    portalBeingPlaced = false;
                    openPortal = true;
                    stillTimer = 0;
                }
            }
            else
            {
                stillTimer = 0;
            }

            lastPosition = transform.position;
            
        }


        PerformEffects();

    }


    public bool CanPlacePortal()
    {
        ///checks whether any objects are in the way of the portal being placed. walls, enemies, etc.
        float radius = proximityRadius;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius,obstacleLayer);
        if (colliders.Length > 0)
        {
            
            for (int i = 0; i < colliders.Length; i++)
            {
                
            }
            return false;
        }
        return true;
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
            gameObject.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180 * t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(1.5f, 1.5f, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 180);
        isOpening = false;

        
    }

    private IEnumerator ClosePortal()
    {
        exitedPortal.Announce(this);
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



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !portalBeingPlaced && !linkedPortalScript.portalBeingPlaced) 
        {
            //perform checks for one way / two way portals.
            CalculateTeleportType(collision);

        }
    }

    private void CalculateTeleportType(Collider2D col)
    {
        if (portalNode == PortalNode.Start)
        {
            //check if the linked portal is a one way portal, if so, close both portals and destroy
            //if not, teleport the player to the linked portal, both portals remain open, but stop the player from zapping back and forth immediately.
            if(portalData.oneWay)
            {
                //teleport the player to the linked portal, close both portals and destroy.
                linkedPortalScript.triggerCol.enabled = false;
                col.gameObject.transform.position = linkedPortal.transform.position;
                linkedPortalScript.closePortal = true;
                closePortal = true;
                Invoke("DestroyPortal", 2.0f);
            }
            else
            {
                //leave both portals open, teleport the player to the linked portal
                linkedPortalScript.triggerCol.enabled = false;
                col.gameObject.transform.position = linkedPortal.transform.position;
                StartCoroutine(linkedPortalScript.ReEnableTrigger(2.0f));
            }

        }
        else if (portalNode == PortalNode.End)
        {
            if (portalData.oneWay)
            {
                linkedPortalScript.triggerCol.enabled = false;
                col.gameObject.transform.position = linkedPortal.transform.position;
                linkedPortalScript.closePortal = true;
                closePortal = true;
                Invoke("DestroyPortal", 2.0f);
            }
            else
            {
                linkedPortalScript.triggerCol.enabled = false;
                col.gameObject.transform.position = linkedPortal.transform.position;
                StartCoroutine(linkedPortalScript.ReEnableTrigger(2.0f));
            }

            if (portalData.closesAfterTwoWay)
            {
                linkedPortalScript.triggerCol.enabled = false;
                col.gameObject.transform.position = linkedPortal.transform.position;
                linkedPortalScript.closePortal = true;
                closePortal = true;
                Invoke("DestroyPortal", 2.0f);
            }
            else
            {
                linkedPortalScript.triggerCol.enabled = false;
                col.gameObject.transform.position = linkedPortal.transform.position;
                StartCoroutine(linkedPortalScript.ReEnableTrigger(2.0f));
            }

        }
       
        


    }

    public IEnumerator ReEnableTrigger(float delay)
    {
        yield return new WaitForSeconds(delay);
        triggerCol.enabled = true;
    }



    public void PerformEffects()
    {
       
    }


    public void DestroyPortal()
    {
        Destroy(gameObject);
    }
    
}
