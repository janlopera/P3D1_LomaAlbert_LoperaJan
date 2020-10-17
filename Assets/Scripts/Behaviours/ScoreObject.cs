using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObject : MonoBehaviour
{
    public int scoreGain;

    public bool destructOnShoot = false;
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
            Destroy(this.gameObject);
        }
    }
    
}
