using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_Score : MonoBehaviour {

	//オブジェクトplayer呼び出し
	private GameObject myPlayer;
	//スコア表示テキスト
	Text text;


	void Start () {
		text = GetComponent<Text>(); //自分のインスペクター内からTextコンポーネントを取得
	}


	void Update () {
		this.myPlayer = GameObject.Find("Player");
		float SP = myPlayer.GetComponent<PlayerController>().ScorePt;
		int SP_int = (int)SP;
		string SP_string; //テキスト形式の衝撃耐性値を用意

		SP_string = SP_int.ToString ();
		text.text = "SCORE : " + SP_string;
	}
}
