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
	AdRequest request;

	//アプリ終了時の広告か
	bool quitInterstitialFlag = false; 
	bool isDialog = false; 
	
	// Use this for initialization
	void Awake () {
		// バナー広告を表示
		RequestBanner ();
	}
	
	// Update is called once per frame
	void Update () {
		// エスケープキー取得 何かシステムダイアログでてる内はずっとキーが反応する様子
		if (Input.GetKey(KeyCode.Escape) && !isDialog)
		{
			StartCoroutine(ExitDialog());
		}
	}
	IEnumerator ExitDialog () {
		isDialog = true;
		yield return null;

		// アプリケーション終了
		DialogManager.Instance.SetLabel("Yes", "No", "Close");
		DialogManager.Instance.ShowSelectDialog(
			"終了確認",
			"アプリを終了しますか？",
			(bool result) =>
			{
			if(result){
				quitInterstitialFlag = true;
				CreateInterstitial ();
				currentInterstitial.Show ();
			}
			isDialog = false;
		}
		);
		yield break;
	}
	//インタースティシャル広告を表示
	internal void ShowInterstitial()
	{
		CreateInterstitial ();
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
		BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		AdRequest request = CreateAdRequest();
		// Load the banner with the request.
		bannerView.LoadAd(request);
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
		request = CreateAdRequest();
		// Load the interstitial with the request.
		currentInterstitial.LoadAd (request);
		
		currentInterstitial.AdClosed += HandleAdClosed;
	}

	// インタースティシャル広告を閉じた時に走る
	void HandleAdClosed (object sender, System.EventArgs e)
	{
		currentInterstitial.Destroy ();
		currentInterstitial = null;
		//終了時の広告ならアプリ終了
		if (quitInterstitialFlag) {
			Application.Quit ();
		}
	}
}