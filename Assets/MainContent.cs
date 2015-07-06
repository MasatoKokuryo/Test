using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
O:Room
P:WayPoint
W:Way


OOO     OOO
OPO     OOP
OOO     OOO
 W        W
 W        W
 W        W
OOO       W
OPOWWWWWWWP
OOO
 */

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


	//マップ移動を検証
	internal void CheckMapCollision (Character target) {
	}
	//マップ移動を検証
	internal void CheckObjCollision (Character target) {
	}

}
