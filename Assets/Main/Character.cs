using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	[SerializeField] internal JoyStick JoyStick;
	[SerializeField] internal CameraController CameraController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var pov = JoyStick.PovRate;
		var move = new Vector3(pov.x*0.1f,0,pov.y*0.1f);

		var angles = CameraController.transform.rotation.eulerAngles;
		move = Quaternion.AngleAxis (angles.y, new Vector3 (0, 1, 0)) * move;

		transform.localPosition = 
			transform.localPosition + 
			move; 
	}
}
