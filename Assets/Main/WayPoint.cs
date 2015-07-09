using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//みち
internal class WayPoint{
	internal Vector3 pos;//
	
	internal Room Room;
	internal List<WayPoint> link = new List<WayPoint>();
	internal List<GameObject> model = new List<GameObject>();//自身から次への道

	internal void AddWay(WayPoint point){
		//中継必要？
		if (pos.x != point.pos.x && pos.z != point.pos.z) {
			var midPoint = new WayPoint ();

			//　「型か　」型か
			if(Random.Range(0,2) == 0){
				midPoint.pos.x = pos.x;
				midPoint.pos.z = point.pos.z;
			}else{
				midPoint.pos.x = point.pos.x;
				midPoint.pos.z = pos.z;
			}
			AddWay (midPoint);
			midPoint.AddWay (point);
			return;
		} else {
			link.Add (point);
			point.link.Add (this);
		}
	}
	internal void createModel(MainContent parent){

		//実際に部屋の真ん中から道を作ると道と重なってしまうので部屋の端に寄せる


		var wayModel = (GameObject)GameObject.Instantiate(parent.WayPrefab, pos, Quaternion.identity);
		wayModel.name = "WayPoint";
		model.Add(wayModel);
		foreach(var way in link){
			if(way.model.Count > 0){
				continue;//作成済み
			}
			//みち
			wayModel = (GameObject)GameObject.Instantiate(parent.WayPrefab, (way.pos + pos)/2, Quaternion.LookRotation(way.pos - pos));
			var modPos = way.pos - pos;
			wayModel.transform.localScale = new Vector3(1,1, modPos.magnitude-1);
			wayModel.name = "Way";
			model.Add(wayModel);

			way.createModel(parent);
		}
	}
}
