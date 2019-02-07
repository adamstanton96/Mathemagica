using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/////////////////
//Player Script//
/////////////////

[RequireComponent (typeof (PlayerController))]
[RequireComponent(typeof(MagicController))]
[RequireComponent(typeof(TextInput))]
[RequireComponent(typeof(Text))]
public class Player : LivingEntity {

    Camera viewCamera;
    public float moveSpeed = 5;
    PlayerController controller;
    MagicController magicController;
    TextInput textInput;

    public Text currentSolutionText;
    public Text currentInputText;

    string attackValue;

    public AudioSource[] sounds;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();

        controller = GetComponent<PlayerController>();
        magicController = GetComponent<MagicController>();
        textInput = GetComponent<TextInput>();

        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update ()
    {
        //Movement and Rotation:
        //Gets movement for player object...
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);
        //Gets the player rotation using a raycast from camera...
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (plane.Raycast(ray,out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            controller.LookAt(point);
        }

        //Spellcasting Input:
        if(Input.GetMouseButton(0))
        {
            magicController.Cast(attackValue);
        }

        if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            setAttackValue();
        }

        currentInputText.text = textInput.getInputString();
    }

    void setAttackValue()
    {
        attackValue = textInput.getInputString();
        textInput.clear();
        currentSolutionText.text = attackValue;
    }

    public bool addHealth()
    {
        if (this.health < this.totalHealth)
        {
            this.health++;
            return true;
        }
        else
            return false;
    }

    public float getHealth()
    {
        return this.health;
    }

    public override void TakeDamage(float damage)
    {
        if(health > 1)
        {
            if (sounds != null)
            {
                Vector3 pos = this.transform.position;
                AudioSource deathSound = Instantiate(sounds[0], pos, Quaternion.identity);
                deathSound.Play();
                Destroy(deathSound, 2.5f);
            }
        }
        else
        {
            if (sounds != null)
            {
                Vector3 pos = this.transform.position;
                AudioSource deathSound = Instantiate(sounds[1], pos, Quaternion.identity);
                deathSound.Play();
                Destroy(deathSound, 2.5f);
            }
        }
        base.TakeDamage(damage);
    }
}
