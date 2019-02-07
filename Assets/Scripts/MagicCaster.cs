using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCaster : MonoBehaviour {

    public Transform projectileOrigin;
    public Projectile projectile;
    public float projectileSpeed = 50f;
    public float timeBetweenShots = 0.25f;

    float nextCastTime;

    public void Cast(string attackValue)
    {
        if(Time.time > nextCastTime)
        {
            nextCastTime = Time.time + timeBetweenShots;
            Projectile newProjectile = Instantiate(projectile, projectileOrigin.position, projectileOrigin.rotation) as Projectile;
            newProjectile.SetProjectileSpeed(projectileSpeed);
            newProjectile.SetProjectileAttackValue(attackValue);
        }
    }
}
