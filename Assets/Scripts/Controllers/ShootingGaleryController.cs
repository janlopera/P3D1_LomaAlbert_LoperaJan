using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class ShootingGaleryController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<SimpleMovement> movingObjects; 
    public float miniGameMaxTime = 60;
    public float miniGameTime;
    public bool miniGameStarted = false;
    public KeyCode StartGameKey = KeyCode.G;
    public bool isPlayerInStartZone = false;
    public GameObject HUDInfoPanel;
    public int actualScore;
    public int lastScore = 0;

    public GameObject HUD_GamePanel;
    public TMP_Text HUDTime;
    public TMP_Text HUDScore;

    public GameObject chickensToSpawn;
    public GameObject chickensInstance;

    public Transform chickenSpawnPoint;

    public float minScoreToOpenDoor = 1000;
    public DoorController containerDoor;
    // Update is called once per frame
    void Update()
    {
        if (!miniGameStarted)//GAME IS NOT STARTED YET
        {
            
            if (isPlayerInStartZone)
            {
                HUDInfoPanel.SetActive(true);
                HUD_GamePanel.SetActive(true);
                HUDTime.SetText("Time: "+miniGameMaxTime);
                if (lastScore == 0)
                {
                    HUDScore.SetText("Score: "+lastScore);
                }
            }
            else
            {
                HUDInfoPanel.SetActive(false);
                HUD_GamePanel.SetActive(false);
            }
            if (Input.GetKeyDown(StartGameKey) && isPlayerInStartZone)//THE GAME START
            {
                PlayerInfo.score = 0;
                PlayerInfo.canScore = true;
                miniGameStarted = true;
                HUDInfoPanel.SetActive(false);
                HUD_GamePanel.SetActive(true);
                miniGameTime = miniGameMaxTime;
                enableMovement();
                spawnChickens();

                StartTimer();
            }
        }
        else //IN GAME
        {
            lastScore = PlayerInfo.score;
            HUDScore.SetText("Score: "+PlayerInfo.score);

        }
    }

    private async Task StartTimer()
    {
        HUDTime.SetText("Time: "+ miniGameTime);
        if (miniGameTime <= -1)//END OF THE MINIGAME
        {
            miniGameStarted = false;
            PlayerInfo.canScore = false;
            deleteChickens();
            openDoorOnFinish();
            disableMovement();
            
            return; 
        }
        else
        {
            await Task.Delay(1000);
            miniGameTime -= 1;
            StartTimer();
        }
    }

    private void enableMovement()
    {
        foreach (SimpleMovement obj  in movingObjects)
        {
            obj.isMoving = true;
        }
    }
    
    private void disableMovement()
    {
        foreach (SimpleMovement obj  in movingObjects)
        {
            obj.isMoving = false;
        }
    }

    private void spawnChickens()
    {
        chickensInstance = Instantiate(chickensToSpawn, chickenSpawnPoint.position, Quaternion.identity);
    }

    private void deleteChickens()
    {
        Destroy(chickensInstance);
    }

    private void openDoorOnFinish()
    {
        if (lastScore >= minScoreToOpenDoor)
        {
            if (!containerDoor.isOpen)
            {
                containerDoor.OpenDoor();
            }

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
