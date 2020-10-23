using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FMODUnity;
using UnityEngine;

public class EndgameController : MonoBehaviour
{


    public List<GameObject> enemiesAlive;
    public int delayTime;
    public int timeBetweenExplosions;
    public List<GameObject> Explosions;

    private StudioEventEmitter _sound;
    private bool ended = false;

    private void Start()
    {
        foreach (GameObject g in Explosions)
        {
            g.SetActive(false);
        }

        _sound = GetComponent<StudioEventEmitter>();
    }

    async void Update()
    {
        if (enemiesAlive[0] == null && enemiesAlive[1] == null && ! ended)
        {
            await StartFinalScene();
        }
    }

    async Task StartFinalScene()
    {
        ended = true;
        if(!_sound.IsPlaying()) _sound.Play();
        await Task.Delay(delayTime);
        _sound.Event = "event:/Explosive/Explosion";
        _sound.Play();
        foreach (GameObject g in Explosions)
        {
            g.SetActive(true);
        }

    }
}
