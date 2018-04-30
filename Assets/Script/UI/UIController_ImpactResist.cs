using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_ImpactResist : MonoBehaviour {


	//オブジェクトplayer呼び出し
	private GameObject myPlayer;
	//衝撃耐性表示テキスト
	Text text;


	void Start () {
		text = GetComponent<Text>(); //自分のインスペクター内からTextコンポーネントを取得
	}


	void Update () {
		this.myPlayer = GameObject.Find("Player");
		float IResistPt = myPlayer.GetComponent<PlayerController>().ImpactResistPt;
		int IR = (int)IResistPt;
		string IRText; //テキスト形式の衝撃耐性値を用意

		IRText = IR.ToString ();
		text.text = IRText + "ブットブー";
	}
}
