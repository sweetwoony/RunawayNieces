using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebug_DamageRecoilTime : MonoBehaviour {


	//オブジェクトplayer呼び出し
	private GameObject myPlayer;
	//衝撃耐性表示テキスト
	Text text;


	void Start () {
		text = GetComponent<Text>(); //自分のインスペクター内からTextコンポーネントを取得
	}


	void Update () {
		this.myPlayer = GameObject.Find("Player");
		float DRT = myPlayer.GetComponent<PlayerController>().DamageRecoilTime;
		int DRTint = (int)DRT;
		string DRTtext; //テキスト形式の衝撃耐性値を用意

		DRTtext = DRTint.ToString ();
		text.text = "DamageRecoilTime : " + DRTtext;
	}
}