using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/////////////////////////////////////////////////////////
//Enemy class used at the core of varying enemy prefabs//
/////////////////////////////////////////////////////////

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(TextMesh))]
public class Enemy : LivingEntity
{
    NavMeshAgent pathFinder;
    Transform target;
    Spawner spawner;
    AbstractNode formulaTree;

    LivingEntity targetEntity;

    public TextMesh formulaMesh;

    public GameObject[] deathEffects;
    public GameObject[] shieldEffects;
    public GameObject[] bleedEffects;
    public AudioSource[] sounds;

    public GameController controller;

    public int difficulty;
    private int solutuionBounds = 50;
    private float attackDistanceThreshold = .75f;
    private float sqrAttackDistanceThreshold;
    private float timeBetweenAttacks = 1.0f;
    private float nextAttackTime = 0.0f;

    float collisionRadius, targetCollisionRadius;

    public enum State { Idle, Chasing, Attacking}
    State currentState;

    //bool hasTarget = false;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            //hasTarget = true;

            generateFormula(solutuionBounds, this.difficulty);
            formulaMesh.text = formulaTree.ToString();

            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            currentState = State.Chasing;
            collisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath()); //Less operation heavy than previous method.

            sqrAttackDistanceThreshold = Mathf.Pow(attackDistanceThreshold + collisionRadius + targetCollisionRadius, 2);
        }
        else
        {
            Debug.LogError("Enemy has no player target.");
        }
    }
	
    void OnTargetDeath()
    {
        //hasTarget = false;
        currentState = State.Idle;
    }

	// Update is called once per frame
	void Update ()
    {
        if (controller != null)
        {
            if (target != null)
            {
                if (Time.time > nextAttackTime)
                {
                    float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
                    if (sqrDistanceToTarget < sqrAttackDistanceThreshold)
                    {
                        nextAttackTime = Time.time + timeBetweenAttacks;
                        StartCoroutine(Attack());
                    }
                }
            }
            //pathFinder.SetDestination(target.position); //Operation heavy...
        }
	}

    public void SetSpawner(Spawner spawner)
    {
        this.spawner = spawner;
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathFinder.enabled = false;

        Vector3 originalPos = transform.position;
        Vector3 targetDir = (target.position - transform.position).normalized;
        Vector3 attackPos = target.position - targetDir * (collisionRadius);

        float attackSpeed = 3.0f;
        float percent = 0;
        bool hasAppliedDamage = false;

        while(percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                targetEntity.TakeDamage(1.0f);
                hasAppliedDamage = true;
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent,2) + percent) *4;
            transform.position = Vector3.Lerp(originalPos, attackPos, interpolation);
            yield return null;
        }

        pathFinder.enabled = true;
        currentState = State.Chasing;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 0.25f;

        while(target != null)
        {
            if (currentState == State.Chasing)
            {
                Vector3 targetDir = (target.position - transform.position).normalized;
                Vector3 targetPos = target.position - targetDir * (collisionRadius + targetCollisionRadius + attackDistanceThreshold * .75f);
                if (!dead)
                    pathFinder.SetDestination(targetPos);
            }
                yield return new WaitForSeconds(refreshRate);
        }
    }

    protected override void Die()
    {
        if (this.spawner != null)
            this.spawner.RegisterDeadEnemy();
        base.Die();
    }

    protected void generateFormula(int solutionBounds, int difficulty)
    {
        formulaTree = AbstractNode.createTree(Difficulty.getRandomInt(solutionBounds), difficulty);
        solution = formulaTree.getValue().ToString();
        //Debug.Log(formulaTree + " = " + formulaTree.getValue());   
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDir, string attackValue)
    {
        if (damage >= this.health && solution.CompareTo(attackValue) == 0)
        {
            for(int i = 0; i < deathEffects.Length; i++)
                Destroy(Instantiate(deathEffects[i], hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDir)) as GameObject, 2.0f);

            if (sounds != null)
            {
                Vector3 pos = this.transform.position;
                AudioSource deathSound = Instantiate(sounds[0], pos, Quaternion.identity);
                deathSound.time = .75f;
                deathSound.Play();
                Destroy(deathSound, 2.5f);
            }

            controller.incrementCorrectSolutions();

            base.TakeHit(damage, hitPoint, hitDir, attackValue);
        }
        else if (solution.CompareTo(attackValue) == 0)
        {
            for (int i = 0; i < bleedEffects.Length; i++)
                Destroy(Instantiate(bleedEffects[i], hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDir)) as GameObject, 1.0f);

            controller.incrementCorrectSolutions();

            base.TakeHit(damage, hitPoint, hitDir, attackValue);

            if (this.difficulty < 5)
                this.difficulty++;

            generateFormula(solutuionBounds, this.difficulty);
            formulaMesh.text = formulaTree.ToString();
        }
        else
        {
            for (int i = 0; i < shieldEffects.Length; i++)
                Destroy(Instantiate(shieldEffects[i], hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDir)) as GameObject, 1.0f);

            if (sounds[1] != null)
                sounds[1].Play();

            controller.incrementIncorrectSolutions();

            base.TakeHit(damage, hitPoint, hitDir, attackValue);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}
