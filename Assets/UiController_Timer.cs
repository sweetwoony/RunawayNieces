using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController_Timer : MonoBehaviour {

	//経過時間テキスト
	Text text;
	//経過時間を記録する変数
	private float ElapsedTime = 0;

	void Start () {
		this.ElapsedTime = 0; //時間を初期化
		text = GetComponent<Text>(); //自分のインスペクター内からTextコンポーネントを取得

	}
	
	// Update is called once per frame
	void Update () {
		ElapsedTime += Time.deltaTime; //毎フレームの時間を加算

		int minute = (int)ElapsedTime / 60; //分。ElapsedTimeを60で割った値。
		int second = (int)ElapsedTime % 60; //秒。ElapsedTimeを60で割った余り。

		string minText; //テキスト形式の秒を用意
		string secText; //テキスト形式の秒を用意

		//ToStringで int から string に変換
		if (minute < 10) {
			minText = "0" + minute.ToString (); 
		} else {
			minText = minute.ToString ();
		}

		//ToStringで int から string に変換
		if(second < 10){
			secText = "0" + second.ToString (); 
		} else {
			secText = second.ToString ();
		}

		text.text = minText + ":" + secText;
	}
}
