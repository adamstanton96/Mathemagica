using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////
//Abstract definition for gameobjects with a death sequence//
/////////////////////////////////////////////////////////////

public class LivingEntity : MonoBehaviour, IDamagable {

    public float totalHealth;
    protected float health;
    protected bool dead;

    public event System.Action OnDeath;

    protected string solution;

    protected virtual void Start()
    {
        health = totalHealth;
    }

    public virtual void TakeHit(float damage, RaycastHit hit, string attackValue)
    {
        if (solution.CompareTo(attackValue) == 0)
        {
            TakeDamage(damage);
        }
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir, string attackValue)
    {
        if (solution.CompareTo(attackValue) == 0)
        {
            TakeDamage(damage);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        //Debug.Log (health);
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        dead = true;
        GameObject.Destroy(gameObject);
    }

}
