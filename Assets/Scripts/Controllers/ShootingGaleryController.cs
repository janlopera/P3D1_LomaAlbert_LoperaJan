using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ShootingGaleryController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ScoreObject> ScoreObjects;
    public float miniGameMaxTime = 60;
    public float miniGameTime;
    public bool miniGameStarted = false;
    public KeyCode StartGameKey = KeyCode.G;
    public bool isPlayerInStartZone = false;
    public GameObject HUDInfoPanel;

    public GameObject HUD_GamePanel;
    public TMP_Text HUDTime;

    // Update is called once per frame
    void Update()
    {
        if (!miniGameStarted)//GAME IS NOT STARTED YET
        {
            HUD_GamePanel.SetActive(false);
            if (isPlayerInStartZone)
            {
                HUDInfoPanel.SetActive(true);
            }
            else
            {
                HUDInfoPanel.SetActive(false);
            }
            if (Input.GetKeyDown(StartGameKey) && isPlayerInStartZone)//THE GAME START
            {
                miniGameStarted = true;
                HUDInfoPanel.SetActive(false);
                HUD_GamePanel.SetActive(true);
                miniGameTime = miniGameMaxTime;
                StartTimer();
            }
        }
        else //IN GAME
        {
            
            
        }
    }

    private async Task StartTimer()
    {
        HUDTime.SetText("Time: "+ miniGameTime);
        if (miniGameTime <= -1)
        {
            miniGameStarted = false;
            return; 
        }
        else
        {
            await Task.Delay(1000);
            miniGameTime -= 1;
            StartTimer();
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInStartZone = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInStartZone = false;
        }
    }
}
