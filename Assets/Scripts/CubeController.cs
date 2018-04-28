using UnityEngine;

public class CubeController : MonoBehaviour {
	public Vector3 Position { get; private set; }

	// Use this for initialization
	void Start () {
		Position = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
