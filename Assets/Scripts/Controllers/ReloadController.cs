using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Weapons;
using UnityEngine;
using Object = System.Object;

public class ReloadController : MonoBehaviour
{

    public KeyCode reloadKey = KeyCode.R;
    public Weapon actualWeapon;

    async void Update()
    {
        return;
        if (!Input.GetKeyDown(reloadKey)) return;
        try
        {
            if (!actualWeapon.isReloading)
            { 
                await actualWeapon.Reload(this);                   
            }

        }
        catch (Exception e)
        {
            //No Weapon on hand
            Debug.Log("No attached Weapon");
        }

    }

}
