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

    void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            try
            {
                if (!actualWeapon.isReloading)
                { 
                    Reload();                    
                }

            }
            catch (Exception e)
            {
                //No Weapon on hand
                Debug.Log("No attached Weapon");
            }
        }
        
    }
    
    public async Task Reload()
    {
        actualWeapon.isReloading = true;
        await Task.Delay(actualWeapon.WeaponStats.ReloadTime);
        actualWeapon.isReloading = false;
    }
}
