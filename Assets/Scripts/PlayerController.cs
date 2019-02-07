using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour {

    Rigidbody rigidBody;
    Vector3 velocity;

	// Use this for initialization
	void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// 
	public void Move(Vector3 ivelocity)
    {
        velocity = ivelocity;
	}

    public void LookAt(Vector3 target)
    {
        Vector3 heightCorrectedTarget = new Vector3(target.x, transform.position.y, target.z);
        transform.LookAt(heightCorrectedTarget);
    }

    // 
    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
    }
}
