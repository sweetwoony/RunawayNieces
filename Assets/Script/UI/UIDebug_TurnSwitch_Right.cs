using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebug_TurnSwitch_Right : MonoBehaviour {


	//オブジェクトplayer呼び出し
	private GameObject myPlayer;
	//衝撃耐性表示テキスト
	Text text;


	void Start () {
		text = GetComponent<Text>(); //自分のインスペクター内からTextコンポーネントを取得
	}


	void Update () {
		this.myPlayer = GameObject.Find ("Player");
		bool RNTS = myPlayer.GetComponent<PlayerController> ().RNormalTurnSwitch;

		if (RNTS) {
			text.text = "ReftTurn : True";
		} else {
			text.text = "ReftTurn : false";
		}
	}
}