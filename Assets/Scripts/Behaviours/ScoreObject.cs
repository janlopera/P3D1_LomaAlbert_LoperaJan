using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObject : MonoBehaviour
{
    public int scoreGain;

    public bool destructOnShoot = false;

    public ParticleSystem destroyParticles;

    public bool prueba = false;
/*DEPRECATED
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Bullet"))
        {
            //addScore to player
            PlayerInfo.score += scoreGain;
            Debug.Log(PlayerInfo.score);
            //destroy the other object
            Destroy(other.gameObject);
        }
    }
*/

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
