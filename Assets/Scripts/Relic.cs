using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////////////
//Pickup used to control game progression//
///////////////////////////////////////////

public class Relic : Collectable
{
    public AudioSource pickupSound;

    protected override void PerformTask()
    {
        if (pickupSound != null)
        {
            Vector3 pos = this.transform.position;
            AudioSource deathSound = Instantiate(pickupSound, pos, Quaternion.identity);
            deathSound.Play();
            Destroy(deathSound, 2.5f);
        }

        this.controller.registerRelicPickup();
        Die();
    }
}
