using System;
using UnityEngine;
using UnityEngine.AI;

[Obsolete("Похоже пока нет нужды")]
public class CubeController : MonoBehaviour {
	private NavMeshLink[] _angelLinks;
	private NavMeshLink[] _surfaceLinks;
	public Vector3 Position { get; private set; }

	// Use this for initialization
	void Start () {
		Position = gameObject.transform.position;
		
		GameObject angelLinks = GameObject.Find("Angel");
		_angelLinks = angelLinks.GetComponentsInChildren<NavMeshLink>();
		
		GameObject surfaceLinks = GameObject.Find("Angel");
		_surfaceLinks = surfaceLinks.GetComponentsInChildren<NavMeshLink>();
	}

	public void DisableLinks() {
		foreach (NavMeshLink link in _angelLinks) {
			link.enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
