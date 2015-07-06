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
public class MainContent : MonoBehaviour {

	//定義===================================
	const int MAP_SIZE = 64;
	
	const int MAX_ROOM_NUM = 6;
	const int MIN_ROOM_NUM = 1;

	const int MAX_ROOM_SIZE = 16;
	const int MIN_ROOM_SIZE = 1;
	
	//シリアライズ===================================
	//へや
	[SerializeField] internal GameObject BoxRoomPrefab;
	[SerializeField] internal GameObject CircleRoomPrefab;
	//つうろ
	[SerializeField] internal GameObject WayPrefab;
	//つうろ
	[SerializeField] internal Character PlayerCharacter;

	//メンバ===================================
	GameObject[] Walls;
	Room[] Rooms;


	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	void Init () {

		Debug.Log ("SEED :" + Random.seed);
		Rooms = new Room[ Random.Range(MIN_ROOM_NUM, MAX_ROOM_NUM) ];
		Debug.Log ("Order RoomNum:" + Rooms.Length);

		//部屋生成
		int retryCount = 0;
		int roomCount = 0;
		for (roomCount=0; roomCount < Rooms.Length; ) {
			if(Rooms[roomCount] == null){
				Rooms[roomCount] = new Room();
			}
			var room = Rooms[roomCount];
			room.roomType = Random.Range(0,2) == 0 ? RoomType.BOX : RoomType.BOX;
			room.pos.x = Random.Range(MIN_ROOM_SIZE, MAP_SIZE-MIN_ROOM_SIZE);
			room.pos.z = Random.Range(MIN_ROOM_SIZE, MAP_SIZE-MIN_ROOM_SIZE);
			var height = Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
			room.h = height+0.5f;
			var width = Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
			room.w = width+0.5f;
			if(room.roomType == RoomType.CIRCLE){
				room.h = room.w;
			}else{
				room.h = Random.Range(MIN_ROOM_SIZE, MAX_ROOM_SIZE);
			}
			room.way.pos.x = room.pos.x + (int)Random.Range(-room.w+1, room.w);
			room.way.pos.z = room.pos.z + (int)Random.Range(-room.h+1, room.h);

			//重複する？
			bool isHit = false; 
			foreach (var room2 in Rooms) {
				if(room == room2)break;
				isHit = room.isHit(room2);
				if(isHit)break;
			}
			if(isHit){
				retryCount++;
				if(retryCount > 32){
					room.roomType = RoomType.NON;
					break;
				}
				continue;
			}
			Debug.Log ("Room[" + roomCount + "]" + room.pos + ":" + room.way.pos);
			//一つ前と、確率でどっかの部屋とつながる
			if(roomCount >= 1){
				Rooms[roomCount-1].way.AddWay(room.way);
				if(roomCount >= 2 && Random.Range(0,2) == 0){
					var index = (int)Random.Range(0, roomCount-1);
					Rooms[index].way.AddWay(room.way);
				}
			}
			
			++roomCount;
			retryCount = 0;
		}
		//ルーム数を調整
		var tempRooms = Rooms;
		Rooms = new Room[ roomCount ];
		for (int i=0; i < Rooms.Length; ++i) {
			Rooms[i] = tempRooms[i];
		}
		Debug.Log("RoomNum:" + Rooms.Length);
		
		//モデル作成
		foreach (var room in Rooms) {
			room.createModel(this);
		}
		Rooms [0].way.createModel (this);

		//自キャラ配置
		PlayerCharacter.transform.position = Rooms [0].way.pos;
		
	}
}
