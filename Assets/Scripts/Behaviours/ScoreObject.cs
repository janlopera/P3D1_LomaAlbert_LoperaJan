using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using UnityEngine;


[RequireComponent(typeof(HealthSystem))]
public class ScoreObject : MonoBehaviour
{
    public int scoreGain;

    public bool destructOnShoot = false;

    public ParticleSystem destroyParticles;

    public void getPoints()
    {
        PlayerInfo.scorePoints(scoreGain);
        if (destructOnShoot)
        {
            if (destroyParticles != null)
            {
                Instantiate(destroyParticles, transform.position, transform.rotation);
                
            }
            Destroy(this.gameObject);
        }
    }
    
}
