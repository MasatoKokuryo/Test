using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using TapjoyUnity;

//TOGのフレームワークマサンプル

public class SampleScene1 : MonoBehaviour {

	void Awake(){
		AdMobManager.Instance.ShowBanner ();
	}

	void OnGUI() {
		//以下デバッグUIの表示

		GUIStyle labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.normal.textColor = Color.white;
		labelStyle.wordWrap = true;
		labelStyle.fontSize = 24;
		
		var centerX = Screen.width / 2;
		var tabHeight = Screen.height / 20;

		var buttonWidth = Screen.width - (Screen.width / 6);
		var buttonHeight = Screen.height / 15;
		
		Rect position;
		float yPosition = Screen.height / 3;

		//オファー
		position = new Rect(centerX - (buttonWidth / 2), yPosition, buttonWidth, buttonHeight);
		if (GUI.Button(position, "広告なし　シーン２へ")) {
			Application.LoadLevel("SampleScene2");
		}
	}
}