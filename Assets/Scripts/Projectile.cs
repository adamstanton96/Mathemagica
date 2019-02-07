using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////////////////////////////////////////
//Collidable projectile script for use in shooting mechanics//
//////////////////////////////////////////////////////////////

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    float speed = 10;
    float damage = 1.0f;
    string attackValue;

    float lifetime = 5.0f;

    public GameObject[] hitEffects;
    public AudioSource[] sounds;

    // Use this for initialization
    void Start ()
    {
        Destroy(gameObject, lifetime);
        Vector3 pos = this.transform.position;
        AudioSource sound = Instantiate(sounds[0], pos, Quaternion.identity);
        sound.Play();
        Destroy(sound, 1.0f);
	}
	
    public void SetProjectileSpeed(float ispeed)
    {
        speed = ispeed;
    }

    public void SetProjectileDamage(float idamage)
    {
        damage = idamage;
    }

    public void SetProjectileAttackValue(string iattackValue)
    {
        attackValue = iattackValue;
    }

    // Update is called once per frame
    void Update ()
    {
        float moveDistance = speed * Time.deltaTime; //Calculates how far the projectile will move this frame.
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        //Register the hit and perform relevant operations...
        IDamagable damagableObject = hit.collider.GetComponent<IDamagable>();
        if (damagableObject != null)
            damagableObject.TakeHit(damage, hit, attackValue);
        //Destroy the projectile...
        GameObject.Destroy(gameObject);
    }

    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        //Register the hit and perform relevant operations...
        for (int i = 0; i < hitEffects.Length; i++)
            Destroy(Instantiate(hitEffects[i], hitPoint, Quaternion.FromToRotation(Vector3.forward, transform.forward)) as GameObject, 0.5f);

        IDamagable damagableObject = c.GetComponent<IDamagable>();
        if (damagableObject != null)
        {
            damagableObject.TakeHit(damage, hitPoint, transform.forward, attackValue);
        }
        //Destroy the projectile...
        GameObject.Destroy(gameObject);
    }
}
