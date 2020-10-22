using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EndgameController : MonoBehaviour
{


    public List<GameObject> enemiesAlive;
    public int delayTime;
    public int timeBetweenExplosions;
    public List<GameObject> Explosions;

    private void Start()
    {
        foreach (GameObject g in Explosions)
        {
            g.SetActive(false);
        }
    }

    void Update()
    {
        if (enemiesAlive[0] == null && enemiesAlive[1] == null)
        {
            StartFinalScene();
        }
    }

    async Task StartFinalScene()
    {

        await Task.Delay(delayTime);
        foreach (GameObject g in Explosions)
        {
            g.SetActive(true);
        }

    }
}
