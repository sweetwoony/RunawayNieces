using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController_SuperButtonCharge04 : MonoBehaviour {

	//プレイヤーのオブジェクト
	private GameObject Player;
	//このオブジェクトのスプライトコンポーネントの情報を、このスクリプト内に収納する
	Image SuperChargeImage;
	//スーパーボタン（仮）の蓄積量を示すスプライトを取得
	public Sprite SuperCharge01;
	public Sprite SuperCharge02;

	void Start () {
		//プレイヤーのオブジェクトを取得
		this.Player = GameObject.Find("Player");
		//このオブジェクトのスプライトを取得
		SuperChargeImage = gameObject.GetComponent<Image>();
	}


	void Update () {
		//衝撃耐性の溜まり方に応じた豚の表情の変化
		float SBCharge = Player.GetComponent<PlayerController>().SuperButtonCharge;
		if (SBCharge > 40) {
			SuperChargeImage.sprite = SuperCharge02;
		} else {
			SuperChargeImage.sprite = SuperCharge01;
		}
	}
}

