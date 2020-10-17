using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField]
    public static int score;

    public static bool canScore = false;

    public static void scorePoints(int points)
    {
        if (canScore)
        {
            score += points;
        }
    }
    
}
