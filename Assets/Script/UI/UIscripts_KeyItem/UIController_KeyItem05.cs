using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_KeyItem05 : MonoBehaviour {


	//プレイヤーのオブジェクト
	private GameObject myPlayer;
	//このオブジェクトのスプライトコンポーネントの情報を、このスクリプト内に収納する
	Image GetKeyImage;
	//スーパーボタン（仮）の蓄積量を示すスプライトを取得
	public Sprite Icon_Key01;
	public Sprite Icon_Key02;
	void Start () {
		//プレイヤーのオブジェクトを取得
		this.myPlayer = GameObject.Find("Player");
		//このオブジェクトのスプライトを取得
		GetKeyImage = gameObject.GetComponent<Image>();
	}


	void Update () {
		//衝撃耐性の溜まり方に応じた豚の表情の変化
		bool GK = myPlayer.GetComponent<PlayerController>().GetKey05;
		if (GK) {
			GetKeyImage.sprite = Icon_Key01;
		} else {
			GetKeyImage.sprite = Icon_Key02;
		}
	}
}


