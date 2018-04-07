using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// The target marker.
	public Transform target;

	// Angular speed in radians per sec.
	float speed = 1f;

	void Update()
	{
		Vector3 targetDir = Vector3.back;

		// The step size is equal to speed times frame time.
		float step = speed * Time.deltaTime;

		Vector3 newDir = Vector3.RotateTowards(transform.forward, Vector3.down, step, 0.0f);
		Debug.DrawRay(transform.position, newDir, Color.red);

		// Move our position a step closer to the target.
//		transform.rotation = Quaternion.LookRotation(newDir);
		transform.rotation =transform.rotation* new Quaternion(0, 0 , 1 , step);

	}
}
