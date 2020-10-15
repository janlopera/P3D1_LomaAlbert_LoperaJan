using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : ScriptableObject
{
    public float Price;
    public float Damage;
    public float RangeModifier;
    public float CycleTime;
    public float Penetration;
    public float KillAward;
    public float MaxPlayerSpeed;
    public int ClipSize;
    public float Range;
    public bool IsFullAuto;
    public int BulletsPerShot;
    public int ReserveAmo;
    
    public int ResetTime;
    public int ReloadTime;
}
