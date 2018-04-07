using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {

	Vector3 moveDirection = Vector3.zero;
	float walkSpeed  = 5.0f;
	float runSpeed = 10.0f;
	float gravity =  1.0f;
	float jumpHeight = 15.0f;
	private CharacterController cc;

	void Start()
	{
		cc = gameObject.GetComponent<CharacterController>();
	}
	void Update()
	{

		if(Input.GetKey(KeyCode.Space))
		{
			moveDirection.y = jumpHeight;
		}

		if (Input.GetKey(KeyCode.LeftShift))
			moveDirection = new Vector3(Input.GetAxis("Horizontal") * walkSpeed, moveDirection.y,
				Input.GetAxis("Vertical") * walkSpeed);
		else
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal") * runSpeed, moveDirection.y,
				Input.GetAxis("Vertical") * runSpeed);

		}
		moveDirection.y -= gravity;
		cc.Move(moveDirection * Time.deltaTime);


	}
    
//    void Update()
//    {
//        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
//        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
//
//        transform.Rotate(0, x, 0);
//        transform.Translate(0, 0, z);
//    }
}
