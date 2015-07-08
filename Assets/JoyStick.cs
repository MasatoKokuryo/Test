using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	[SerializeField] internal Image targetImage;
	// Use this for initialization
	void Start () {
		Resize ();

		var rectTrans = (transform as RectTransform);
		if (rectTrans == null)
			Debug.LogError ("JoyStick is NULL");
		var imageRectTrans = (targetImage.gameObject.transform as RectTransform);
		if (imageRectTrans == null)
			Debug.LogError ("JoyStickImage is NULL");
	}
	
	// Update is called once per frame
	void Update () {

	}

	//座標とサイズ更新
	private void Resize () {
		var rectTrans = (transform as RectTransform);
		if (rectTrans != null) {
			rectTrans.sizeDelta = PovCollisionSize;
			rectTrans.anchoredPosition = PovCenter;

			var imageRectTrans = (targetImage.gameObject.transform as RectTransform);
			imageRectTrans.sizeDelta = PovImageSize;
		}
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
		if(pos.x > PovWidth)pos.x = PovWidth;
		if(pos.x < -PovWidth)pos.x = -PovWidth;
		if(pos.y > PovHeight)pos.y = PovHeight;
		if(pos.y < -PovHeight)pos.y = -PovHeight;
		(transform as RectTransform).anchoredPosition = new Vector3 (pos.x, pos.y, 0);
	}
	public void OnEndDrag(PointerEventData e){
		Debug.Log ("OnEndDrag");
		Resize ();
	}
	private Vector2 PovCollisionSize{
		get{
			return new Vector2(Screen.width/2, Screen.width/2);
		}
	}
	private Vector2 PovImageSize{
		get{
			return PovCollisionSize/2;
		}
	}
	private Vector2 PovCenter{
		get{
			return PovCollisionSize/2;
		}
	}
	//スティックの可動範囲
	private float PovWidth{
		get{return PovCollisionSize.x/4;}
//		get{return 50;}
	}
	private float PovHeight{
		get{return PovCollisionSize.y/4;}
//		get{return 50;}
	}
	internal Vector3 PovRate{
		get{
			var pos = (transform as RectTransform).anchoredPosition - PovCenter;
			pos.x = pos.x/PovWidth;
			pos.y = pos.y/PovHeight;
			return pos;
		}
	}
}
