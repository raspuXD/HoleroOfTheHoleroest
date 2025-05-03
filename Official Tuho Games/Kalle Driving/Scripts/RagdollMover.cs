using UnityEngine;

public class RagdollMover : MonoBehaviour
{
    Camera cam;
    Rigidbody grabbedRigidbody;
    Vector3 grabOffset;
    float grabDistance;

    public bool isHolding = false;

    public Crosshair theCrossHair;

    public LayerMask theIngonre;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        CheckCrosshair();

        if(!isHolding)
        {
            CheckText();
        }

        if (Input.GetMouseButtonDown(1))
        {
            TryGrabRigidbody();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ReleaseRigidbody();
        }
    }

    void FixedUpdate()
    {
        if (grabbedRigidbody != null)
        {
            MoveRigidbody();
        }
    }

    void CheckText()
    {
        theCrossHair.thePetteriPickUP.SetActive(false);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, ~theIngonre))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("PetteriRagdoll") || hitObject.layer == LayerMask.NameToLayer("PetteriRagdoll"))
            {
                theCrossHair.thePetteriPickUP.SetActive(true);
                theCrossHair.UpdateTheCrosshair(1, 0);
            }
        }
    }

    void CheckCrosshair()
    {
        if(isHolding)
        {
            theCrossHair.UpdateTheCrosshair(1, 1);
        }
        else
        {
            theCrossHair.UpdateTheCrosshair(0, 0);
        }
    }

    void TryGrabRigidbody()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, ~theIngonre))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("PetteriRagdoll") || hitObject.layer == LayerMask.NameToLayer("PetteriRagdoll"))
            {
                grabbedRigidbody = hitObject.GetComponent<Rigidbody>();
                if (grabbedRigidbody != null)
                {
                    GameObject theRagdollRoot = grabbedRigidbody.gameObject;
                    GameObject theRoot = theRagdollRoot.transform.root.gameObject; // Corrected here
                    DestroyAfter destroy = theRoot.GetComponent<DestroyAfter>();
                    if (destroy != null)
                    {
                        destroy.RefuseForNow(30f);
                    }

                    grabOffset = hit.point - grabbedRigidbody.position;
                    grabDistance = hit.distance;
                    grabbedRigidbody.useGravity = false;
                }
            }
        }
    }

    void MoveRigidbody()
    {
        theCrossHair.thePetteriPickUP.SetActive(false);
        isHolding = true;
        Vector3 targetPosition = cam.transform.position + cam.transform.forward * grabDistance - grabOffset;
        Vector3 forceDirection = (targetPosition - grabbedRigidbody.position) * 10f;
        grabbedRigidbody.velocity = forceDirection;
    }

    void ReleaseRigidbody()
    {
        if (grabbedRigidbody != null)
        {
            theCrossHair.thePetteriPickUP.SetActive(false);
            isHolding = false;
            grabbedRigidbody.useGravity = true;
            grabbedRigidbody = null;
        }
    }
}