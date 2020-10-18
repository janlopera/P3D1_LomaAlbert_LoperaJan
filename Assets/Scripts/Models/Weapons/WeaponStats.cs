using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStats", menuName = "WeaponStats" )]
public class WeaponStats : ScriptableObject
{
    public float Price;
    public float Damage;
    public float RangeModifier;
    public int CycleTime;
    public float Penetration;
    public float KillAward;
    public float MaxPlayerSpeed;
    public int ClipSize;
    public float Range;
    public bool IsFullAuto;
    public int BulletsPerShot;
    public int ReserveAmo;
    
    public int ResetTime;
    public int ReloadTime;//milliseconds


    public string ReloadAnimation;
    public string ResetAnimation;
    public string ShootAnimation;
    public string MovingAnimation;
    public string StaticAnimation;
}
