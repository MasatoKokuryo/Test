using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class JoyStick: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	Vector2 dragStartPos;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnBeginDrag(PointerEventData e){
		Debug.Log ("OnBeginDrag");
	}
	public void OnDrag(PointerEventData e){
		Debug.Log ("OnDrag :" + e.position);
		/*
		var rateX = e.position.x / Screen.width;
		var rateY = e.position.y / Screen.height;
		Debug.Log ("OnDrag :" + rateX + "," + rateY);
		var posX = 640 * rateX - 50;//画面から現在座標を
		var posY = 960 * rateY - 50;
		transform.localPosition = new Vector3(posX, posY, 0);
		*/
		var pos = e.position;
		pos.x = pos.x - 50;
		pos.y = pos.y - 50;
		if(pos.x > PovWidth)pos.x = PovWidth;
		if(pos.x < -PovWidth)pos.x = -PovWidth;
		if(pos.y > PovHeight)pos.y = PovHeight;
		if(pos.y < -PovHeight)pos.y = -PovHeight;
		transform.localPosition = new Vector3(pos.x, pos.y, 0);
	}
	public void OnEndDrag(PointerEventData e){
		Debug.Log ("OnEndDrag");
		transform.localPosition = new Vector3(0, 0, 0);
	}
	internal int PovWidth{
//		get{return Screen.width/2;}
		get{return 50;}
	}
	internal int PovHeight{
//		get{return Screen.height/4;}
		get{return 50;}
	}
	internal Vector3 PovRate{
		get{
			var pos = transform.localPosition;
			pos.x = pos.x/PovWidth;
			pos.y = pos.y/PovHeight;
			return pos;
		}
	}
}
