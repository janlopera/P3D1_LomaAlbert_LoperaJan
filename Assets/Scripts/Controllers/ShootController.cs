using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Exceptions;
using Models.Weapons;
using UnityEngine;

public class ShootController : MonoBehaviour //PROVISIONAL
{
    public Weapon Weapon;

    public GameObject gm;

    private Task shoot;
    private Task reload;
    
    async void Start()
    {
        Weapon.Reset(this);
        reload = Weapon.Reload(this);
        await reload;
    }

    public async Task Shoot()
    {
        if (shoot is null || (shoot.IsCompleted && reload.IsCompleted))
        {
            try
            {
                shoot = Weapon.Shoot(this, gm);
                await shoot;
            }
            catch (ShootFailedException e)
            {
                switch (e.ErrorType)
                {
                    case ShootFailedException.WeaponErrorType.NoAmmoInClip:
                        reload = Weapon.Reload(this);
                        await reload;
                        break;
                    case ShootFailedException.WeaponErrorType.NoAmmo:
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
           
        }
    }
    
 
}
