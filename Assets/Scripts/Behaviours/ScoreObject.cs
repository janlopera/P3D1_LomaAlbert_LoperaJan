using System;
using System.Collections;
using System.Collections.Generic;
using Behaviours;
using FMODUnity;
using JetBrains.Annotations;
using UnityEngine;


[RequireComponent(typeof(HealthSystem))]
public class ScoreObject : MonoBehaviour
{
    public int scoreGain;

    public bool destructOnShoot = false;

    public string Sound;

    public ParticleSystem destroyParticles;
    
    private StudioEventEmitter _sound;

    public void Start()
    {
        _sound = GetComponent<StudioEventEmitter>();
    }

    public void getPoints()
    {
        PlayerInfo.scorePoints(scoreGain);
        if (destructOnShoot)
        {
            if (destroyParticles != null)
            {
                Instantiate(destroyParticles, transform.position, transform.rotation);
                
            }

            if (Sound != "")
            {
                _sound.Stop();
                _sound.Event = Sound;
                _sound.Play();
            }
            
            Destroy(this.gameObject);
        }
    }
    
}
