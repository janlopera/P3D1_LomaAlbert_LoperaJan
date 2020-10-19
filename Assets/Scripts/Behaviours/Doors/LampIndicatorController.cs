using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampIndicatorController : MonoBehaviour
{

    public GameObject OpenLight;
    public GameObject ClosedLight;

    private void Start()
    {
        setClosed();
    }


    public void setOpen()
    {
        OpenLight.SetActive(true);
        ClosedLight.SetActive(false);
    }

    public void setClosed()
    {
        OpenLight.SetActive(false);
        ClosedLight.SetActive(true);
    }
}
