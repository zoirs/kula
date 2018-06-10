using UnityEngine;

public class DetectClick : MonoBehaviour {

	public Canvas menu;
	
	void OnMouseDown() {
		menu.gameObject.SetActive(false);
		Debug.Log("11111");
	}
}
