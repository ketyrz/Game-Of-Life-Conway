using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isAlive;
    public bool nextStatus;
    
    public void TurnOn()
    {
        isAlive = true;

        spriteRenderer.enabled = true;
    }

    public void TurnOff()
    {
        isAlive = false;

        spriteRenderer.enabled = false;
    }

    public void RunNextStatus()
    {
        if (nextStatus == true)
        {
            TurnOn();
        }
        else
        {
            TurnOff();
        }
    }
}
