using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////
//Abstract Collectable Object Script// 
//////////////////////////////////////

public abstract class Collectable : LivingEntity
{
    protected GameController controller;

    public void contruct(GameController controller)
    {
        this.controller = controller;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this.controller != null)
        {
            if (collision.collider.tag == "Player")
            {
                //Debug.Log("Task Performing...");
                PerformTask();
            }
        }
    }

    protected abstract void PerformTask();
}
