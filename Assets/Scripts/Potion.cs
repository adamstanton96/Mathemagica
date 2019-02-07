using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////
//Potion pickup for health restoration//
////////////////////////////////////////

public class Potion : Collectable
{
    public AudioSource pickupSound;

    protected override void PerformTask()
    {
        if (this.controller.player.addHealth())
        {
            if (pickupSound != null)
            {
                Vector3 pos = this.transform.position;
                AudioSource deathSound = Instantiate(pickupSound, pos, Quaternion.identity);
                deathSound.Play();
                Destroy(deathSound, 2.5f);
            }

            this.controller.registerPotionPickup();
            Die();
        }
    }
}
