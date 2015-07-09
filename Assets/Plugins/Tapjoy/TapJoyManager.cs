using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using TapjoyUnity;

//TOGのフレームワークマネージャー
//TapJoy用

public class TapJoyManager : MonoBehaviour {
	[SerializeField] public bool viewIsShowing = false;
	private bool isConnected = false;

	//シングルトン用
	public static TapJoyManager Instance {
		get;
		private set;
	}
	// Use this for initialization
	void Awake () {
		if (Instance == null) {
			//常駐
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else {
			Destroy(gameObject);
			return;
		}
	}
	void Start() {
		// Connect Delegates
		Tapjoy.OnConnectSuccess += HandleConnectSuccess;
		Tapjoy.OnConnectFailure += HandleConnectFailure;
		
		// Tapjoy View Delegates
		Tapjoy.OnViewWillOpen += HandleViewWillOpen;
		Tapjoy.OnViewDidOpen += HandleViewDidOpen;
		Tapjoy.OnViewWillClose += HandleViewWillClose;
		Tapjoy.OnViewDidClose += HandleViewDidClose;
	}

	void OnDisable()
	{
		// Connect Delegates
		Tapjoy.OnConnectSuccess -= HandleConnectSuccess;
		Tapjoy.OnConnectFailure -= HandleConnectFailure;

		// Tapjoy View Delegates
		Tapjoy.OnViewWillOpen -= HandleViewWillOpen;
		Tapjoy.OnViewDidOpen -= HandleViewDidOpen;
		Tapjoy.OnViewWillClose -= HandleViewWillClose;
		Tapjoy.OnViewDidClose -= HandleViewDidClose;
	}

	#region Connect Delegate Handlers
	public void HandleConnectSuccess() {
		Debug.Log("C#: Handle Connect Success");
		isConnected = true;
	}
	
	public void HandleConnectFailure() {
		Debug.Log("C#: Handle Connect Failure");
	}
	#endregion

	#region View Delegate Handlers
	public void HandleViewWillOpen(int viewType) {
		Debug.Log("C#: HandleViewWillOpen, viewType: " + viewType);
	}
	
	public void HandleViewDidOpen(int viewType) {
		Debug.Log("C#: HandleViewDidOpen, viewType: " + viewType);
		viewIsShowing = true;
	}
	
	public void HandleViewWillClose(int viewType) {
		Debug.Log("C#: HandleViewWillClose, viewType: " + viewType);
	}
	
	public void HandleViewDidClose(int viewType) {
		Debug.Log("C#: HandleViewDidClose, viewType: " + viewType);
		viewIsShowing = false;
	}
	#endregion

	#region Global UI for Sample app

	void OnGUI() {
		// TODO: デバッグでない場合は表示しないように注意
		if (viewIsShowing) {
			return;
		}

		//以下デバッグUIの表示

		GUIStyle labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.normal.textColor = Color.white;
		labelStyle.wordWrap = true;
		labelStyle.fontSize = 24;
		
		var centerX = Screen.width / 2;
		var tabWidth = Screen.width / 3;
		var tabHeight = Screen.height / 20;
		var yPadding = tabHeight + 10;

		var buttonWidth = Screen.width - (Screen.width / 6);
		var buttonHeight = Screen.height / 15;
		
		Rect position;
		float yPosition = 0;

		//オファー
		position = new Rect(centerX - (buttonWidth / 2), yPosition, buttonWidth, buttonHeight);
		if (GUI.Button(position, "Show Offerwall")) {
			Tapjoy.ShowOffers();
		}

		//繋がってない
		if (!isConnected) {
			yPosition += yPadding;
			position = new Rect(centerX - 200, yPosition, 400, 25);
			GUI.Label(position, "Trying to connect to Tapjoy...", labelStyle);
		}
	}

	#endregion
}