using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HUDController : MonoBehaviour
{
    public static GameObject TopItem;
    public static GameObject BottomItem;
    public static GameObject LeftItem;
    public static GameObject RightItem;
    public GameObject Bubble;
    public GameObject Dance;
    public GameObject Bazooka;
    public GameObject Hammer;
    private Vector2 inputVector;
    private Vector2 Up = new Vector2(0, 1);
    private Vector2 Down = new Vector2(0, -1);
    private Vector2 Left = new Vector2(-1, 0);
    private Vector2 Right = new Vector2(1, 0);
    public PlayerCombat playerCombat;
    public MaterialManager materialManager;
    private GameObject MeleeWeapon;
    private GameObject Shield;

    private GameObject RangedWeapon;
    public Vector3 PickPosition;    // x = 0.003419     y = 0.000451    z = 0.000457
    public Vector3 PickRotation;    // x = 82.979       y = -8.463      z = 86.094

    public Vector3 bazookaPos;      // x= 0.147         y = 1.07500005  z = 0.26699999

    public Vector3 bazookaRot;
    public Transform Hand;

    public Transform WeaponPosition;
    public bool hasHammer = false;
    public bool hasShield = false;
    public bool hasBazooka = false;

    // Start is called before the first frame update
    void Start()
    {

        TopItem = GameObject.Find("TopItem");
        BottomItem = GameObject.Find("BottomItem");
        LeftItem = GameObject.Find("LeftItem");
        RightItem = GameObject.Find("RightItem");

        Bubble.SetActive(false);
        Dance.SetActive(false);
        Hammer.SetActive(false);
        Bazooka.SetActive(false);

    }

    private void FixedUpdate()
    {

    }

    public void OnInventory(InputAction.CallbackContext value)
    {
        inputVector = value.ReadValue<Vector2>();
        if (inputVector.Equals(Up)) //1
        {
            if (Bubble.activeSelf)
            {
                deactivateTopItem();
                playerCombat.hasShield = true;
                materialManager.ActivateShield();
            }
        }
        else if (inputVector.Equals(Down)) //2
        {

        }
        else if (inputVector.Equals(Left) && hasBazooka && value.started) //3
        {
            if (playerCombat.hasWeapon)
            {
                playerCombat.hasWeapon = false;
                MeleeWeapon.SetActive(false);
                activateRightItem();
            }
            if (Bazooka.activeSelf)
            {
                deactivateLeftItem();
                playerCombat.isShooting = true;
                RangedWeapon.SetActive(true);
                Debug.Log("Activating bazooka: " + RangedWeapon.activeSelf);
            }
            else if (!Bazooka.activeSelf)
            {
                activateLeftItem();
                playerCombat.isShooting = false;
                RangedWeapon.SetActive(false);
                Debug.Log("Deactivating bazooka: " + RangedWeapon.activeSelf);
            }
            
        }
        else if (inputVector.Equals(Right) && hasHammer && value.started) //4
        {
            if(playerCombat.isShooting){
                playerCombat.isShooting = false;
                RangedWeapon.SetActive(false);
                activateLeftItem();
            }
            if (Hammer.activeSelf)
            {
                deactivateRightItem();
                playerCombat.hasWeapon = true;
                MeleeWeapon.SetActive(true);
                Debug.Log("Activating hammer: " + MeleeWeapon.activeSelf);
            }
            else if (!Hammer.activeSelf)
            {
                activateRightItem();
                playerCombat.hasWeapon = false;
                MeleeWeapon.SetActive(false);
                Debug.Log("Deactivating hammer: " + MeleeWeapon.activeSelf);
            }
        }
    }

    public void pickUpMeleeWeapon(Collider collision)
    {
        hasHammer = true;
        MeleeWeapon = collision.gameObject;
        MeleeWeapon.tag = "Untagged";
        MeleeWeapon.transform.parent = Hand.transform;
        MeleeWeapon.transform.localPosition = PickPosition;
        MeleeWeapon.transform.localEulerAngles = PickRotation;
        MeleeWeapon.SetActive(false);
        Debug.Log("Picking object: " + MeleeWeapon.activeSelf);
        activateRightItem();
    }

    public void pickUpShield(Collider collision)
    {
        hasShield = true;
        Shield = collision.gameObject;
        Shield.tag = "Untagged";
        Shield.SetActive(false);
        activateTopItem();
    }

    public void pickUpRangedWeapon(Collider collision)
    {
        hasBazooka = true;
        RangedWeapon = collision.gameObject;
        RangedWeapon.tag = "Untagged";
        Collider coll = RangedWeapon.GetComponent<Collider>();
        coll.enabled = false;
        /*
            TODO positioning of the weapon
        */
        RangedWeapon.transform.parent = WeaponPosition.transform;
        RangedWeapon.transform.localPosition = bazookaPos;
        RangedWeapon.transform.localEulerAngles = bazookaRot;
        RangedWeapon.SetActive(false);
        Debug.Log("Picking Object: " + RangedWeapon.activeSelf);
        activateLeftItem();
    }

    public void activateTopItem()
    {

        TopItem.GetComponent<Image>().color = new Color(1f, 0.5607843f, 0.8747101f, 1f);
        Bubble.SetActive(true);

    }
    
    public void deactivateTopItem()
    {

        TopItem.GetComponent<Image>().color = new Color(1f, 0.5607843f, 0.8747101f, 0.3921569f);
        Bubble.SetActive(false); //oppure Bubble.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.3921569f); se vogliamo che la prima volta non ci sia e le altre sia trasparente

    }

    public void activateBottomItem()
    {

        BottomItem.GetComponent<Image>().color = new Color(1f, 0.5607843f, 0.8747101f, 1f);
        Dance.SetActive(true);

    }

    public void deactivateBottomItem()
    {

        BottomItem.GetComponent<Image>().color = new Color(1f, 0.5607843f, 0.8747101f, 0.3921569f);
        Dance.SetActive(false);

    }

    public void activateLeftItem()
    {

        LeftItem.GetComponent<Image>().color = new Color(1f, 0.5607843f, 0.8747101f, 1f);
        Bazooka.SetActive(true);

    }

    public void deactivateLeftItem()
    {

        LeftItem.GetComponent<Image>().color = new Color(1f, 0.5607843f, 0.8747101f, 0.3921569f);
        Bazooka.SetActive(false);

    }

    public void activateRightItem()
    {

        RightItem.GetComponent<Image>().color = new Color(1f, 0.5607843f, 0.8747101f, 1f);
        Hammer.SetActive(true);

    }

    public void deactivateRightItem()
    {

        RightItem.GetComponent<Image>().color = new Color(1f, 0.5607843f, 0.8747101f, 0.3921569f);
        Hammer.SetActive(false); 

    }

}
