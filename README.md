# TestUniRxExtenject <!-- omit in toc --> 
- [環境](#環境)
  - [vscode extention](#vscode-extention)
  - [unity plugins](#unity-plugins)
- [作成方針](#作成方針)
- [Tips](#tips)
## 環境
* Macbook pro 
* homebrew
  * update用コマンド
  * ``brew update; brew upgrade `brew list`; brew upgrade --cask `brew list --cask` ``  
* git
* vscode
* google-chrome
* unity(unityhub)
* gimp
### vscode extention
* C# powered by OmniSharp
* C# Extensions
* C# FixFormat
* C# XML Documentation Comments
* Debugger for Unity
* Git History
* Japanese Language Pack for VS Code
* vscode-icons
* zenkaku
### unity plugins
* TMP
  * Fonts
* UniRx 
  * assetstore
  * https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276
* UniTask
  * github UPM->"add package from git URL"
  * https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
* Extenject
  * assetstore
  * https://assetstore.unity.com/packages/tools/utilities/extenject-dependency-injection-ioc-157735
* DOTween
  * assetstore
  * https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676
* CriWare
  * ADX2 LE 小規模向け無料版
  * https://game.criware.jp/products/adx2-le/#ledl
* Haruko
  * assetstore
  * https://assetstore.unity.com/packages/3d/haruko-69164
* #D Cartoon Box Map
  * assetstore
  * https://assetstore.unity.com/packages/3d/environments/dungeons/3d-cartoon-box-map-50743
## 作成方針
* UniRx + Extenject を使ってMVPパターンで作成する
## Tips
* vscodeのUpdate時の注意  
  1. インテリセンスが効かなくなったらmonoを更新  
  1. それでもダメなら、C#の拡張のバージョンをアップデート前に戻す  
  1. C#の拡張機能はvscodeのsetting.jsonに`"omnisharp.useGlobalMono": "always"`を追加しないとエラー出る
* zoomの更新でエラー出る場合は以下を実行  
  1. `$ brew uninstall zoomus`  
  1. `$ brew reinstall zoom`  
* Mac用 設定追記  
  1. 開発環境導入
  DockからLaunchpad→その他→ターミナルでターミナルを起動  
  以下のコマンドを入力  
  `/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)"`  
  `brew install git`  
  `brew install git-lfs`  
  `brew cask install google-chrome`  
  `brew cask install unity-hub`  
  `brew cask install visual-studio-code`  
  `brew cask install zoom`  
  `brew cask install gimp`  
  1. spotlightのキーバインドを変更  
  システム環境設定→spotlight→キーバインド  
  「spotlightの検索を表示」のチェックを外す  
  1. インテリセンスのサジェストトリガーの設定  
  `command+k -> command+s`  
  以下の文字で検索  
  `editor.action.triggerSuggest`  
  キーバインドを「command+space」に変更  
* Unity設定
  1. VScodeをデフォルトエディターに設定  
 「Edit」タブを開き「Preferences…」をクリック  
 「External Tools」の「External Script Editor」を「VisualStudioCode」に変更  
  1. 「CRIware」の導入  
  「ADX2 LE」を公式サイトよりダウンロード  
  ダウンロードしたファイルを展開  
  Unityにて「Asset」タブを開き「Import Package」=>「Custom Package…」をクリック  
  ダウンロードしたUnityパッケージを導入（チェックマークはデフォルト）  
  1. font導入（M+Fonts）
  TextMesh Pro -> PackageManagerからインストール  
  M+Fonts -> otfファイルは https://mplus-fonts.osdn.jp/about.html よりダウンロード  
  下記のリンク先を参考に導入、※ttfはmplus-2c-regular.ttf※Atlas Resolutionは4096  
  https://hi-network.sakura.ne.jp/wp/2020/04/15/post-1407/  