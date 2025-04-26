using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    public float throwForce = 500f;
    public float pickUpRange = 5f;

    public GameObject heldObj;
    private Rigidbody heldObjRb;
    public bool canDrop = true;

    private Camera mainCamera;
    public TMP_Text theUseText;
    public CheckAllItems items;
    bool isAll;

    public RASNUSCUTSCENE rasnusCutScene;

    void Start()
    {
        // Cache the main camera for easier access
        mainCamera = Camera.main;
    }

    void Update()
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("Player"); // Exclude the "Player" layer
        theUseText.text = "";

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange, layerMask))
        {
            if (hit.transform.gameObject.CompareTag("canPickUp") && heldObj == null)
            {
                InteractableObject theObject = hit.transform.gameObject.GetComponent<InteractableObject>();
                if(!theObject.rb.isKinematic)
                {
                    theUseText.text = "Pick up " + theObject.nameForThis + " E";
                }
            }
        }

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange, layerMask))
        {
            if (hit.transform.gameObject.CompareTag("Leave") && heldObj == null)
            {
                if(items.allFurniture.Count == items.allItemsInScene.Count)
                {
                    theUseText.text = "You got everythig GO GO GO! E";
                    isAll = true;
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isAll)
            {
                Debug.Log("Cutscene");
                PointSystem system = FindObjectOfType<PointSystem>();
                system.REMEMBERTOCALLIFSCENESWITCH();
                PlayerPrefs.SetInt("NormalEnding", 1);
                rasnusCutScene.GoodEnding();
            }


            if (heldObj == null)
            {
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange, layerMask))
                {
                    if (hit.transform.gameObject.CompareTag("canPickUp"))
                    {
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop)
                {
                    DropObject();
                }
            }
        }

        if (heldObj != null)
        {
            MoveObject();

            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop)
            {
                ThrowObject();
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            AudioManager.Instance.PlaySFX("Pick");

            heldObj = pickUpObj;
            heldObjRb = pickUpObj.GetComponent<Rigidbody>();

            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform;
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }

    public void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObj = null;
    }

    void MoveObject()
    {
        heldObj.transform.position = holdPos.transform.position;
        heldObj.transform.rotation = holdPos.transform.rotation * heldObj.transform.localRotation;
    }

    public void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(mainCamera.transform.forward * throwForce);
        heldObj = null;
    }
}
