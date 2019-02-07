using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////////////
//Spawner script for gameobjects reponsbile for spawning prefabricated enemies//
////////////////////////////////////////////////////////////////////////////////
public class Spawner : LivingEntity
{
    protected Wave[] wave;
    public GameObject[] deathEffects;

    int currentlySpawned = 0;
    int stillToSpawn = 0;
    float timeTillNextSpawn = 0;
    bool constructed = false;

    int currentWaveIndex = -1;
    Wave currentWave;

    GameController controller;

    // Update is called once per frame
    void Update()
    {
        if (constructed)
        {
            if (stillToSpawn > 0 && Time.time > timeTillNextSpawn)
            {
                currentlySpawned++;
                stillToSpawn--;
                timeTillNextSpawn = Time.time + currentWave.spawnTime;
                Vector3 spawnPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);

                Enemy spawnedEnemy = Instantiate(currentWave.wavePrefab, spawnPos, Quaternion.identity) as Enemy;
                spawnedEnemy.SetSpawner(this);
                spawnedEnemy.controller = this.controller;
            }

            if (currentlySpawned == 0 && stillToSpawn == 0)
                NextWave();
        }
    }

    //Creates a spawner//
    public void construct(Wave[] wave, GameController controller)
    {
        this.wave = wave;
        this.controller = controller;
        constructed = true;
    }

    //Increments wave counter, if final wave, removes spawner//
    void NextWave()
    {
        if(currentWaveIndex + 1 < wave.Length)
        {
            currentWaveIndex++;
            currentWave = wave[currentWaveIndex];
            stillToSpawn = currentWave.maxSpawnCount;
        }
        else
        {
            Die();
        }
    }

    public void RegisterDeadEnemy()
    {
        currentlySpawned--;
    }

    //Removes the spawner from the game and registers its removal//
    protected override void Die()
    {

        for (int i = 0; i < deathEffects.Length; i++)
        {
            Destroy(Instantiate(deathEffects[i], new Vector3(this.transform.position.x, this.transform.position.y + (0.25f * i) + (1.5f), this.transform.position.z), Quaternion.identity) as GameObject, 2.5f);
        }

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        controller.registerDeadSpawner();
        base.Die();
    }
}
