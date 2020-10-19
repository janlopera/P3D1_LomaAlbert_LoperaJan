using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using Controllers;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour, IController
{

    public Transform centerView;
    public float MaxDistanceDetectionItems = 10;
    public LayerMask itemsLayer;
    public KeyCode pickItemKey = KeyCode.E;
    public GameObject HUDInfoPanel;

    public ShootingController _controller;
    public HealthSystem _HealthSystem;

    void Update()
    {
        Vector3 forward = centerView.TransformDirection(Vector3.forward) * MaxDistanceDetectionItems;
        Debug.DrawRay(centerView.position, forward, Color.green);

        RaycastHit hit;
        if (Input.GetKeyDown(pickItemKey))
        {

            if (Physics.Raycast(centerView.position, centerView.TransformDirection(Vector3.forward), out hit,
                MaxDistanceDetectionItems, itemsLayer))
            {

                if (proccesInteraction(hit.transform.tag, hit.transform.gameObject))
                {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(centerView.position, centerView.TransformDirection(Vector3.forward), out hit,
            MaxDistanceDetectionItems, itemsLayer))
        {
            string tag = hit.transform.gameObject.tag;
            if (tag != null)
            {
                if (tag == "Health" || tag == "Ammo" || tag == "Shield")
                {
                    HUDInfoPanel.SetActive(true);
                    HUDInfoPanel.GetComponent<TMP_Text>().SetText("Press [E] to pick "+tag);
                }else if (tag == "Key")
                {
                    HUDInfoPanel.SetActive(true);
                    HUDInfoPanel.GetComponent<TMP_Text>().SetText("Press [E] to activate key");
                }
            }
        }
        else
        {
            HUDInfoPanel.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (proccesInteraction(other.gameObject.tag, null))
        {
            if (other.gameObject.tag != "Key")
            {
                Destroy(other.gameObject);
            }

        }
        
    }

    private bool proccesInteraction(string tag, GameObject obj)
    {
        switch (tag)
        {
            case "Health": 
                incrementHealth();
                return true;
                break;
            case "Shield":
                incrementShield();
                return true;
                break;
            case "Ammo":
                incrementAmmo();
                return true;
                break;
            case "Key":
                unlockKey(obj);
                return true;
            default:
                return false;
                break;
        }
    }


    private void incrementHealth()
    {
        _HealthSystem.RefillHealth(40);
    }

    private void incrementShield()
    {
        _HealthSystem.RefillArmor(40);
    }

    private void incrementAmmo()
    {
        _controller.Refill(30);
    }

    private void unlockKey(GameObject key)
    {
        if (key != null)
        {
            key.GetComponent<KeyDoorController>().isUnlock = true;
        }
    }

    public void Constructor(object state, object sender)
    {
        
    }
}
