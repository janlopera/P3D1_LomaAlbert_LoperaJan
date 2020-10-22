using System.Collections;
using System.Collections.Generic;
using Behaviours;
using UnityEngine;

public class PlayerDamagedHUD : MonoBehaviour
{
    private Animation damageAnim;
    private HealthSystem playerHealthSystem;
    private float lastHealth; 
    void Start()
    {
        damageAnim = this.GetComponent<Animation>();
        playerHealthSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        lastHealth = playerHealthSystem.MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealthSystem.Health < lastHealth)//Damaged
        {
            damageAnim.Play("BloodScreen");
            lastHealth = playerHealthSystem.Health;
        }
        
    }
}
