using UnityEngine;

public class CubeController : MonoBehaviour {
	public Vector3 Position { get; private set; }

	void Start () {
		Position = gameObject.transform.position;
	}
}
