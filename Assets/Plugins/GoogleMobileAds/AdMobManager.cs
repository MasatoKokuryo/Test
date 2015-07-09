using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;


public class AdMobManager : MonoBehaviour {

	[SerializeField] string Android_Banner;
	[SerializeField] string Android_Interstitial;
	[SerializeField] string ios_Banner;
	[SerializeField] string ios_Interstitial;
	//テスト用デバイスの登録
	[SerializeField] List<string> Android_TestDevices;

	InterstitialAd currentInterstitial;
	BannerView currentBannerView; 
	
	//アプリ終了時の広告か
	bool quitInterstitialFlag = false; 
	bool isDialog = false; 

	//シングルトン用
	public static AdMobManager Instance {
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

		// ロードが終わってないと表示できず、ロードに時間かかるので先読みする
		CreateInterstitial ();
		// バナー広告を表示
		RequestBanner ();
	}
	
	// Update is called once per frame
	void Update () {
		// エスケープキー取得 何かシステムダイアログでてる内はずっとキーが反応する様子
		if (Input.GetKeyDown(KeyCode.Escape) && !isDialog)
		{
			Debug.Log("AdMobQuit");
			isDialog = true;
			quitInterstitialFlag = true;
			currentInterstitial.Show ();
			/*
			// アプリケーション終了
			DialogManager.Instance.SetLabel("Yes", "No", "Close");
			DialogManager.Instance.ShowSelectDialog(
				"終了確認",
				"アプリを終了しますか？",
				(bool result) =>
				{
				if(result){
					quitInterstitialFlag = true;
//					CreateInterstitial ();
					currentInterstitial.Show ();
				}
				isDialog = false;
			}
			*/
		}
	}

	//インタースティシャル広告を表示
	public void ShowInterstitial()
	{
		currentInterstitial.Show ();
	}


	private AdRequest CreateAdRequest(){
		// リクエストの生成
		//　テスト端末設定があった場合は取得
		var builder = new AdRequest.Builder ();
		builder.AddTestDevice (AdRequest.TestDeviceSimulator);
		if (Android_TestDevices != null) {
			foreach(var str in Android_TestDevices){
				builder.AddTestDevice(str);//MyAndroid
			}
		}
		return builder.Build ();
	}

	//バナー読み込み
	private void RequestBanner()
	{
		#if UNITY_ANDROID
		string adUnitId = Android_Banner;
		#elif UNITY_IPHONE
		string adUnitId = ios_Banner;
		#else
		string adUnitId = "unexpected_platform";
		#endif
		
		// Create a 320x50 banner at the top of the screen.
		currentBannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = CreateAdRequest();
		// Load the banner with the request.
		currentBannerView.LoadAd(request);
	}
	public void ShowBanner()
	{
		currentBannerView.Show ();
	}
	public void HideBanner()
	{
		currentBannerView.Hide ();
	}
	
	private void CreateInterstitial()
	{
		//一個だけ
		if (currentInterstitial != null) {
			currentInterstitial.Destroy();
		}
		#if UNITY_ANDROID
		string adUnitId = Android_Interstitial;
		#elif UNITY_IPHONE
		string adUnitId = ios_Interstitial;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Initialize an InterstitialAd.
		currentInterstitial = new InterstitialAd (adUnitId);
		var request = CreateAdRequest();
		// Load the interstitial with the request.
		currentInterstitial.LoadAd (request);
		
		currentInterstitial.AdClosed += HandleAdClosed;
	}

	// インタースティシャル広告を閉じた時に走る
	void HandleAdClosed (object sender, System.EventArgs e)
	{
		currentInterstitial.Destroy ();
		currentInterstitial = null;
		if (quitInterstitialFlag) {
			//終了時の広告ならアプリ終了
			Application.Quit ();
		} else {
			//終了出ない場合は次のを作っておく
			CreateInterstitial ();
		}
	}
}