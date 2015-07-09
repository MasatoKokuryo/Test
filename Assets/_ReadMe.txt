
https://redmine.tayu.jp/projects/product3/wiki/Ad_account


■AdMobの組み込み
ここが分かりやすげ
http://games.genieus.co.jp/unity/admob_unity/

Androidの終了ダイアログ用に以下のプラグインを追加（MIT License）
https://github.com/asus4/UnityNativeDialogPlugin

パッケージをインストールしてサンプルシーン作成
http://games.genieus.co.jp/unity/admob_unity/　からManagerクラスをコピペして追加
google-play-services_lib　をAndroidSDKから最新を投入

AdMobManagerにAdMob管理ページからアプリ毎のKeyを取得し入れる
ca-app-pub-8266025531581562/2329688532
ca-app-pub-8266025531581562/3806421732
テストデバイスIDを調べて入れる(これ入れないと本番扱いで広告タップしまくるとバンされるとか)
38C3D8EAC82710DC73208661ED6B9586

■TapJoyの組み込み

パッケージをインストールしてサンプルシーン起動
メニュー>window>Tapjoyから以下のキーを入力

SDK Key
iOS: Bk9Ek_Z7SkKi33NXM7ThIAEBDdhDypx7nLmwkHGapC0sk_ovGMAuk-kp2VbI
Android: uHRy2uWBRLm0LxqVZOamFgECfvhlNXGr8ESOox0kkZfwC399kttGIkPEbEBe

TapJoySampleAndroidManifestをAndroidManifestへリネーム


■AdMobとTapJoyの組み込み
google-play-services_lib以外は競合しないのでそのまま追加
google-play-services_libはAndroidSDKからインストールした物を利用、TapJoyの物はパッケージインストール時に外す
