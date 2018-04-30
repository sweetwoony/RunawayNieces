using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDebug_SuperButtonCharge : MonoBehaviour {


	//オブジェクトplayer呼び出し
	private GameObject myPlayer;
	//衝撃耐性表示テキスト
	Text text;


	void Start () {
		text = GetComponent<Text>(); //自分のインスペクター内からTextコンポーネントを取得
	}


	void Update () {
		this.myPlayer = GameObject.Find("Player");
		float SBC = myPlayer.GetComponent<PlayerController>().SuperButtonCharge;
		int SBCint = (int)SBC;
		string SBCtext; //テキスト形式のスーパーボタン蓄積値を用意

		SBCtext = SBCint.ToString ();
		text.text = "SuperButtonCharge : " + SBCtext;



	}
}