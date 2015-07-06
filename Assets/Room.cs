using UnityEngine;
using System.Collections;


enum RoomType{
	NON,
	BOX,
	CIRCLE,
}

//構築用
internal class Room{
	internal Vector3 pos;//中心
	internal float w;//半径なので注意
	internal float h;
	internal WayPoint way = new WayPoint();
	internal RoomType roomType;
	internal GameObject model;
	
	//重複判定
	internal bool isHit(Room target){
		if (roomType == RoomType.NON)
			return false;
		if (Mathf.Abs (pos.x - target.pos.x) < w + target.w &&
		    Mathf.Abs (pos.y - target.pos.y) < h + target.h) {
			return true;
		}
		return false;
	}
	internal void createModel(MainContent parent){
		switch (roomType) {
		case RoomType.BOX:
			model = (GameObject)GameObject.Instantiate(parent.BoxRoomPrefab, pos, Quaternion.identity);
			model.transform.localScale = new Vector3(w*2,1,h*2);
			break;
		case RoomType.CIRCLE:
			model = (GameObject)GameObject.Instantiate(parent.CircleRoomPrefab, pos, Quaternion.identity);
			model.transform.localScale = new Vector3(w*2,1,h*2);
			break;
		}
	}
}
