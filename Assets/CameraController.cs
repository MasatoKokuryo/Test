using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	[SerializeField] internal Character PlayerCharacter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = (transform.position + 
		                      PlayerCharacter.transform.position) / 2;
	}
}
